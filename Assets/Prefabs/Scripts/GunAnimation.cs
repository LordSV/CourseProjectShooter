using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    [SerializeField] private Gun _gun;
    [SerializeField] private Animator _animator;
    private const string shoot = "Shoot";

    private void Start()
    {
        _gun.shoot += Shoot;
    }

    private void Shoot()
    {
        _animator.SetTrigger(shoot);
    }

    private void OnDestroy()
    {
        _gun.shoot -= Shoot;
    }
}
[System.Serializable]
public struct ShootInfo
{
    public string key;
    public float pX;
    public float pY;
    public float pZ;
    public float dX;
    public float dY;
    public float dZ;
}
