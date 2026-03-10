using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HighScoresUI : MonoBehaviour
{
    public TMP_Text highScoresText;

    void OnEnable()
    {
        List<int> scores = LoadHighScores();

        highScoresText.text = "Top 5 Scores\n";

        for (int i = 0; i < 5; i++)
        {
            string value = i < scores.Count ? scores[i].ToString() : "---";
            highScoresText.text += $"{i + 1}. {value}\n";
        }
    }

    List<int> LoadHighScores()
    {
        List<int> scores = new List<int>();

        for (int i = 0; i < 5; i++)
        {
            int score = PlayerPrefs.GetInt($"HighScore_{i}", 0);
            if (score > 0)
                scores.Add(score);
        }

        scores.Sort((a, b) => b.CompareTo(a));
        return scores;
    }
}