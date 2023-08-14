using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    public Vector3 TargetPosition { get; private set; } = Vector3.zero;
    public Vector3 BodyRotation;
    public Vector3 HeadRotation;
    [SerializeField] private Health _health;
    [SerializeField] private Transform _head;
    private float _velocityMagnitude = 0;
    private string _sessionID;

    public void Init(string sessionID)
    {
        _sessionID = sessionID; 
    }

    private void Start()
    {
        TargetPosition = transform.position;
    }
    private void Update()
    {
        if(_velocityMagnitude > 0.1f)
        {
            float maxDistance = _velocityMagnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, maxDistance);
        }
        else
        {
            transform.position = TargetPosition;
        }


    }
    public void SetSpeed(float value) => speed = value;
    public void SetMaxHP(int value)
    {
        maxHealth = value;
        _health.SetMax(value);
        _health.SetCurrent(value);
    }

    public void RestoreHP(int newvalue)
    {
        _health.SetCurrent(newvalue);
    }

    public void ApplyDamage(int damage)
    {
        _health.ApplyDamage(damage);

        Dictionary<string, object> data = new Dictionary<string, object>() 
        {
            {"id", _sessionID},
            {"value", damage}
        };

        MultiplayerManager.Instance.SendMessage("damage", data);
    }
    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval)
    {
        TargetPosition = position + (velocity * averageInterval);
        _velocityMagnitude = velocity.magnitude;

        this.Velocity = velocity;
    }
    public void SetBodyRotation(in Vector3 rotation, in Vector3 angularVelocity, in float averageInterval)
    {
        Vector3 velocity = angularVelocity;
        velocity.x = 0;
        velocity.z = 0;
        BodyRotation = rotation + (velocity* averageInterval);
        transform.localEulerAngles = BodyRotation;
    }

    public void SetHeadRotation(in Vector3 rotation, in Vector3 angularVelocity, in float averageInterval)
    {
        Vector3 velocity = angularVelocity;
        velocity.y = 0;
        velocity.z = 0;
        HeadRotation = rotation + (velocity * averageInterval);
        _head.localEulerAngles = HeadRotation;
    }

    //public void SetRotateX(float value, in float averageInterval)
    //{
    //    _head.localEulerAngles = new Vector3(value, 0, 0);
    //}
    //public void SetRotateY(float value, in float averageInterval)
    //{
    //    transform.localEulerAngles = new Vector3(0, value, 0);
    //}

    public void SetBodyPosition(Vector3 bodyPosition)
    {
        UpperBody.localPosition = bodyPosition;
    }
}
