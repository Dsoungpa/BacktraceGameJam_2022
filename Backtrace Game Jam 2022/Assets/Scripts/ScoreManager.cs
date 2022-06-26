using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using LootLocker.Requests;

public class ScoreManager : MonoBehaviour
{

    public PlayerScore leaderboard;
    public TMP_InputField playerNameInputfield;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetupRoutine());
    }


    public void SetPlayerName(){
        LootLockerSDKManager.SetPlayerName(playerNameInputfield.text, (response) =>
        {
            if(response.success)
            {
                Debug.Log("Succesfully set player name");
            }else{
                Debug.Log("Could not set player name" + response.Error);
            }
        });
    }

    IEnumerator SetupRoutine(){
        yield return LoginRoutine();
        yield return leaderboard.FetchTopHighscoresRoutine();
    }

    IEnumerator LoginRoutine(){
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if(response.success)
            {
                Debug.Log("Success");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }else{
                Debug.Log("Failure");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }


}
