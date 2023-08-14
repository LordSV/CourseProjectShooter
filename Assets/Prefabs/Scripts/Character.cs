using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [field: SerializeField] public int maxHealth { get; protected set; } = 10;
    [field: SerializeField] public float speed { get; protected set; } = 2f;
    public Vector3 Velocity { get; protected set; }
    public Vector3 AngularVelocity { get; protected set; }
    public Transform UpperBody;
}
