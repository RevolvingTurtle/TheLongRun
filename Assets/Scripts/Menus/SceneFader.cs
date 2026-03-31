using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public static SceneFader I;

    public Image fadeImage;
    public float fadeDuration = 0.5f;

    void Awake()
    {
        I = this;
    }

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    IEnumerator FadeIn()
    {
        float t = fadeDuration;

        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime;
            SetAlpha(1f - (t / fadeDuration));
            yield return null;
        }

        SetAlpha(0f);
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            SetAlpha(t / fadeDuration);
            yield return null;
        }

        SetAlpha(1f);
        SceneManager.LoadScene(sceneName);
    }

    void SetAlpha(float alpha)
    {
        Color c = fadeImage.color;
        c.a = Mathf.Clamp01(alpha);
        fadeImage.color = c;
    }
}