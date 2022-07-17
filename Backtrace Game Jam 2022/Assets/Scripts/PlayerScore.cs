using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using LootLocker.Requests;

public class PlayerScore : MonoBehaviour
{
    
    public int ID;

    public GameObject player;

    public float CurrentScore;
    [SerializeField] public float HighScore;

    int leaderboardID = 3999;
    public TMP_Text playerNames;
    public TMP_Text HoldScore;
    public TMP_Text Score;

    // Start is called before the first frame update
    void Start()
    {
        HighScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentScore = player.transform.position.y;
        if (CurrentScore > HighScore){
            HighScore = CurrentScore;
            Score.text = "Score: " + HighScore.ToString("F0");
        }
    }

    public void SubmitScore()
    {
        StartCoroutine(DeathRoutine());
    }


    IEnumerator DeathRoutine(){
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1f);
        int realscore = (int)HighScore;
        yield return SubmitScoreRoutine(realscore);
        yield return FetchTopHighscoresRoutine();
        // Time.timeScale = 1f;
    }


    

    public IEnumerator SubmitScoreRoutine(int scoreToUpload){
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardID, (response) =>
        {
            if(response.success){
                Debug.Log("Successfully uploaded score");
                done = true;
            }else{
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(()=>done == false);
    }

    public IEnumerator FetchTopHighscoresRoutine(){
        bool done = false;
        LootLockerSDKManager.GetScoreList(leaderboardID, 20, 0, (response) =>
        {
            if(response.success){
                string tempPlayerNames = "Names\n\n";
                string tempPlayerScores = "Scores\n\n";

                LootLockerLeaderboardMember [] members = response.items;

                for (int i = 0; i < members.Length; i++){
                    tempPlayerNames += members[i].rank + ".  ";
                    if(members[i].player.name != ""){
                        tempPlayerNames += members[i].player.name;
                    }else{
                        tempPlayerNames += members[i].player.id;
                    }
                    tempPlayerScores += members[i].score + "\n";
                    tempPlayerNames += "\n";
                }
                done = true;
                playerNames.text = tempPlayerNames;
                HoldScore.text = tempPlayerScores;
            }else{
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}
