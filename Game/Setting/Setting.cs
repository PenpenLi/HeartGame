using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    Slider bgmSlider;
    Slider seSlider;
    Slider voiceSlider;
    // Use this for initialization
    void Start()
    {
        MusicManager.SetVolume();
        bgmSlider = UIManager.GetSlider("bgmSlider");
        seSlider = UIManager.GetSlider("seSlider");
        voiceSlider = UIManager.GetSlider("voiceSlider");

        bgmSlider.onValueChanged.AddListener(delegate { OnBgmSliderChange(); });
        seSlider.onValueChanged.AddListener(delegate { OnSeSliderChange(); });
        voiceSlider.onValueChanged.AddListener(delegate { OnVoiceSliderChange(); });

        bgmSlider.value = Globals.bgmVolume;
        seSlider.value = Globals.seVolume;
        voiceSlider.value = Globals.voiceVolume;
    }
    void OnBgmSliderChange()
    {
        if (bgmSlider != null)
        {
            if (bgmSlider.gameObject.activeInHierarchy)
            {
                Globals.bgmVolume = bgmSlider.value;
            }
        }
        MusicManager.SetVolume();
    }
    void OnSeSliderChange()
    {
        if (seSlider != null)
        {
            if (seSlider.gameObject.activeInHierarchy)
            {
                Globals.seVolume = seSlider.value;
            }
        }
        MusicManager.SetVolume();
    }
    void OnVoiceSliderChange()
    {
        if (voiceSlider != null)
        {
            if (voiceSlider.gameObject.activeInHierarchy)
            {
                Globals.voiceVolume = voiceSlider.value;
            }
        }
        MusicManager.SetVolume();
    }
}
