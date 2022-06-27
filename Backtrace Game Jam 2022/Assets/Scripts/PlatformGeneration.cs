using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlatformGeneration : MonoBehaviour
{
    [SerializeField] private GameObject regPlat;
    [SerializeField] private GameObject icyPlatform;
    [SerializeField] private GameObject rockyPlatform;
    [SerializeField] private GameObject warden;
    [SerializeField] private GameObject fire;
    [SerializeField] private Transform newPlatformSpawnTrigger;
    [SerializeField] private float newPlatformSpawnTriggerShift = 8.2f;
    [SerializeField] private float wardenSpawnDelay = 50f;
    [SerializeField] private float icyPlatformIntro = 200f;
    [SerializeField] private float rockyPlatformIntro = 500f;
    [SerializeField] private PlayerScore playerScoreScript;
    private bool allPlatformTypesIntroduced = true;
    private int[] spawnPositions = new int[] {-1, 0, 1}; // -1 = left, 0 = center, 1 = right
    private int[] weightedCenter = new int[] {-1, -1, -1, 0, 1, 1, 1}; //previous column was center
    private int[] weightedLeft = new int[] {-1, 0, 0, 0, 0, 1, 1}; //previous column was left
    private int[] weightedRight = new int[] {-1, -1, 0, 0, 0, 0, 1}; //previous column was right
    [SerializeField] private float fireChance = .25f;
    private Vector2 previousPlatformPosition = new Vector2(0, 0);
    private Vector2 newPlatform;
    private GameObject wardenInstance;
    private GameObject[] platformStorage;
    // Start is called before the first frame update
    void Awake()
    {
        //playerScoreScript
        wardenInstance = GameObject.FindWithTag("Warden");
        platformStorage = new GameObject[] {regPlat, regPlat, regPlat, regPlat, regPlat, regPlat, regPlat, regPlat};
        SpawnPlatform();
        SpawnPlatformSets(5);
    }
    // Update is called once per frame
    void Update()
    {
        if (allPlatformTypesIntroduced) {
            AddNewPlatformTypes();
        }
        if (previousPlatformPosition.y - newPlatformSpawnTrigger.position.y < newPlatformSpawnTriggerShift) {
            SpawnPlatformSets(10);
        }
        if (wardenInstance == null){
            SpawnWarden();
            wardenInstance = GameObject.FindWithTag("Warden");
        }
    }
    private void AddNewPlatformTypes() {
        if (playerScoreScript.CurrentScore > icyPlatformIntro) {
            platformStorage[2] = icyPlatform;
            platformStorage[3] = icyPlatform;
        }
        if (playerScoreScript.CurrentScore > rockyPlatformIntro) {
            platformStorage[4] = rockyPlatform;
            platformStorage[5] = rockyPlatform;
            allPlatformTypesIntroduced = false;
        }
    }
    private void SpawnWarden() {
        if (newPlatformSpawnTrigger.position.y > wardenSpawnDelay) {
            Instantiate(warden, Vector2.zero, Quaternion.identity);
        }
    }
    private void SpawnPlatform() {
        Vector2 newPlatform = CalculatePosition(previousPlatformPosition);
        Vector2 fireOffset = new Vector2(0f, .5f);
        if(Random.value <= fireChance){
            Instantiate(fire, newPlatform + fireOffset, Quaternion.identity);
        }
        Instantiate(ChoosePlatformTypes(), newPlatform, Quaternion.identity);
        previousPlatformPosition = newPlatform;
    }
    private void SpawnPlatformSets(int platformLimit) {
        for (int i = 0; i < platformLimit; i++) {
            SpawnPlatform();
        }
    }
    private GameObject ChoosePlatformTypes() {
        GameObject chosenPlatformType = platformStorage[Random.Range(0, platformStorage.Length - 1)];
        return chosenPlatformType;
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