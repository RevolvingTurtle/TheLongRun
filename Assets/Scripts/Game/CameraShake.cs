using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float duration = 0.2f;
    public float magnitude = 0.15f;

    private Vector3 originalPosition;
    private float timer;

    void LateUpdate()
    {
        if (timer > 0f)
        {
            Vector2 offset2D = Random.insideUnitCircle * magnitude;
            transform.localPosition = originalPosition + new Vector3(offset2D.x, offset2D.y, 0f);
            timer -= Time.unscaledDeltaTime;
        }
        else
        {
            // Keep updating the resting position in case something else moves the camera
            originalPosition = transform.localPosition;
        }
    }

    public void Shake()
    {
        originalPosition = transform.localPosition;
        timer = duration;
    }
}