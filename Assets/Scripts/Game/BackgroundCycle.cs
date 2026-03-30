using UnityEngine;

public class BackgroundCycle : MonoBehaviour
{
    public float changeInterval = 30f;
    public Color navyBlue = new Color(0.82f, 0.69f, 0.69f);
    public Color crimson = new Color(0.72f, 0.82f, 0.69f);
    public Color emerald = new Color(0.69f, 0.82f, 0.80f);
    Camera cam;

    Color[] colors;

    void Awake()
    {
        cam = GetComponent<Camera>();

        colors = new Color[]
        {
            navyBlue,
            crimson,
            emerald
        };
    }

    void Update()
    {
        if (GameManager.I == null) return;

        float time = GameManager.I.runTime;

        int index = Mathf.FloorToInt(time / changeInterval) % colors.Length;
        int nextIndex = (index + 1) % colors.Length;

        float t = (time % changeInterval) / changeInterval;

        cam.backgroundColor = Color.Lerp(colors[index], colors[nextIndex], t);
    }
}