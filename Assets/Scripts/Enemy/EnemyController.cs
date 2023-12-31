using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyCharacter _character;
    [SerializeField] private EnemyGun _gun;
    private List<float> _receivedTimeInterval = new List<float> { 0, 0, 0, 0, 0 };
    private float _lastReceivedTime = 0f;
    private Player _player;

    private float AverageInterval
    {
        get
        {
            int receivedTimeIntervalCount = _receivedTimeInterval.Count;
            float summ = 0;
            for(int i = 0; i < receivedTimeIntervalCount; i++)
            {
                summ += _receivedTimeInterval[i];
            }
            return summ / receivedTimeIntervalCount;
        }
    }

    public void Init(string key, Player player)
    {
        _character.Init(key);

        _player = player;
        _character.SetSpeed(player.speed);
        _character.SetMaxHP(player.maxHP);
        _player.OnChange += OnChange;
    }

    public void Shoot(in ShootInfo info)
    {
        Vector3 position = new Vector3(info.pX, info.pY, info.pZ);
        Vector3 velocity = new Vector3(info.dX, info.dY, info.dZ);
        _gun.Shoot(position, velocity);
    }
    public void Destroy()
    {
        _player.OnChange -= OnChange;
        Destroy(gameObject);
    }
    private void SaveReceivedTime()
    {
        float interval = Time.time - _lastReceivedTime;
        _lastReceivedTime = Time.time;

        _receivedTimeInterval.Add(interval);
        _receivedTimeInterval.RemoveAt(0);
    }
    internal void OnChange(List<DataChange> changes)
    {
        SaveReceivedTime();

        Vector3 position = _character.TargetPosition;
        Vector3 velocity = _character.Velocity;
        Vector3 bodyPosition = _character.UpperBody.localPosition;
        Vector3 angularVelocity = _character.AngularVelocity;
        Vector3 headRotation = _character.HeadRotation;
        Vector3 bodyRotation = _character.BodyRotation;

        foreach (var dataChange in changes) 
        {
            switch (dataChange.Field)
            {
                case "loss":
                    MultiplayerManager.Instance.LossCounter.SetEnemyLoss((byte)dataChange.Value);
                    break;
                case "currentHP":
                    if((sbyte)dataChange.Value > (sbyte)dataChange.PreviousValue)
                        _character.RestoreHP((sbyte)dataChange.Value);
                    break;
                case "pX":
                    position.x = (float)dataChange.Value;
                    break;
                case "pY":
                    position.y = (float)dataChange.Value;
                    break;
                case "pZ":
                    position.z = (float)dataChange.Value;
                    break;
                case "vX":
                    velocity.x = (float)dataChange.Value;
                    break;
                case "vY":
                    velocity.y = (float)dataChange.Value;
                    break;
                case "vZ":
                    velocity.z = (float)dataChange.Value;
                    break;
                case "rX":
                    headRotation.x = (float)dataChange.Value;
                    break;
                case "rY":
                    bodyRotation.y = (float)dataChange.Value;
                    break;
                case "bpX":
                    bodyPosition.x = (float)dataChange.Value;
                    break;
                case "bpY":
                    bodyPosition.y = (float)dataChange.Value;
                    break;
                case "bpZ":
                    bodyPosition.z = (float)dataChange.Value;
                    break;
                default:
                    Debug.LogWarning("�� �������������� ��������� ���� " + dataChange.Field);
                    break;
            }
        }
        _character.SetBodyPosition(bodyPosition);
        _character.SetMovement(position, velocity, AverageInterval);
        _character.SetBodyRotation(bodyRotation, angularVelocity, AverageInterval);
        _character.SetHeadRotation(headRotation, angularVelocity, AverageInterval);
    }
}
