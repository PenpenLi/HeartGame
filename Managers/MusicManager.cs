using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public static void SetVolume()
    {
        AudioSource bgm = GameObject.Find("bgm").GetComponent<AudioSource>();
        AudioSource se = GameObject.Find("se").GetComponent<AudioSource>();
        bgm.volume = Globals.bgmVolume;
        se.volume = Globals.seVolume;
        if (GameObject.Find("voice")!=null)
        {
            AudioSource voice = GameObject.Find("voice").GetComponent<AudioSource>();
            voice.volume = Globals.voiceVolume;
        }
    }
    public static void PlayBgm(string bgm)
    {
        AudioSource audio = GameObject.Find("bgm").GetComponent<AudioSource>();
        audio.loop = true;
        AudioClip ac = (AudioClip)GameFuncs.GetResource("Music/Bgm/"+bgm);
        audio.clip = ac;
        audio.Play();
        SetVolume();
    }
    public static void StopBgm()
    {
        AudioSource audio = GameObject.Find("bgm").GetComponent<AudioSource>();
        audio.Stop();
    }
    public static void PlaySe(string se)
    {
        AudioSource audio = GameObject.Find("se").GetComponent<AudioSource>();
        audio.loop = false;
        AudioClip ac = (AudioClip)GameFuncs.GetResource("Music/Se/" + se);
        audio.clip = ac;
        audio.Play();
        SetVolume();
    }
    public static void PlayVoice(string voice)
    {
        AudioSource audio = GameObject.Find("voice").GetComponent<AudioSource>();
        audio.loop = false;
        AudioClip ac = (AudioClip)GameFuncs.GetResource("Music/Voice/" + voice);
        audio.clip = ac;
        audio.Play();
        SetVolume();
    }
}
