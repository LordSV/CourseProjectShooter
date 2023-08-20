using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] private EnemyCharacter _character;
    [SerializeField] private DamageBodyPart _bodyPart;

    public void ApplyDamage(int damage)
    {
        switch (_bodyPart)
        {
            case DamageBodyPart.Head:
                _character.ApplyDamage(damage * 5);
                break;
            case DamageBodyPart.Body:
                _character.ApplyDamage(damage);
                break;
        }
    }
}
