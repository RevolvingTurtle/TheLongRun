using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HighScoresUI : MonoBehaviour
{
    public TMP_Text highScoresText;

    void OnEnable()
    {
        if (GameManager.I == null) return;

        List<int> scores = GameManager.I.LoadHighScores();

        highScoresText.text = "Top 5 Scores\n";

        for (int i = 0; i < 5; i++)
        {
            string value = i < scores.Count ? scores[i].ToString() : "---";
            highScoresText.text += $"{i + 1}. {value}\n";
        }
    }
}