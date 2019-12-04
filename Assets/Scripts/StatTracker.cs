using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTracker : MonoBehaviour
{
    // Stat-keeping variables.
    public int LettersTyped { get; private set; }
    public int LettersSpawned { get; private set; }
    public int WordsTyped { get; private set; }
    public int WordsSpawned { get; private set; }
    public int TotalFails { get; private set; }
    public long Score { get; private set; }
    public int HighestStreak { get; private set; }
    public float Accuracy { get { return (LettersTyped / (float) (LettersTyped+TotalFails))*100f; } }

    public void ChangeLettersTyped(int deltaValue)
    {
        LettersTyped += deltaValue;
    }
    public void ChangeLettersSpawned(int deltaValue)
    {
        LettersSpawned += deltaValue;
    }
    public void ChangeWordsSpawned(int deltaValue)
    {
        WordsSpawned += deltaValue;
    }
    public void ChangeWordsTyped(int deltaValue)
    {
        WordsTyped += deltaValue;
    }
    public void ChangeTotalFails(int deltaValue)
    {
        TotalFails += deltaValue;
    }
    public void ChangeScore(int deltaValue)
    {
        Score += deltaValue;
    }
    public void ChangeHighestStreak(int deltaValue)
    {
        HighestStreak += deltaValue;
    }
}
