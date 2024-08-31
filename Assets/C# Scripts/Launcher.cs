using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Random = UnityEngine.Random;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField nicknameField;
    private readonly string _gameVersion = "1";

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; //방에 있는 사람들이 방장의 Scene상태와 자동으로 동기화됩니다.
        PhotonNetwork.GameVersion = _gameVersion;  // 같은 게임 버전끼리 접속할 수 있도록 합니다.
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Connect()
    {
        if (nicknameField.text.Equals(string.Empty))
        {
            PhotonNetwork.NickName = "User " + Random.Range(0, 1000);
        }
        PhotonNetwork.NickName = nicknameField.text;
        PhotonNetwork.ConnectUsingSettings();
    }

    // 포톤 서버에 접속 후 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    
    // 로비에 접속 후 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}
