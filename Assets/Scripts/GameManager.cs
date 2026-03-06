using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("Run State")]
    public bool isRunning = true;

    [Header("Speed")]
    public float scrollSpeed = 6f;
    public float speedIncreasePerSecond = 0.05f;

    [Header("Scoring")]
    public float scorePerSecond = 10f;
    public int score;
    public float runTime;
    public int obstaclesCleared;

    [Header("Last Run")]
    public int lastRunScore;
    public float lastRunTime;
    public int lastRunObstaclesCleared;

    const int MaxHighScores = 5;

    void Awake()
    {
        I = this;
    }

    void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (!isRunning) return;

        scrollSpeed += speedIncreasePerSecond * Time.deltaTime;

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

        lastRunScore = score;
        lastRunTime = runTime;
        lastRunObstaclesCleared = obstaclesCleared;

        SaveHighScore(score);

        Debug.Log($"GAME OVER | Time: {lastRunTime:F2} | Score: {lastRunScore} | Obstacles Cleared: {lastRunObstaclesCleared}");

        Time.timeScale = 0f;
    }

    void SaveHighScore(int newScore)
    {
        List<int> scores = LoadHighScores();
        scores.Add(newScore);

        scores = scores
            .OrderByDescending(s => s)
            .Take(MaxHighScores)
            .ToList();

        for (int i = 0; i < MaxHighScores; i++)
        {
            int value = i < scores.Count ? scores[i] : 0;
            PlayerPrefs.SetInt($"HighScore_{i}", value);
        }

        PlayerPrefs.Save();
    }

    public List<int> LoadHighScores()
    {
        List<int> scores = new List<int>();

        for (int i = 0; i < MaxHighScores; i++)
        {
            int value = PlayerPrefs.GetInt($"HighScore_{i}", 0);
            if (value > 0)
                scores.Add(value);
        }

        return scores.OrderByDescending(s => s).ToList();
    }
}