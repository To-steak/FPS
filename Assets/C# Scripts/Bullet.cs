using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviourPunCallbacks
{
    private Transform _muzzle;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Destroy(gameObject, 3f);
    }

    void Start()
    {

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Envi"))
        {
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, 20f);
            Destroy(gameObject);
        }
    }
}
