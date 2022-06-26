using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollectionScript : MonoBehaviour
{
    public HealthBarScript healthBar;

    void OnTriggerEnter2D(Collider2D collision){
        print("In here");
        if(collision.gameObject.tag == "Player"){
            int currHealth = healthBar.currentHealth;
            currHealth += 10;
            healthBar.SetHealth(currHealth);
            Destroy(this.gameObject);
        }
    }
}
