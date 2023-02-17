using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target, background1, background2;
    [SerializeField] private float smoothSpeed = 0.25f;
    [SerializeField] private float size;

    void Start()
    {
        size = 24f;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = new Vector3(transform.position.x, target.position.y + 5f, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed);
        //transform.LookAt(target); //DRUNK EFFECT

        if(transform.position.y > size){ //size was just 24f
            // Switch backgrounds
            // if you go higher than background 1 put it on top
            if(transform.position.y >= background2.position.y){
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