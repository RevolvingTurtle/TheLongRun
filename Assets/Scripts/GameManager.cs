using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    public bool isRunning = true;

    public float scrollSpeed = 6f;
    public float speedIncreasePerSecond = 0.05f;

    public CameraShake cameraShake;

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
    }

    public void GameOver()
    {
        if (!isRunning) return;

        isRunning = false;

        Debug.Log("GAME OVER: Player hit obstacle");

        cameraShake.Shake();

        Time.timeScale = 0f;
    }
}