using UnityEngine;
using TMPro;

public class GameplayHUD : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text scoreText;

    void Update()
    {
        if (GameManager.I == null) return;

        timeText.text = $"Time: {GameManager.I.runTime:F2}";
        scoreText.text = $"Score: {GameManager.I.score}";
    }
}