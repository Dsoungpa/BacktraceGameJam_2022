using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerAbilties : MonoBehaviour
{
    [SerializeField] private GameObject emergencyBlock;
    [SerializeField] private bool offCooldown;
    public float cooldownTimer = 5f;
    [SerializeField] private HealthBarScript healthBar;
    [SerializeField] private float damageAmount = 20f;

    [Header("Emergency Block Cooldown")]
    [SerializeField] private Image emergencyBlockIcon;

    // Start is called before the first frame update
    void Start()
    {
        offCooldown = true;

        emergencyBlockIcon.fillAmount = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && offCooldown){
            UseAbility();
            float currHealth = healthBar.currentHealth;
            currHealth -= damageAmount;
            healthBar.SetHealth(currHealth);
        }

        if (offCooldown == false) {
            emergencyBlockIcon.fillAmount -= 1 / cooldownTimer * Time.deltaTime;
            if (emergencyBlockIcon.fillAmount <= 0) {
                emergencyBlockIcon.fillAmount = 0;
                offCooldown = true;
            }
        }
    }
    void UseAbility() {
        Vector2 emergencyBlockPosition = new Vector2(transform.position.x, transform.position.y - 3);
        Instantiate(emergencyBlock, emergencyBlockPosition, Quaternion.identity);

        offCooldown = false;
        emergencyBlockIcon.fillAmount = 1;
    }

}
