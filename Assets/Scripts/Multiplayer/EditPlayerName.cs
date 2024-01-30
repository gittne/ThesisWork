using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EditPlayerName : MonoBehaviour {


    public static EditPlayerName Instance { get; private set; }

    [SerializeField] private GameObject playerNameText;


    private string playerName = "Player";

    private void Awake()
    {
        Instance = this;
    }

    public string GetPlayerName() {
        return playerName;
    }

    public void UpdatePlayerName()
    {
        playerName = playerNameText.GetComponent<TMP_InputField>().text.ToUpper();
        LobbyManager.Instance.UpdatePlayerName(playerName);
    }
}