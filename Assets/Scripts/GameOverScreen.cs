using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public Text gameOverText;
    void Start()
    {
        StatTracker st = GameObject.FindObjectOfType<StatTracker>();
        gameOverText.text = string.Format(
            "Game Over!\n\nScore: {0:N0}\nLetters typed: {1:N0}/{5:N0} ({8:N2}%)\nWords typed: {2:N0}/{6:N0} ({9:N2}%)\nMistypes: {3:N0}\nAccuracy: {7:N2}%\nHighest streak: {4:N0}",
            st.Score,
            st.LettersTyped,
            st.WordsTyped,
            st.TotalFails,
            st.HighestStreak,
            st.LettersSpawned,
            st.WordsSpawned,
            st.Accuracy,
            (st.LettersTyped / (float)st.LettersSpawned) * 100f,
            (st.WordsTyped / (float)st.WordsSpawned) * 100f);
        Destroy(st.gameObject);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
