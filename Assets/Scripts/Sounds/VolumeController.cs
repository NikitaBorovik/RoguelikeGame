using App.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI volumeText;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("Volume");
        }
        else
        {
            AudioListener.volume = 1f;
        }
        if (volumeText != null)
            volumeText.text = (AudioListener.volume * 10).ToString("0");
    }

    public void VolumeUp()
    {
        var prevVolume = AudioListener.volume;
        if (AudioListener.volume < 1f)
        {
            AudioListener.volume += 0.1f;
        }
        if (AudioListener.volume > 1f)
            AudioListener.volume = 1f;
        PlayerPrefs.SetFloat("Volume", AudioListener.volume);
        if (volumeText != null)
            volumeText.text = (AudioListener.volume * 10).ToString("0");
    }
    public void VolumeDown()
    {
        var prevVolume = AudioListener.volume;
        if (AudioListener.volume > 0f)
        {
            AudioListener.volume -= 0.1f;
        }
        if (AudioListener.volume < 0f)
            AudioListener.volume = 0f;
        PlayerPrefs.SetFloat("Volume", AudioListener.volume);
        if (volumeText != null)
            volumeText.text = (AudioListener.volume * 10).ToString("0");
    }
}
