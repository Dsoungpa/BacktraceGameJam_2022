using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformGeneration : MonoBehaviour
{
    [Header("Platforms")]
    // [SerializeField] private GameObject regPlat;
    // [SerializeField] private GameObject icyPlatform;
    // [SerializeField] private GameObject rockyPlatform;
    // [SerializeField] private GameObject movingPlatform;
    [SerializeField] private GameObject[] prefabs; //index 0 = reg/1 = icy/2 = rocky/3 = moving
    [SerializeField] private Transform newPlatformSpawnTrigger;
    [SerializeField] private float wardenSpawnDelay = 50f;
    [SerializeField] private float icyPlatformStart = 100f;
    [SerializeField] private float rockyPlatformStart = 200f;
    [SerializeField] private float movingPlatformStart = 300f;
    [SerializeField] private int platformSetSize = 10;
    private bool allPlatformTypesIntroduced = false;
    public bool[] specialPlatformsSpawned;
    private float newPlatformSpawnTriggerShift = 8.2f;
    private Vector2 previousPlatformPosition = new Vector2(0, 0);
    private Vector2 newPlatform;

    [Header("Other Entities")]
    [SerializeField] private GameObject warden;
    [SerializeField] private GameObject flame;
    [SerializeField] private float flameChance = .25f;
    [SerializeField] private int flameBackupThreshold = 30;
    [SerializeField] private int flameBackupMultiplayer = 5;
    private int flameBackup = 0;
    private GameObject wardenInstance;

    [Header("Script References")]
    [SerializeField] private PlayerScore playerScoreScript;
    [SerializeField] private CameraShake cameraShakeScript;

    [Header("Column Choice Ratio")] // Need to manually calculate and enter ratio
    [SerializeField] private int[] weightedCenter = new int[3];
    [SerializeField] private int[] weightedLeft = new int[3];
    [SerializeField] private int[] weightedRight = new int[3];

    [Header("Platform Choice Ration")]
    [SerializeField] private int[] platformChoiceRatio;
    private GameObject[] platformTypes;

    // Start is called before the first frame update
    void Awake()
    {
        platformTypes = DefaultTypesStorage();
        specialPlatformsSpawned = new bool[prefabs.Length];
        wardenInstance = GameObject.FindWithTag("Warden");
        SpawnPlatform();
        SpawnPlatformSets(5);
    }
    
    void Update()
    {
        if (!allPlatformTypesIntroduced) {
            AddNewPlatformTypes();
        }
        if (previousPlatformPosition.y - newPlatformSpawnTrigger.position.y < newPlatformSpawnTriggerShift) {
            SpawnPlatformSets(platformSetSize);
        }
        if (wardenInstance == null){
            SpawnWarden();
            wardenInstance = GameObject.FindWithTag("Warden");
        }
    }
    
    // Platform Type Picker
    private GameObject ChoosePlatformTypes() {
        GameObject[] platformStorage = PlatformStorageOrganizer();
        GameObject chosenPlatformType = platformStorage[Random.Range(0, platformStorage.Length)];
        return chosenPlatformType;
    }

    private GameObject[] PlatformStorageOrganizer() {

        int platformStorageSize = SumArray(platformChoiceRatio);
        GameObject[] tempPlatformStorage = new GameObject[platformStorageSize];
        int countManager = 0;
        
        for (int i = 0; i < platformChoiceRatio.Length; i++) {
            for (int j = 0; j < platformChoiceRatio[i]; j++) {
                tempPlatformStorage[countManager] = platformTypes[i];
                countManager += 1;
            }
        }

        return tempPlatformStorage;
    }

    // Spawning
    private void SpawnWarden() {
        if (newPlatformSpawnTrigger.position.y > wardenSpawnDelay) {
            Instantiate(warden, Vector2.zero, Quaternion.identity);
        }
    }

    private void SpawnPlatform() {
        Vector2 newPlatform = CalculatePosition(previousPlatformPosition);

        Instantiate(ChoosePlatformTypes(), newPlatform, Quaternion.identity);
        previousPlatformPosition = newPlatform;

        Vector2 flameOffset = new Vector2(0f, .5f);
        float tempFlameChance = flameChance;
        if (flameBackup > flameBackupThreshold) {
            tempFlameChance *= flameBackupMultiplayer;
            flameBackup = 0;
        }
        
        if(Random.value <= tempFlameChance){
            Instantiate(flame, newPlatform + flameOffset, Quaternion.identity);
            tempFlameChance = flameChance;
            flameBackup = 0;
        }else {
            flameBackup += 1;
        }
    }

    private void SpawnPlatformSets(int platformLimit) {
        for (int i = 0; i < platformLimit; i++) {
            SpawnPlatform();
        }
    }

    // Platforms
    private void AddNewPlatformTypes() {
        if (playerScoreScript.CurrentScore > icyPlatformStart) {
            platformTypes[1] = prefabs[1];
            specialPlatformsSpawned[0] = true; //0 = icy and 1 = rocky
        }
        if (playerScoreScript.CurrentScore > rockyPlatformStart) {
            platformTypes[2] = prefabs[2];
            specialPlatformsSpawned[1] = true;
        }
        if (playerScoreScript.CurrentScore > movingPlatformStart) {
            platformTypes[3] = prefabs[3];
            specialPlatformsSpawned[2] = true;
        }

        if (specialPlatformsSpawned[0] && specialPlatformsSpawned[1] && specialPlatformsSpawned[2]) {
            allPlatformTypesIntroduced = !allPlatformTypesIntroduced;
        }
    }

    private Vector2 CalculatePosition(Vector2 previousPlatform) {
        int column;
        if (ColumnChecker(previousPlatform) < 0) { //left (-1)
            column = WeightedColumnPicker(weightedLeft);
        }else if (ColumnChecker(previousPlatform) > 0) { //right (1)
            column = WeightedColumnPicker(weightedRight);
        }else { //center (0)
            column = WeightedColumnPicker(weightedCenter);
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

    // Columns
    private int ColumnChecker(Vector2 platformPosition) {
        if (platformPosition.x < -0.5f) {
            return -1;
        }else if (platformPosition.x > 0.5f) {
            return 1;
        }else {
            return 0;
        }
    }

    private int WeightedColumnPicker(int[] weightSide) {

        int weightTotal = weightSide[0] + weightSide[1] + weightSide[2];
        int[] weightManager = new int[weightTotal];
        int countManager = 0;

        for(int i = 0; i < 3; i++) { // 3 can be change if we add more columns
            for (int j = 0; j < weightSide[i]; j++) {
                weightManager[countManager] = i - 1; //minus one to offset to -1, 0, 1 from 0, 1, 2
                countManager += 1;
            }
        }

        int chosenColumn = weightManager[Random.Range(0, weightManager.Length)];

        return chosenColumn;
    }


    // Other
    private int SumArray(int[] arrayToBeSummed) {
        int sum = 0;
        foreach(int element in arrayToBeSummed) {
            sum += element;
        }

        return sum;
    }

    private GameObject[] DefaultTypesStorage() {
        GameObject[] tempPlatformTypes = new GameObject[prefabs.Length];
        for(int i = 0; i < prefabs.Length; i++) {
            tempPlatformTypes[i] = prefabs[0];
        }
        return tempPlatformTypes;
    }
}