using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("Run State")]
    public bool isRunning = true;
    bool isGameOver = false;

    [Header("Speed")]
    public float scrollSpeed = 6f;
    public float speedIncreasePerSecond = 0.05f;
    public float maxScrollSpeed = 12f;

    [Header("Scoring")]
    public float scorePerSecond = 10f;
    public int score;
    public float runTime;
    public int obstaclesCleared;

    [Header("Last Run")]
    public int lastRunScore;
    public float lastRunTime;
    public int lastRunObstaclesCleared;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;
    public TMP_Text returnText;
    public TMP_InputField nameInputField;

    bool scoreSubmitted = false;

    const int MaxHighScores = 5;

    void Awake()
    {
        I = this;
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver)
        {
            if (!scoreSubmitted && Input.GetKeyDown(KeyCode.Return))
            {
                SubmitHighScore();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (!scoreSubmitted)
                    SubmitHighScore();

                Time.timeScale = 1f;
                SceneManager.LoadScene("MainMenu");
            }

            return;
        }

        if (!isRunning) return;

        scrollSpeed = Mathf.Min(
            scrollSpeed + speedIncreasePerSecond * Time.deltaTime,
            maxScrollSpeed
        );

        runTime += Time.deltaTime;
        score += Mathf.RoundToInt(scorePerSecond * Time.deltaTime);
    }

    public void AddObstacleScore(int amount)
    {
        if (!isRunning) return;

        score += amount;
        obstaclesCleared++;
    }

    public void GameOver()
    {
        if (!isRunning) return;

        isRunning = false;
        isGameOver = true;

        lastRunScore = score;
        lastRunTime = runTime;
        lastRunObstaclesCleared = obstaclesCleared;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = $"Final Score: {lastRunScore}";

        if (returnText != null)
            returnText.text = "Press R to Return to the Main Menu";

        if (nameInputField != null)
        {
            nameInputField.text = "";
            nameInputField.ActivateInputField();
        }

        Time.timeScale = 0f;
    }

    public void SubmitHighScore()
    {
        if (scoreSubmitted) return;

        string enteredName = "PLAYER";

        if (nameInputField != null && !string.IsNullOrWhiteSpace(nameInputField.text))
            enteredName = nameInputField.text.Trim();

        SaveHighScore(enteredName, lastRunScore);
        scoreSubmitted = true;

        Debug.Log($"score saved {enteredName} - {lastRunScore}");
    }

    void SaveHighScore(string playerName, int newScore)
    {
        List<HighScoreEntry> scores = LoadHighScores();

        scores.Add(new HighScoreEntry(playerName, newScore));

        scores = scores
            .OrderByDescending(entry => entry.score)
            .Take(MaxHighScores)
            .ToList();

        for (int i = 0; i < MaxHighScores; i++)
        {
            if (i < scores.Count)
            {
                PlayerPrefs.SetString($"HighScore_Name_{i}", scores[i].playerName);
                PlayerPrefs.SetInt($"HighScore_Score_{i}", scores[i].score);
            }
            else
            {
                PlayerPrefs.DeleteKey($"HighScore_Name_{i}");
                PlayerPrefs.DeleteKey($"HighScore_Score_{i}");
            }
        }

        PlayerPrefs.Save();
    }

    public List<HighScoreEntry> LoadHighScores()
    {
        List<HighScoreEntry> scores = new List<HighScoreEntry>();

        for (int i = 0; i < MaxHighScores; i++)
        {
            string name = PlayerPrefs.GetString($"HighScore_Name_{i}", "");
            int score = PlayerPrefs.GetInt($"HighScore_Score_{i}", -1);

            if (!string.IsNullOrEmpty(name) && score >= 0)
                scores.Add(new HighScoreEntry(name, score));
        }

        return scores.OrderByDescending(entry => entry.score).ToList();
    }
}