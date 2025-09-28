using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    public Slider volume;
    public Slider sensitivity;

    public void Update()
    {
        GameManager.Instance.volume = volume.value;
        GameManager.Instance.sensitivity = sensitivity.value;
    }
}
