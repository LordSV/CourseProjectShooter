using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Gun
{
    
    [SerializeField] private int _damage;
    [SerializeField] private Transform _bulletPoint;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _shootDelay;
    private float _lastShootTime;
    private int _currentGun = 0;

    public bool TryShoot(out ShootInfo info)
    {
        info = new ShootInfo();

        if (Time.time - _lastShootTime < _shootDelay) return false;

        Vector3 position = _bulletPoint.position;
        Vector3 velocity = _bulletPoint.forward * _bulletSpeed;

        _lastShootTime = Time.time;
        Instantiate(_bulletPrefab, _bulletPoint.position, _bulletPoint.rotation).Init(velocity, _damage);
        shoot?.Invoke();

        info.pX = position.x;
        info.pY = position.y;
        info.pZ = position.z;
        info.dX = velocity.x;
        info.dY = velocity.y;
        info.dZ = velocity.z;

        return true;
    }

    public override void ChangeWeapon()
    {
        _currentGun++;
        if(_currentGun > 1)
        {
            _currentGun = 0;
        }
        switch (_currentGun)
        {
            case 0:
                _damage = 1;
                _bulletSpeed = 10;
                _shootDelay = 0.2f;
                break;
            case 1:
                _damage = 5;
                _bulletSpeed = 15;
                _shootDelay = 0.6f;
                break;
        }
    }
}
