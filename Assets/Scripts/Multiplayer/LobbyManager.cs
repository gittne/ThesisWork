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
using UnityEditor.ShaderGraph;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance { get; private set; }
    private void Awake() { Instance = this; }


    [SerializeField] TextMeshProUGUI usernameText;

    [SerializeField] TextMeshProUGUI lobbyNameText;
    [SerializeField] TextMeshProUGUI lobbyPassText;

    [SerializeField] string playerName;

    [SerializeField] string lobbyName;
    [SerializeField] string lobbyPass;

    public const string KEY_PLAYER_NAME = "PlayerName";
    public const string KEY_GAME_MODE = "GameMode";
    public const string KEY_START_GAME = "Start";

    public event EventHandler OnLeftLobby;

    public event EventHandler<LobbyEventArgs> OnJoinedLobby;
    public event EventHandler<LobbyEventArgs> OnJoinedLobbyUpdate;
    public event EventHandler<LobbyEventArgs> OnKickedFromLobby;
    public event EventHandler<LobbyEventArgs> OnLobbyGameModeChanged;

    public event EventHandler<EventArgs> OnGameStarted;

    public class LobbyEventArgs : EventArgs
    {
        public Lobby lobby;
    }

    private Lobby joinedLobby;

    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;
    public class OnLobbyListChangedEventArgs : EventArgs
    {
        public List<Lobby> lobbyList;
    }

    public enum GameMode
    {
        Difficulty_Normal,
        Difficulty_Hard
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
            // do nothing
            Debug.Log("Signed in with ID: " + AuthenticationService.Instance.PlayerId + "\n and username: " + playerName);

            //RefreshLobbyList();
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public void UpdateUsername() { playerName = usernameText.text; }

    public void SetLobbyStats()
    {
        lobbyName = lobbyNameText.text;
        lobbyPass = lobbyPassText.text;
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
        bool isPrivate = (lobbyPass == null) ? true : false;
        GameMode gm = GameMode.Difficulty_Normal;

        CreateLobbyOptions options = new CreateLobbyOptions
        {
            Player = player,
            IsPrivate = isPrivate,
            Data = new Dictionary<string, DataObject> {
                { KEY_GAME_MODE, new DataObject(DataObject.VisibilityOptions.Public, gm.ToString()) },
                { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0") }
            }
        };

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, 1, options);

        joinedLobby = lobby;

        OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });

        Debug.Log("Lobby successfully created.");
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

    public Lobby GetJoinedLobby() { return joinedLobby; }

    public bool IsLobbyHost() { return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId; }
}
