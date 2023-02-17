using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerAbilties : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthBarScript healthBarScript;
    [SerializeField] private GameObject emergencyBlock;
    [SerializeField] private AudioSource spawnPlatformAudio;
    
    [Header("Abilities")]
    // ability 1 = Emergency Block
    [SerializeField] private Image emergencyBlockIcon;
    [SerializeField] private float ability1Cost = 25f;
    [SerializeField] private float spawnOffset = 1.5f;
    
    private bool offCooldown1;
    public float cooldown1 = 5f;

    void Start()
    {
        offCooldown1 = true;
        emergencyBlockIcon.fillAmount = 0;
    }
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && offCooldown1){
            UseAbilityEmergency();
            healthBarScript.currentHealth -= ability1Cost;
        }

        if (offCooldown1 == false) {
            emergencyBlockIcon.fillAmount -= 1 / cooldown1 * Time.deltaTime;
            if (emergencyBlockIcon.fillAmount <= 0) {
                emergencyBlockIcon.fillAmount = 0;
                offCooldown1 = true;
            }
        }
    }

    void UseAbilityEmergency() {
        Vector2 emergencyBlockPosition = new Vector2(transform.position.x, transform.position.y - spawnOffset);
        Instantiate(emergencyBlock, emergencyBlockPosition, Quaternion.identity);
        offCooldown1 = false;
        emergencyBlockIcon.fillAmount = 1;

        spawnPlatformAudio.Play();
    }
}
