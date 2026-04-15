using UnityEngine;

public class RainController : MonoBehaviour
{
    public float baseRainSpeed = 12f;
    public float speedMultiplier = 0.5f;

    private ParticleSystem rainSystem;
    private ParticleSystem.MainModule mainModule;

    void Awake()
    {
        rainSystem = GetComponent<ParticleSystem>();
        mainModule = rainSystem.main;
    }

    void Update()
    {
        if (GameManager.I == null) return;

        float targetSpeed = baseRainSpeed + (GameManager.I.scrollSpeed * speedMultiplier);
        mainModule.startSpeed = targetSpeed;
    }
}
