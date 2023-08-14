using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    public void UpdateHealth(float max, int current)
    {
        _fillImage.fillAmount = current / max;
    }
}
