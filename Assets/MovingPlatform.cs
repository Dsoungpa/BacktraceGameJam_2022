using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Vector2 target1;
    private Vector2 target2;
    private bool goingRight = true;
    private bool goingLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        target1 = new Vector2(-3.12f, transform.position.y);
        target2 = new Vector2(2.52f, transform.position.y);

    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;

        // print("Going Right: " + goingRight);
        // print("Going Left: " + goingLeft);

        if(goingRight){
            //print("In here");
            transform.position = Vector2.MoveTowards(transform.position, target2, step);
        }

        if(goingLeft){
            transform.position = Vector2.MoveTowards(transform.position, target1, step);
        }

        // print("POSITION: " + transform.position.x);
        // print("TARGET1: " + (target1.x + 0.02f));

        // Check to see if too far to the left
        if(transform.position.x <= target1.x + 0.25f){
            print("GOT IN HERE");
            transform.position = Vector2.MoveTowards(transform.position, target2, step);
            goingLeft = false;
            goingRight = true;
        }

        // Check to see if too far to the right
        if(transform.position.x >= target2.x - 0.25f){
            print("GOT IN HERE2");
            transform.position = Vector2.MoveTowards(transform.position, target1, step);
            goingLeft = true;
            goingRight = false;

        }
    }
}
