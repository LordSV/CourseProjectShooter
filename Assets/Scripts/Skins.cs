using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skins : MonoBehaviour
{
    public int Length { get { return _materials.Length; } }
    [SerializeField] private Material[] _materials;

    public Material GetMaterial(int index)
    {
        if(_materials.Length <= index) return _materials[0];
        return _materials[index];
    }
}
