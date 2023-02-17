using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject inputField;
    [SerializeField] private HealthBarScript healthBarScriptReference;

    void Update() {
        if (healthBarScriptReference.currentHealth <= 0) {
            EndGame();
        }
    }

    void OnTriggerEnter2D(Collider2D otherObject) {
        if (otherObject.gameObject.tag == "Warden") {
            EndGame();
        }
    }

    void EndGame() {
        Time.timeScale = 0; //pause game
        leaderboardPanel.SetActive(true);
        inputField.SetActive(true);
    }
}
