using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RocyPlatformBreak : MonoBehaviour
{
    public float waitBeforeRumble = 0.5f;
    public float waitBeforeFall = 2f;
    private Rigidbody2D rb;
    private Animator animator;
    private bool temp = true;

    //audio
    public AudioSource rockfall;
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        animator = GetComponent<Animator>();
    }
    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            StartCoroutine(BreakBlock());
        }
    }
    public IEnumerator BreakBlock() {
        yield return new WaitForSeconds(waitBeforeRumble);
        animator.SetBool("rumbleStart", true);

        if(temp){
            rockfall.Play();
            temp = false;
        }
        

        yield return new WaitForSeconds(waitBeforeFall);

        blockFall();
    }
    public void blockFall() {
        print("BLOCK FALL");
        rb.isKinematic = false;
        rb.constraints = ~(RigidbodyConstraints2D.FreezePositionY);
    }
}
