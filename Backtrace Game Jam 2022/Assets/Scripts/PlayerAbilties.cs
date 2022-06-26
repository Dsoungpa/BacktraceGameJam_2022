using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilties : MonoBehaviour
{
    [SerializeField] private bool offCooldown;
    [SerializeField] private float cooldownTimer;
    [SerializeField] private GameObject emergencyBlock;

    // Start is called before the first frame update
    void Start()
    {
        offCooldown = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && offCooldown){
            UseAbility();
            Cooldown();
        }
    }

    void UseAbility() {
        Vector2 emergencyBlockPosition = new Vector2(transform.position.x, transform.position.y - 3);
        Instantiate(emergencyBlock, emergencyBlockPosition, Quaternion.identity);
    }

    IEnumerator Cooldown() {
        offCooldown = false;
        yield return new WaitForSeconds(cooldownTimer);
        offCooldown = true;
    }
}
