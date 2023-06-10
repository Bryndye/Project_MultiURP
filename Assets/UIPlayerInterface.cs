using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInterface : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private Image health_bar;
    [SerializeField] private TextMeshProUGUI health_text;


    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateHealth;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHealth;
    }

    private void UpdateHealth(ushort _health)
    {
        health_bar.fillAmount = (float)(_health / (float)100);
        health_text.text = _health.ToString();
    }
}
