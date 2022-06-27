using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LootLocker.Requests;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNames;
    [SerializeField] private TMP_Text playerScores;
    [SerializeField] private int leaderboardID = 3999;
    [SerializeField] private int leaderboardAmount = 10;

    void Start() {
        StartCoroutine(SetupRoutine());
    }

    IEnumerator SetupRoutine(){
        yield return LoginRoutine();
        yield return FetchTopHighscoresRoutine();
    }

    public IEnumerator FetchTopHighscoresRoutine(){
        bool done = false;
        LootLockerSDKManager.GetScoreList(leaderboardID, leaderboardAmount, 0, (response) =>
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
                playerScores.text = tempPlayerScores;
            }else{
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
    
    IEnumerator LoginRoutine() {
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
