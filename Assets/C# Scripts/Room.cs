using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Room : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text roomText;
    public TMP_Text[] text;
    public TMP_InputField sendField;
    public GameObject field;
    public PhotonView pv;
    public TMP_Text[] nickname;

    private void Awake()
    {
        UserNickname();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Send();
            EventSystem.current.SetSelectedGameObject(field);
        }
    }
    
    public void Send()
    {
        if (sendField.text == "")
        {
            return;
        }
        pv.RPC("ChatRPC", RpcTarget.All, "<color=green>" + PhotonNetwork.NickName + "</color>" + " : " + sendField.text);
        sendField.text = "";
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < text.Length; i++)
            if (text[i].text == "")
            {
                isInput = true;
                text[i].text = msg;
                break;
            }
        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < text.Length; i++) text[i - 1].text = text[i].text;
            text[text.Length - 1].text = msg;
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
        base.OnLeftRoom();
    }

    public override void OnJoinedRoom()
    {
        UserNickname();
    }
    public override void OnPlayerEnteredRoom(Player player)
    {
        UserNickname();
    }
    public override void OnPlayerLeftRoom(Player player)
    {
        UserNickname();
    }
    
    public void UserNickname()
    {
        for (int i = 0; i < nickname.Length; i++)
        {
            nickname[i].text = "";
        }
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.IsMasterClient)
            {
                for (int i = 0; i < nickname.Length; i++)
                {
                    if (nickname[i].text == "")
                    {
                        nickname[i].text = "[Master]" + player.Value.NickName;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < nickname.Length; i++)
                {
                    if (nickname[i].text == "")
                    {
                        nickname[i].text = player.Value.NickName;
                        break;
                    }
                }
            }
        }
    }

    public void Gamestart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Game");
        }
        else
        {
            ChatRPC("<color=red>방장만 게임을 시작할 수 있습니다.</color>");
        }
    }
}
