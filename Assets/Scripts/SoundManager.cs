using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource jump;
    public AudioSource landing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void JumpSound(){
        jump.Play();
    }

    void LandingSound(){
        landing.Play();
    }
}
