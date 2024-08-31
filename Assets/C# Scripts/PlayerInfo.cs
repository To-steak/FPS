using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;

public class PlayerInfo : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text ammoText;
    private Animator _animator;
    
    public float curHealth;
    public readonly float MaxHealth = 100f;
    public int curAmmo;
    public int remainedAmmo;
    public readonly int MaxAmmo = 30;
    public GameObject canvas;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (!photonView.IsMine)
        {
            canvas.SetActive(false);
        }
        curHealth = MaxHealth;

        curAmmo = 30;
        remainedAmmo = 120;
    }

    void Update()
    {
        if (curHealth <= 0)
        {
            Die();
        }
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        hpText.text = "HP : " + curHealth;
        ammoText.text = curAmmo + " / " + remainedAmmo;
    }

    public void TakeDamage(float damage)
    {
        curHealth -= damage;
    }
    private void Die()
    {
        _animator.SetTrigger("isDied");
    }
}
