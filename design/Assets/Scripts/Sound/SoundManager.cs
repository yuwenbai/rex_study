using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {
        #region BACKGROUND_MUSIC

        public AudioClip backgroundMusicClipsArray;
        public AudioSource backgroundMusicSource;

        #endregion

        #region SFX_SOUNDS

        public AudioClip SFXSounds;
        public AudioSource SFXAudioSource;

    #endregion

    public Button back;
    public Button sfx;

    private void Start()
    {
        back.onClick.AddListener(OnClickBack);
        sfx.onClick.AddListener(OnClickSfx);
    }
    private void OnClickBack()
    {
        Debug.Log("Button Clicked. ClickHandler.");
        PlayRandomMusic();
    }
    private void OnClickSfx()
    {
        Debug.Log("Button Clicked. sfx.");
        PlayRandomSFXSounds();
    }

    #region PUBLIC_METHODS

    public void PlayRandomMusic()
        {
            backgroundMusicSource.clip = backgroundMusicClipsArray;
            backgroundMusicSource.Play();
        }

        public void PlayRandomSFXSounds()
        {
            SFXAudioSource.PlayOneShot(SFXSounds);
        }

        #endregion
}
