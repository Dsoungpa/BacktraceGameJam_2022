using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warden : MonoBehaviour
{
    [SerializeField] private float riseSpeed = 5f;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(0, riseSpeed);
    }

    void OnTriggerEnter2D(Collider2D otherObject) {
        if (otherObject.gameObject.tag == "Platform"){
            Destroy(otherObject.gameObject);
        }
    }
}