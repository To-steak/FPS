using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIK : MonoBehaviour
{
    [SerializeField] private Transform leftPivot;
    [SerializeField] private Transform rightPivot;
    private Animator _animator;
    private int _layerIndex;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _layerIndex = _animator.GetLayerIndex("Top");
    }

    void Update()
    {
        
    }

    private void OnAnimatorIK(int index)
    {
        if (index != _layerIndex)
        {
            return;
        }
        _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        //_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        
        _animator.SetIKPosition(AvatarIKGoal.LeftHand, leftPivot.position);
        //_animator.SetIKRotation(AvatarIKGoal.LeftHand, leftPivot.rotation);
        
        _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        //_animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        
        _animator.SetIKPosition(AvatarIKGoal.RightHand, rightPivot.position);
        //_animator.SetIKRotation(AvatarIKGoal.RightHand, rightPivot.rotation);
    }
}
