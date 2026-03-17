using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HighScoresUI : MonoBehaviour
{
    public TMP_Text highScoresText;

    void OnEnable()
    {
        if (GameManager.I == null)
        {
            highScoresText.text = BuildHighScoreText(LoadHighScoresDirectly());
            return;
        }

        highScoresText.text = BuildHighScoreText(GameManager.I.LoadHighScores());
    }
    string BuildHighScoreText(List<HighScoreEntry> scores)
    {
        string text = "Top 5 Scores\n\n";

        for (int i = 0; i < 5; i++)
        {
            if (i < scores.Count)
            {
                string line = $"{i + 1}. {scores[i].playerName} - {scores[i].score}";

                if (i == 0)
                {
                    // Gold + Bold + Slightly Larger
                    line = $"<b><size=120%><color=#FFD700>{line}</color></size></b>";
                }

                text += line + "\n";

            }
            else
            {
                text += $"{i + 1}. ---\n";
            }
        }

        return text;
    }

    List<HighScoreEntry> LoadHighScoresDirectly()
    {
        List<HighScoreEntry> scores = new List<HighScoreEntry>();

        for (int i = 0; i < 5; i++)
        {
            string name = PlayerPrefs.GetString($"HighScore_Name_{i}", "");
            int score = PlayerPrefs.GetInt($"HighScore_Score_{i}", -1);

            if (!string.IsNullOrEmpty(name) && score >= 0)
                scores.Add(new HighScoreEntry(name, score));
        }

        scores.Sort((a, b) => b.score.CompareTo(a.score));
        return scores;
    }
}