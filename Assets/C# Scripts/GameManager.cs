using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviourPunCallbacks
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    
    [SerializeField] private Transform[] spawnPoints;
    public GameObject character;
    public int numberOfUser;
    public GameObject escape;
    public GameObject mainCamera;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
    }

    void Start()
    {
        spawnUnit();
        mainCamera.AddComponent<CinemachineBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Escape();
        }
    }

    public void spawnUnit()
    {
        int index = Random.Range(0, spawnPoints.Length);
        PhotonNetwork.Instantiate(character.name, spawnPoints[index].transform.position, Quaternion.identity, 0);
    }
    
    public void CursorOn()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    private void CursorOff()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void Escape()
    {
        if (escape.activeSelf)
        {
            escape.SetActive(false);
            CursorOff();
        }
        else
        {
            escape.SetActive(true);
            CursorOn();
        }
    }
}
