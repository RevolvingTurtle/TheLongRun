using UnityEngine;

public class HighScoreDebug : MonoBehaviour
{
    [ContextMenu("Clear scores")]
    public void ClearHighScores()
    {
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.DeleteKey($"HighScore_Name_{i}");
            PlayerPrefs.DeleteKey($"HighScore_Score_{i}");
        }

        PlayerPrefs.Save();
        Debug.Log("scores cleared");
    }
}