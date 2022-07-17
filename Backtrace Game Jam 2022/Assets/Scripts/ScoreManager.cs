using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
using LootLocker.Requests;

public class ScoreManager : MonoBehaviour
{

    public PlayerScore leaderboard;
    public TMP_InputField playerNameInputfield;
    [SerializeField] private TMP_Text playersrank;
    string playerID;
    [SerializeField] private PlayerScore scorescript;
    


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetupRoutine());
        playerID = PlayerPrefs.GetString("PlayerID");
    }


    public void SetPlayerName(){
        LootLockerSDKManager.SetPlayerName(playerNameInputfield.text, (response) =>
        {
            if(response.success)
            {
                Debug.Log("Succesfully set player name");
                StartCoroutine(WaitRank());
            }else{
                Debug.Log("Could not set player name" + response.Error);
            }
        });
    }

    public void PlayerRank(){
        LootLockerSDKManager.GetMemberRank(3999,playerID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Found Rank");
                int rank = response.rank;
                playersrank.text = "Placed rank " + rank.ToString() + "\n" + scorescript.HighScore.ToString("F0");
            }else{
                Debug.Log("No rank");
            }
        });
    }

    IEnumerator WaitRank(){
        yield return new WaitForSecondsRealtime(3f);
        print("waited");
        PlayerRank();
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
