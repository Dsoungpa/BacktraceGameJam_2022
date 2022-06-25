using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGeneration : MonoBehaviour
{
    [SerializeField] private GameObject regularPlatform;

    private int[] spawnPositions = new int[] {-1, 0, 1}; // -1 = left, 0 = center, 1 = right
    private int[] weightedColumns = new int[] {1, 2, 2, 2, 3, 3};
    private Vector2 previousPlatformPosition;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 firstPlatformPosition = CalculatePosition(new Vector2(0, 3f));
        Instantiate(regularPlatform, firstPlatformPosition, Quaternion.identity);
        previousPlatformPosition = firstPlatformPosition;

        SpawnInitialPlatforms();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnPlatforms() {
        
    }
//////MAKE A COLUMN CHECKER: something that decides what column the next platform should be at.
//////add variations after.
    private void SpawnInitialPlatforms() {
        int initialPlatformLimit = 6;

        for (int i = 0; i < initialPlatformLimit; i++) {
            Vector2 newPlatform = CalculatePosition(new Vector2(0, previousPlatformPosition.y));
        }
    }

    private Vector2 CalculatePosition(Vector2 platformPosition) {
        int column = spawnPositions[Random.Range(0, spawnPositions.Length - 1)];
        print(column);

        switch(column) {
            case -1:
                platformPosition += new Vector2(-2.5f, 0);
                break;
            case 1:
                platformPosition += new Vector2(2.5f, 0);
                break;
        }
        print(platformPosition);

        float randomShiftX = Random.Range(-0.5f, -0.5f);
        float randomShiftY = Random.Range(-0.25f, 0.25f);

        platformPosition += new Vector2(randomShiftX, 0f);
        platformPosition += new Vector2(0f, randomShiftY);

        print(platformPosition);

        return platformPosition;
    }

    // private int ColumnChecker(Vector2 platformPosition) {
    //     if (platformPosition >= -2.75f && platformPosition <)
    // }
}
