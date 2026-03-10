using UnityEngine;
using TMPro;

public class RunStatsUI : MonoBehaviour
{
    public TMP_Text finalTimeText;
    public TMP_Text finalScoreText;
    public TMP_Text obstaclesClearedText;

    void OnEnable()
    {
        if (GameManager.I == null) return;

        finalTimeText.text = $"Time Survived: {GameManager.I.lastRunTime:F2}";
        finalScoreText.text = $"Final Score: {GameManager.I.lastRunScore}";
        obstaclesClearedText.text = $"Obstacles Cleared: {GameManager.I.lastRunObstaclesCleared}";
    }
}