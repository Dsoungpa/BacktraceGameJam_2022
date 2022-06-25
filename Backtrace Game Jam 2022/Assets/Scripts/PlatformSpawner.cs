using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformpattern1Prefab;
    public GameObject platformpattern2Prefab;
    public GameObject platformpattern3Prefab;
    public GameObject platformpattern4Prefab;

    public GameObject player;
    
    private int platformCount = 8;
    private float platformspawnHeight = -1f;

    // Start is called before the first frame update
    void Start()
    {
        // INITIAL PLATFORM SPAWING
        // -8f and 7f are just starting values for the HEIGHT to spawn the first few platform
        spawnPlatforms(); 
    }

    // Update is called once per frame
    void Update()
    {
        // print(platformspawnHeight);
        if(player.transform.position.y > platformspawnHeight - 30){
            spawnPlatforms();
        }
    }   

    // Function for spawning platforms
    void spawnPlatforms(){
        for(int i = 0; i < platformCount; i++){
            Vector3 spawnPosition = new Vector3(-0.5f, platformspawnHeight, 0f);

            // We roll a dice to choose the pattern that will be instantiated
            int patternChoice = Random.Range(1,4);

            // if 100m - 1000m
            if(platformspawnHeight < 58f){
                switch(patternChoice){
                case 1:
                    // pattern 1
                    Instantiate(platformpattern1Prefab, spawnPosition, Quaternion.identity);
                    platformspawnHeight += 8f;
                    break;
                case 2:
                    // pattern 2
                    Instantiate(platformpattern2Prefab, spawnPosition, Quaternion.identity);
                    platformspawnHeight += 8f;
                    break;
                case 3:
                    // pattern 3
                    Instantiate(platformpattern3Prefab, spawnPosition, Quaternion.identity);
                    platformspawnHeight += 8f;
                    break;
                }
            }
            // milestone 2
            else if(platformspawnHeight < 1000f){
                // print("Spawned Second Pattern Milestone");
                Instantiate(platformpattern4Prefab, spawnPosition, Quaternion.identity);
                platformspawnHeight += 8f;

            }
            

            // if ()
        }
    }
}



