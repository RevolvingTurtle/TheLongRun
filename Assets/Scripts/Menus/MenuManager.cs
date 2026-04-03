using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject highScoresPanel;
    public GameObject settingsPanel;

    [Header("Audio")]
    public Slider audioSlider;
    public AudioSource menuMusicSource;
    public float musicFadeDuration = 0.25f;
    public AudioSource uiAudioSource;



    const string AudioVolumeKey = "AudioVolume";

    bool isStartingGame = false;
    float baseMusicVolume = 1f;

    [Header("UI Sounds")]
    public AudioClip buttonClickClip;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(AudioVolumeKey, 1f);
        AudioListener.volume = savedVolume;

        if (audioSlider != null)
        {
            audioSlider.value = savedVolume;
            audioSlider.onValueChanged.AddListener(OnAudioChanged);
        }

        if (menuMusicSource != null)
        {
            baseMusicVolume = menuMusicSource.volume;
        }
    }

    public void PlayButtonClick()
    {
        if (uiAudioSource != null && buttonClickClip != null)
        {
            uiAudioSource.PlayOneShot(buttonClickClip);
        }
    }

    public void PlayGame()
    {
        PlayButtonClick();

        if (isStartingGame) return;
        isStartingGame = true;

        Time.timeScale = 1f;
        StartCoroutine(PlayGameSequence());
    }

    IEnumerator PlayGameSequence()
    {
        if (menuMusicSource != null)
        {
            float startVolume = baseMusicVolume;
            float t = 0f;

            while (t < musicFadeDuration)
            {
                t += Time.unscaledDeltaTime;
                float normalized = t / musicFadeDuration;
                menuMusicSource.volume = Mathf.Lerp(startVolume, 0f, normalized);
                yield return null;
            }

            menuMusicSource.volume = 0f;
        }

        if (SceneFader.I != null)
            SceneFader.I.FadeToScene("GameScene");
        else
            SceneManager.LoadScene("GameScene");
    }

    public void OpenHighScores()
    {
        PlayButtonClick();

        mainPanel.SetActive(false);
        highScoresPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        PlayButtonClick();

        mainPanel.SetActive(false);
        highScoresPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackToMain()
    {
        PlayButtonClick();

        mainPanel.SetActive(true);
        highScoresPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        PlayButtonClick();

        Application.Quit();
        Debug.Log("Quit Game");
    }
    public void OnAudioChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat(AudioVolumeKey, value);
        PlayerPrefs.Save();
    }
}