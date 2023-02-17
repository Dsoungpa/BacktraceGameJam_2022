using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropThroughPlatform : MonoBehaviour
{
    private BoxCollider2D playerCollider;

    [Header("Drop Down")]
    [SerializeField] private float waitBeforeDrop = 0.25f;
    [SerializeField] private float waitAfterDrop = 0.5f;
    [SerializeField] private bool onPlatform = false;

    void Awake() {
        playerCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            if (onPlatform) {
                StartCoroutine(DropDown());
            }
        }
    }

    private void OnCollisionStay2D(Collision2D otherObject) {
        if (otherObject.gameObject.CompareTag("Platform")) {
            onPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D otherObject) {
        if (otherObject.gameObject.CompareTag("Platform")) {
            onPlatform = false;
        }
    }

    private IEnumerator DropDown(){
        yield return new WaitForSeconds(waitBeforeDrop);
        playerCollider.enabled = false;
        yield return new WaitForSeconds(waitAfterDrop);
        playerCollider.enabled = true;
        // StopCoroutine(DropDown()); //don't need to stop coroutine if code can end normally (without yield)
    }
}
