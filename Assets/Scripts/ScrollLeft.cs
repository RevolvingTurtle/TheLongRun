using UnityEngine;

public class ScrollLeft : MonoBehaviour
{
    public float destroyX = -15f;

    void Update()
    {
        if (!GameManager.I.isRunning) return;

        transform.position += Vector3.left * GameManager.I.scrollSpeed * Time.deltaTime;

        if (transform.position.x < destroyX)
            Destroy(gameObject);
    }
}