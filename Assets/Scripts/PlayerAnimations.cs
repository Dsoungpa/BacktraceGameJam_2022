using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{

    [SerializeField] public Animator animator;

    public GameObject Player;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Flips the Sprite
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput > 0){
            Player.transform.localScale = new Vector3(1,1,1);
        }
        if (horizontalInput < 0){
            Player.transform.localScale = new Vector3(-1,1,1);
        }

        //run animation
        animator.SetFloat("speed", Mathf.Abs(horizontalInput));


    }
}
