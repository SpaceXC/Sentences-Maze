using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : Singleton<HealthUI>
{
    public Image barSlider;
    public Text text;

    private void Update()
    {
        print(FindObjectOfType<GameManager>());
    }

    public void UpdateHealthBarUI()
    {
        barSlider.fillAmount = FindObjectOfType<GameManager>().currentHealth / FindObjectOfType<GameManager>().health;
        text.text = "HP: " + FindObjectOfType<GameManager>().currentHealth;
    }
}
