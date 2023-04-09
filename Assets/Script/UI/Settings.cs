using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    #region FloatVariables
    [HideInInspector] public float BGMMixerVolume;
    [HideInInspector] public float SFXMixerVolume;
    [HideInInspector] public float masterMixerVolume;
    #endregion

    #region OtherVariables
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider BGMMixerVolumeSlider;
    [SerializeField] private Slider SFXMixerVolumeSlider;
    [SerializeField] private Slider masterMixerVolumeSlider;
    [SerializeField] private Toggle fullscreenToogle;
    [SerializeField] private Button backButton;
    private int fullscreenIndicator;
    #endregion

    private void Start()
    {
        fullscreenIndicator = PlayerPrefs.GetInt("IsFullscreen",1);

        BGMMixerVolume = PlayerPrefs.GetFloat("BGMVolume",0);
        SFXMixerVolume = PlayerPrefs.GetFloat("SFXVolume",0);
        masterMixerVolume = PlayerPrefs.GetFloat("MasterVolume",0);

        audioMixer.SetFloat("BGM_Volume",BGMMixerVolume);
        audioMixer.SetFloat("SFX_Volume",SFXMixerVolume);
        audioMixer.SetFloat("Master_Volume",masterMixerVolume);

        BGMMixerVolumeSlider.value = BGMMixerVolume;
        SFXMixerVolumeSlider.value = SFXMixerVolume;
        masterMixerVolumeSlider.value = masterMixerVolume;

        if(fullscreenIndicator == 1) fullscreenToogle.isOn = true;
        else if(fullscreenIndicator == 0) fullscreenToogle.isOn = false;

        backButton.onClick.AddListener(() => {
            StartCoroutine(HideSettings());
        });

        gameObject.SetActive(false);
    }
    
    public void UpdateBGMSound(float value)
    {
        audioMixer.SetFloat("BGM_Volume",value);
        PlayerPrefs.SetFloat("BGMVolume",value);
    }
    public void UpdateSFXSound(float value)
    {
        audioMixer.SetFloat("SFX_Volume",value);
        PlayerPrefs.SetFloat("SFXVolume",value);
    }

    public void UpdateMasterSound(float value)
    {
        audioMixer.SetFloat("Master_Volume",value);
        PlayerPrefs.SetFloat("MasterVolume",value);
    }

    public void FullScreenControl(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        if(isFullscreen) PlayerPrefs.SetInt("IsFullscreen",1);
        else if(!isFullscreen) PlayerPrefs.SetInt("IsFullscreen",0);
    }

    public void ShowSettings()
    {
        gameObject.SetActive(true);
        
        void UpdateAlpha(float alpha)
        {
            GetComponent<CanvasGroup>().alpha = alpha;
        }

        LeanTween.value(gameObject, UpdateAlpha, 0.0f, 1.0f, 0.5f);
    }

    IEnumerator HideSettings()
    {
        void UpdateAlpha(float alpha)
        {
            GetComponent<CanvasGroup>().alpha = alpha;
        }

        LeanTween.value(gameObject, UpdateAlpha, 1.0f, 0.0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
