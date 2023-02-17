using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarScript : MonoBehaviour
{
    public Slider slider;
    public float currentHealth;

    void Update(){
        slider.value = currentHealth;
    }
    
    public void SetMaxHealth(float health){
        slider.maxValue = health;
        slider.value = health;
        currentHealth = health;
    }
    public void SetHealth(float health){
        slider.value = health;
        currentHealth = health;
    }
}
