using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Ground Obstacles")]
    public GameObject[] groundObstaclePrefabs;

    [Header("Overhead Obstacle (Duck)")]
    public GameObject girderPrefab;
    [Range(0f, 1f)] public float girderChance = 0.25f;

    [Header("Spawn Position")]
    public float spawnX = 12f;
    public float groundY = -4.39f;
    public float girderY = -2.8f; // tweak until standing hits, duck passes

    [Header("Spacing (in world units)")]
    public float minGap = 6f;
    public float maxGap = 12f;

    [Header("Difficulty")]
    public float gapShrinkPerSecond = 0.15f;
    public float minGapLimit = 4.5f;
    public float maxGapLimit = 9f;

    [Header("Unlock")]
    public float unlockGirderAtSpeed = 7.5f; // prevents early unfair deaths

    float nextSpawnX;

    void Start()
    {
        nextSpawnX = 0f;
    }

    void Update()
    {
        if (!GameManager.I.isRunning) return;

        minGap = Mathf.Max(minGapLimit, minGap - gapShrinkPerSecond * Time.deltaTime);
        maxGap = Mathf.Max(maxGapLimit, maxGap - gapShrinkPerSecond * Time.deltaTime);

        nextSpawnX -= GameManager.I.scrollSpeed * Time.deltaTime;

        if (nextSpawnX <= 0f)
        {
            Spawn();
            float gap = Random.Range(minGap, maxGap);
            nextSpawnX = gap;
        }
    }

    void Spawn()
    {
        bool girdersUnlocked = GameManager.I.scrollSpeed >= unlockGirderAtSpeed;
        bool spawnGirder = girdersUnlocked && girderPrefab != null && Random.value < girderChance;

        if (spawnGirder)
        {
            Instantiate(girderPrefab, new Vector3(spawnX, girderY, 0f), Quaternion.identity);
            return;
        }

        if (groundObstaclePrefabs == null || groundObstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("Spawner: No groundObstaclePrefabs assigned.");
            return;
        }

        var prefab = groundObstaclePrefabs[Random.Range(0, groundObstaclePrefabs.Length)];
        Instantiate(prefab, new Vector3(spawnX, groundY, 0f), Quaternion.identity);
    }
}