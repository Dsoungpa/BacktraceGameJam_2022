using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGeneration : MonoBehaviour

{
    [SerializeField] private GameObject regularPlatform;
    [SerializeField] private GameObject warden;
    [SerializeField] private Transform newPlatformSpawnTrigger;
    [SerializeField] private float newPlatformSpawnTriggerShift = 8.2f;
    [SerializeField] private float wardenSpawnDelay = 50f;
    public float wardenChaseSpeed = 5f;

    private int[] spawnPositions = new int[] {-1, 0, 1}; // -1 = left, 0 = center, 1 = right

    private int[] weightedCenter = new int[] {-1, -1, -1, 0, 1, 1, 1}; //previous column was center
    private int[] weightedLeft = new int[] {-1, 0, 0, 0, 0, 1, 1}; //previous column was left
    private int[] weightedRight = new int[] {-1, -1, 0, 0, 0, 0, 1}; //previous column was right

    private Vector2 previousPlatformPosition = new Vector2(0, 0);
    private Vector2 newPlatform;
    private GameObject wardenInstance;
    // Start is called before the first frame update
    void Awake()
    {
        wardenInstance = GameObject.FindWithTag("Warden");

        SpawnPlatform();
        SpawnPlatformSets(5);
    }

    // Update is called once per frame
    void Update()
    {
        if (previousPlatformPosition.y - newPlatformSpawnTrigger.position.y < newPlatformSpawnTriggerShift) { //LOOK INTO THIS
            SpawnPlatformSets(10);
        }

        if (wardenInstance == null){
            SpawnWarden();
            wardenInstance = GameObject.FindWithTag("Warden");
        }
    }

    private void SpawnWarden() {
        if (newPlatformSpawnTrigger.position.y > wardenSpawnDelay) {
            Instantiate(warden, Vector2.zero, Quaternion.identity);
        }
    }

    private void SpawnPlatform() {
        Vector2 newPlatform = CalculatePosition(previousPlatformPosition);
        Instantiate(regularPlatform, newPlatform, Quaternion.identity);
        previousPlatformPosition = newPlatform;
    }

    private void SpawnPlatformSets(int platformLimit) {
        for (int i = 0; i < platformLimit; i++) {
            SpawnPlatform();
        }
    }

    private Vector2 CalculatePosition(Vector2 previousPlatform) {
        int column;
        if (ColumnChecker(previousPlatform) < 0) { //left (-1)
            column = weightedLeft[Random.Range(0, weightedLeft.Length - 1)];
        }else if (ColumnChecker(previousPlatform) > 0) { //right (1)
            column = weightedRight[Random.Range(0, weightedRight.Length - 1)];
        }else { //center (0)
            column = weightedCenter[Random.Range(0, weightedCenter.Length - 1)];
        }

        newPlatform = new Vector2(0, 0);

        switch(column) {
            case -1:
                newPlatform += new Vector2(-2.5f, 0);
                break;
            case 1:
                newPlatform += new Vector2(2.5f, 0);
                break;
        }

        float randomShiftX = Random.value - 0.5f;
        float randomShiftY = Random.value - 0.25f;

        newPlatform += new Vector2(randomShiftX, 0f);
        newPlatform += new Vector2(0f, randomShiftY);

        newPlatform += new Vector2(0, previousPlatform.y + 3f);

        return newPlatform;
    }

    private int ColumnChecker(Vector2 platformPosition) {
        if (platformPosition.x < -0.5f) {
            return -1;
        }else if (platformPosition.x > 0.5f) {
            return 1;
        }else {
            return 0;
        }
    }
}
