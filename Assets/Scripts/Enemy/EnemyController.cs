using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyCharacter _character;
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

    public void Init(Player player)
    {
        _player = player;
        _character.SetSpeed(player.speed);
        _player.OnChange += OnChange;
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
        Vector3 velocity = _character.velocity;

        foreach (var dataChange in changes) 
        {
            switch (dataChange.Field)
            {
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
                    _character.SetRotateX((float)dataChange.Value);
                    break;
                case "rY":
                    _character.SetRotateY((float)dataChange.Value);
                    break;
                default:
                    Debug.LogWarning("Не обрабатывается изменение поля " + dataChange.Field);
                    break;
            }
        }
        _character.SetMovement(position, velocity, AverageInterval);
    }
}
