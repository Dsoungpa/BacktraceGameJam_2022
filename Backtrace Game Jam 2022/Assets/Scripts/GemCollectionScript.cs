using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GemCollectionScript : MonoBehaviour
{
    [SerializeField] private HealthBarScript healthBar;
    [SerializeField] private float healAmount = 10f;

    

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Player"){
            float currHealth = healthBar.currentHealth;
            currHealth += healAmount;
            healthBar.SetHealth(currHealth);
            Destroy(gameObject);
            
        }
    }
}