using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;


public class OptionMenu : MonoBehaviour
{
    public Slider volume;
    public TextMeshProUGUI valueText;
    public AudioMixer mixer;
    public AudioSource audioSource;

    public Slider sensitivity;

    void Start()
    {
        mixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("Volume",1)) * 20);
    }
    public void Update()
    {

    }
    public void OnChangeSlider(float value)
    {
        valueText.SetText($"{value.ToString("N4")}");
        mixer.SetFloat("Volume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }

}
