using UnityEngine;

public class BackgroundPropSpawner : MonoBehaviour
{
    public GameObject[] propPrefabs;

    [Header("Spawn Position")]
    public float spawnX = 12f;
    public float minY = -3.5f;
    public float maxY = -2.0f;

    [Header("Spawn Timing")]
    public float minSpawnDelay = 2f;
    public float maxSpawnDelay = 5f;

    float timer;

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        if (GameManager.I == null || !GameManager.I.isRunning) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnProp();
            ResetTimer();
        }
    }

    void SpawnProp()
    {
        if (propPrefabs == null || propPrefabs.Length == 0)
        {
            Debug.LogWarning("no prop prefabs assigned");
            return;
        }

        GameObject prefab = propPrefabs[Random.Range(0, propPrefabs.Length)];
        float spawnY = Random.Range(minY, maxY);

        Instantiate(prefab, new Vector3(spawnX, spawnY, 0f), Quaternion.identity);
    }

    void ResetTimer()
    {
        timer = Random.Range(minSpawnDelay, maxSpawnDelay);
    }
}