using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Ground Obstacles")]
    public GameObject[] groundObstaclePrefabs;

    [Header("Overhead Obstacle (Duck)")]
    public GameObject girderPrefab;
    [Range(0f, 1f)] public float girderChance = 0.25f;

    [Header("Raised Platform + Spikes")]
    public GameObject raisedPlatformPrefab;
    public GameObject raisedSpikePrefab;
    [Range(0f, 1f)] public float raisedPatternChance = 0.15f;
    public float platformY = -3.2f;
    public float spikeOnPlatformY = -2.6f;
    public float unlockRaisedPatternAtSpeed = 8.5f;
    public float platformEdgeInset = 0.15f;

    [Header("Spawn Position")]
    public float spawnX = 12f;
    public float groundY = -4.39f;
    public float girderY = -2.8f;

    [Header("Spacing (in world units)")]
    public float minGap = 6f;
    public float maxGap = 12f;

    [Header("Difficulty")]
    public float gapShrinkPerSecond = 0.15f;
    public float minGapLimit = 4.5f;
    public float maxGapLimit = 9f;

    [Header("Unlock")]
    public float unlockGirderAtSpeed = 7.5f;

    float nextSpawnX;
    bool lastSpawnWasRaisedPattern = false;

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
        bool raisedPatternUnlocked = GameManager.I.scrollSpeed >= unlockRaisedPatternAtSpeed;

        bool canSpawnRaisedPattern =
            !lastSpawnWasRaisedPattern &&
            raisedPatternUnlocked &&
            raisedPlatformPrefab != null &&
            raisedSpikePrefab != null;

        bool spawnRaisedPattern =
            canSpawnRaisedPattern &&
            Random.value < raisedPatternChance;

        if (spawnRaisedPattern)
        {
            SpawnRaisedPlatformPattern();
            lastSpawnWasRaisedPattern = true;
            return;
        }

        bool girdersUnlocked = GameManager.I.scrollSpeed >= unlockGirderAtSpeed;
        bool spawnGirder = girdersUnlocked && girderPrefab != null && Random.value < girderChance;

        if (spawnGirder)
        {
            Instantiate(girderPrefab, new Vector3(spawnX, girderY, 0f), Quaternion.identity);
            lastSpawnWasRaisedPattern = false;
            return;
        }

        if (groundObstaclePrefabs == null || groundObstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("Spawner: No groundObstaclePrefabs assigned.");
            return;
        }

        var prefab = groundObstaclePrefabs[Random.Range(0, groundObstaclePrefabs.Length)];
        Instantiate(prefab, new Vector3(spawnX, groundY, 0f), Quaternion.identity);
        lastSpawnWasRaisedPattern = false;
    }

    void SpawnRaisedPlatformPattern()
    {
        GameObject platform = Instantiate(
            raisedPlatformPrefab,
            new Vector3(spawnX, platformY, 0f),
            Quaternion.identity
        );

        float platformWidth = platform.GetComponent<SpriteRenderer>().bounds.size.x;

        float topSpikeX = spawnX + (platformWidth / 2.5f) - platformEdgeInset;

        Instantiate(raisedSpikePrefab, new Vector3(topSpikeX, spikeOnPlatformY, 0f), Quaternion.identity);
    }
}