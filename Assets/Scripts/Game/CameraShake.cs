using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float duration = 0.2f;
    public float magnitude = 0.15f;

    Vector3 originalPosition;
    float timer;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (timer > 0)
        {
            transform.localPosition = originalPosition + Random.insideUnitSphere * magnitude;
            timer -= Time.unscaledDeltaTime;
        }
        else
        {
            transform.localPosition = originalPosition;
        }
    }

    public void Shake()
    {
        timer = duration;
    }
}