using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance = null;

    public AudioMixer audioMixer;
    public GameObject optionPannel;

    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider seSlider;

    public AudioSource bgmSound;
    public AudioSource seSound;
    public AudioClip[] bgmClip;
    public AudioClip[] seClip;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            PannelControl();
        }
    }

    public static AudioManager Instance => _instance == null ? null : _instance;

    public void SetMasterVolume()
    {
        audioMixer.SetFloat("Master", Mathf.Log10(masterSlider.value) * 20);
        float _masterVolume = Mathf.Floor(masterSlider.value * 100);
    }

    public void SetBgmVolume()
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(bgmSlider.value) * 20);
        float _bgmVolume = Mathf.Floor(bgmSlider.value * 100);
    }

    public void SetSeVolume()
    {
        audioMixer.SetFloat("SE", Mathf.Log10(seSlider.value) * 20);
        float _seVolume = Mathf.Floor(seSlider.value * 100);
        seSound.clip = seClip[0];

        if (!seSound.isPlaying)
        {
            seSound.Play();
        }
    }

    public void PannelControl()
    {
        Click();

        if (optionPannel.activeSelf)
        {
            GameManager.Instance.ContinueGame();
        }
        else
        {
            GameManager.Instance.PauseGame();
        }

        optionPannel.SetActive(!optionPannel.activeSelf);
    }

    #region AudioClip

    public void Title()
    {
        bgmSound.clip = bgmClip[0];
        bgmSound.Play();
    }

    public void Game()
    {
        bgmSound.clip = bgmClip[1];
        bgmSound.Play();
    }


    public void Click()
    {
        seSound.clip = seClip[0];
        seSound.Play();
    }

    #endregion AudioClip
}
