using UnityEngine;

public class ScrollLeftBackground : MonoBehaviour
{
    public float speedMultiplier = 0.6f;
    public float destroyX = -15f;

    void Update()
    {
        if (GameManager.I == null || !GameManager.I.isRunning) return;

        transform.position += Vector3.left * GameManager.I.scrollSpeed * speedMultiplier * Time.deltaTime;

        if (transform.position.x < destroyX)
            Destroy(gameObject);
    }
}