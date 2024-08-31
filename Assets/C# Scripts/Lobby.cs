using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject roomPanel;
    public Button[] roomBtns;
    public TMP_InputField roomName;
    private List<RoomInfo> _myList = new List<RoomInfo>();
    private void Awake()
    {
        
    }
    void Start()
    {
        
    }

    void Update()
    {
       //Debug.Log(PhotonNetwork.NetworkClientState.ToString());
    }
    
    /*
     * 로비에 노출 허용
     * 방에 들어오는 것을 허용
     * 최대 6명
     * 플레이어가 0명이면 즉시 방 삭제
     */
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(roomName.text == "" ? "Room " + Random.Range(0, 100) : roomName.text,
            new RoomOptions { MaxPlayers = 4 });
    }
    
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        roomName.text = ""; CreateRoom();
        Debug.Log(returnCode + ", " + message);
    }
    
    public void RoomPanel()
    {
        roomPanel.SetActive(!roomPanel.activeSelf);
        MyListRenewal();
    }
    
    public void MyListClick(int num)
    {
        PhotonNetwork.JoinRoom(_myList[num].Name);
        MyListRenewal();
    }
    
    void MyListRenewal()
    {
        for (int i = 0; i < roomBtns.Length; i++)
        {
            roomBtns[i].interactable = (i < _myList.Count) ? true : false;
            roomBtns[i].transform.GetChild(0).GetComponent<TMP_Text>().text = (i < _myList.Count) ? _myList[i].Name : "";
            roomBtns[i].transform.GetChild(1).GetComponent<TMP_Text>().text = (i < _myList.Count) ? _myList[i].PlayerCount + "/" + _myList[i].MaxPlayers : "";
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!_myList.Contains(roomList[i])) _myList.Add(roomList[i]);
                else _myList[_myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (_myList.IndexOf(roomList[i]) != -1) _myList.RemoveAt(_myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }

    public override void OnJoinedLobby()
    {
        _myList.Clear();
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Room");
        }
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
}
