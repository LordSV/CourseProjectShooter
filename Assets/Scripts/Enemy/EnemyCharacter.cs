using UnityEngine;

public class EnemyCharacter : Character
{
    public Vector3 TargetPosition { get; private set; } = Vector3.zero;
    [SerializeField] private Health _health;
    [SerializeField] private Transform _head;
    private float _velocityMagnitude = 0;

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
    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval)
    {
        TargetPosition = position + (velocity * averageInterval);
        _velocityMagnitude = velocity.magnitude;

        this.velocity = velocity;
    }

    public void ApplyDamage(int damage)
    {
        _health.ApplyDamage(damage);
    }

    public void SetRotateX(float value)
    {
        _head.localEulerAngles = new Vector3(value, 0, 0);
    }
    public void SetRotateY(float value)
    {
        transform.localEulerAngles = new Vector3(0, value, 0);
    }
}
