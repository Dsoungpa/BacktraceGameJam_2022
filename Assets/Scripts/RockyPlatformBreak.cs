using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockyPlatformBreak : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private CameraShake cameraShakeScript;

    [SerializeField] private float waitBeforeRumble = 0.5f;
    [SerializeField] private float waitBeforeFall = 2f;
    [SerializeField] private float shakeDuration = 2f;
    [SerializeField] private float shakeMagnitude = .5f;
    [SerializeField] private float magnitudeFade = 0.1f;

    private bool temp = true;

    //audio
    public AudioSource rockfall;
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        animator = GetComponent<Animator>();
        cameraShakeScript = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            StartCoroutine(BreakBlock());
        }
    }

    public IEnumerator BreakBlock() {
        yield return new WaitForSeconds(waitBeforeRumble);
        animator.SetBool("rumbleStart", true);

        StartCoroutine(cameraShakeScript.CameraShakeController(shakeDuration, shakeMagnitude, magnitudeFade));

        if(temp){
            rockfall.Play();
            temp = false;
        }
    
        yield return new WaitForSeconds(waitBeforeFall);

        blockFall();
    }

    public void blockFall() {
        rb.isKinematic = false;
        rb.constraints = ~(RigidbodyConstraints2D.FreezePositionY);
    }
}
