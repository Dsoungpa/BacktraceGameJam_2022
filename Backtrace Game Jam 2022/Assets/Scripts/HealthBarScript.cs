using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;
    public int currentHealth;

    public void SetMaxHealth(int health){
        slider.maxValue = health;
        slider.value = health;
        currentHealth = health;
    }
    
    public void SetHealth(int health){
        slider.value = health;
        currentHealth = health;
    }
}
