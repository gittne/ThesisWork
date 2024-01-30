using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance { get; private set; }
    private void Awake() { Instance = this; }

    [SerializeField] TextMeshProUGUI lobbyNameText;

    [SerializeField] string playerName;

    [SerializeField] string lobbyName;

    public const string KEY_PLAYER_NAME = "PlayerName";
    public const string KEY_GAME_MODE = "GameMode";
    public const string KEY_START_GAME = "Start";

    public event EventHandler OnLeftLobby;

    public event EventHandler<LobbyEventArgs> OnJoinedLobby;
    public event EventHandler<LobbyEventArgs> OnJoinedLobbyUpdate;
    public event EventHandler<LobbyEventArgs> OnKickedFromLobby;

    public event EventHandler<EventArgs> OnGameStarted;

    [Space(30)]
    [Header("Display ThingyMajingies")]
    [SerializeField] TextMeshProUGUI lobbyCodeDisplay;
    [SerializeField] TextMeshProUGUI lobbyNameDisplay;
    [SerializeField] GameObject lobbyCodeInput;
    [SerializeField] TextMeshProUGUI player1NameDisplay;
    [SerializeField] TextMeshProUGUI player2NameDisplay;

    [SerializeField] GameObject lobbyMainDisplay;
    [SerializeField] GameObject lobbyCodeDisplayObject;

    string attemptJoinCode;

    public class LobbyEventArgs : EventArgs
    {
        public Lobby lobby;
    }

    private Lobby joinedLobby;
    private float heartbeatTimer;
    private float lobbyPollTimer;

    public class OnLobbyListChangedEventArgs : EventArgs
    {
        public List<Lobby> lobbyList;
    }

    public enum GameMode
    {
        Difficulty_Normal,
        Difficulty_Hard
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
        HandleLobbyPolling();
    }

    private void OnApplicationQuit()
    {
        if(joinedLobby != null) LeaveLobby();
    }

    public void GoAuthenticate()
    {
        Authenticate(EditPlayerName.Instance.GetPlayerName());
    }

    public async void Authenticate(string name)
    {
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(EditPlayerName.Instance.GetPlayerName());

        await UnityServices.InitializeAsync(initializationOptions);

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in with ID: " + AuthenticationService.Instance.PlayerId + "\n and username: " + EditPlayerName.Instance.GetPlayerName());
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public void SetLobbyStats()
    {
        lobbyName = lobbyNameText.text;
    }

    private Player GetPlayer()
    {
        return new Player(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject> {
            { KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) }
        });
    }

    public async void CreateLobby()
    {
        Player player = GetPlayer();
        GameMode gm = GameMode.Difficulty_Normal;

        CreateLobbyOptions options = new CreateLobbyOptions
        {
            Player = player,
            IsPrivate = false,
            Data = new Dictionary<string, DataObject> {
                { KEY_GAME_MODE, new DataObject(DataObject.VisibilityOptions.Public, gm.ToString()) },
                { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0") }
            }
        };

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, 2, options);

        joinedLobby = lobby;

        OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });

        Debug.Log("Lobby successfully created: " + lobby.LobbyCode);
        lobbyCodeDisplay.text = lobby.LobbyCode;
        RefreshLobbyDisplay();
    }

    public async void JoinLobby()
    {
        Debug.Log("I AM TRYNA JOIN WITH THIS CODE: " + attemptJoinCode);
        try {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer(),
            };
            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(attemptJoinCode, joinLobbyByCodeOptions);

            lobbyMainDisplay.SetActive(true);
            lobbyCodeDisplayObject.SetActive(true);

            Debug.Log("Joined lobby with code: " + lobbyCodeDisplay.text.ToUpper());

            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });

            joinedLobby = lobby;

            RefreshLobbyDisplay();

        } catch(LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }

    public async void LeaveLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

                joinedLobby = null;

                OnLeftLobby?.Invoke(this, EventArgs.Empty);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    private void RefreshLobbyDisplay()
    {
        Player player = GetPlayer();

        lobbyNameDisplay.text = joinedLobby.Name;
        lobbyCodeDisplay.text = joinedLobby.LobbyCode;

        player1NameDisplay.text = joinedLobby.Players[0].Data["PlayerName"].Value;
        player2NameDisplay.text = "";

        if (joinedLobby.Players.Count > 1)
        {
            player2NameDisplay.text = joinedLobby.Players[1].Data["PlayerName"].Value;
        }
    }

    public void SetLobbyJoinCode()
    {
        attemptJoinCode = lobbyCodeInput.GetComponent<TMP_InputField>().text.ToUpper().ToSafeString();
    }

    public async void UpdatePlayerName(string playerName)
    {
        this.playerName = playerName;

        if (joinedLobby != null)
        {
            try
            {
                UpdatePlayerOptions options = new UpdatePlayerOptions();

                options.Data = new Dictionary<string, PlayerDataObject>() {
                    {
                        KEY_PLAYER_NAME, new PlayerDataObject(
                            visibility: PlayerDataObject.VisibilityOptions.Public,
                            value: playerName)
                    }
                };

                Debug.Log("Set the username to " + this.playerName);
                string playerId = AuthenticationService.Instance.PlayerId;

                Lobby lobby = await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, playerId, options);
                joinedLobby = lobby;

                OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void StartGame()
    {
        if(IsLobbyHost() && joinedLobby.Players.Count > 1)
        {
            try
            {
                Debug.Log("Starting the game.");

                string relayCode = await RelayMaker.Instance.CreateRelay();

                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                {
                    { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
                }
                });

                joinedLobby = lobby;
            } catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
        else
        {
            Debug.Log("not enough players to start the lobby.");
        }
    }

    public Lobby GetJoinedLobby() { return joinedLobby; }

    public bool IsLobbyHost() { return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId; }

    private bool IsPlayerInLobby()
    {
        if (joinedLobby != null && joinedLobby.Players != null)
        {
            foreach (Player player in joinedLobby.Players)
            {
                if (player.Id == AuthenticationService.Instance.PlayerId)
                {
                    // This player is in this lobby
                    return true;
                }
            }
        }
        return false;
    }

    private async void HandleLobbyHeartbeat()
    {
        if (IsLobbyHost())
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15f;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }

    private async void HandleLobbyPolling()
    {
        if (joinedLobby != null)
        {
            lobbyPollTimer -= Time.deltaTime;
            if (lobbyPollTimer < 0f)
            {
                float lobbyPollTimerMax = 1.1f;
                lobbyPollTimer = lobbyPollTimerMax;

                joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);

                RefreshLobbyDisplay();

                OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

                if (!IsPlayerInLobby())
                {
                    // Player was kicked out of this lobby
                    Debug.Log("Kicked from Lobby!");

                    OnKickedFromLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

                    joinedLobby = null;
                }

                if (joinedLobby.Data[KEY_START_GAME].Value != "0")
                {
                    if(!IsLobbyHost())
                    {
                        RelayMaker.Instance.JoinRelay(joinedLobby.Data[KEY_START_GAME].Value);
                    }

                    joinedLobby = null;

                    OnGameStarted?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
