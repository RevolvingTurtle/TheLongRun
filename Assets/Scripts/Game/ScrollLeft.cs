using UnityEngine;

public class ScrollLeft : MonoBehaviour
{
    public float destroyX = -15f;
    public int scoreValue = 100;

    bool hasScored = false;
    Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (!GameManager.I.isRunning) return;

        transform.position += Vector3.left * GameManager.I.scrollSpeed * Time.deltaTime;

        if (!hasScored && player != null && transform.position.x < player.position.x)
        {
            hasScored = true;
            GameManager.I.AddObstacleScore(scoreValue);
        }

        if (transform.position.x < destroyX)
            Destroy(gameObject);
    }
}