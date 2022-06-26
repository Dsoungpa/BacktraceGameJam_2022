using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    public Transform target, background1, background2;
    [SerializeField] private float size;
    // Start is called before the first frame update
    void Start()
    {
        size = 24f;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(transform.position.x, target.position.y + 5f, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.2f);
        //print("Transform Position: " + (transform.position.y));
        // print("Background1 Position: " + background1.position.y);
        // print("Background2 Position: " + background2.position.y);
        
        if(transform.position.y > 24f){

            // Switch backgrounds
            // if you go higher than background 1 put it on top
            if(transform.position.y >= background2.position.y){
                print(size);
                print(background2.position.y + size);
                background1.position = new Vector3(background1.position.x, background2.position.y + size, background1.position.z);
                SwitchBackground();
            }
            // // if you go lower than background 1 bring background 2 down
            if(transform.position.y < background1.position.y){
                background2.position = new Vector3(background2.position.x, background1.position.y - size, background2.position.z);
                SwitchBackground();
            }
        }
    }
    private void SwitchBackground(){
            Transform temp = background1;
            background1 = background2;
            background2 = temp;
    }
}