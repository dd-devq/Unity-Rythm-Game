using DG.Tweening;
using Hellmade.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD_Core
{
    public class DD_SoundManager : SingletonMono<DD_SoundManager>
    {
        public bool isMute = false;
        [SerializeField] private List<DD_SoundItem> soundFxItems = new List<DD_SoundItem>();
        private Dictionary<SoundFXIndex, DD_SoundItem> dictSoundFXs = new Dictionary<SoundFXIndex, DD_SoundItem>();
        private List<AudioClip> lsBGMs = new List<AudioClip>();
        private AudioSource bgmSource;

        private void Awake()
        {
            dictSoundFXs.Clear();
            for (int i = 0; i < soundFxItems.Count; i++)
            {
                dictSoundFXs.Add(soundFxItems[i].soundFXIndex, soundFxItems[i]);
            }

            bgmSource = GetComponent<AudioSource>();
        }

        #region BGM

        public void AddSoundBGM(AudioClip bgmClip)
        {
            if (isMute)
            {
                return;
            }

            lsBGMs.Clear();
            lsBGMs.Add(bgmClip);
        }

        public float GetLengthBGM()
        {
            if (lsBGMs.Count > 0)
            {
                return lsBGMs[0].length;
            }

            return 0;
        }

        public void PlaySoundBGM(float volume = 1, bool isLoop = false)
        {
            bgmSource.clip = lsBGMs[0];
            bgmSource.Play();
            bgmSource.volume = 0;
            bgmSource.DOFade(volume, 0.25f);
        }

        public void PauseSoundBGM()
        {
            bgmSource.Pause();
        }

        public void Resume()
        {
            bgmSource.Play();
        }

        public void StopSoundBGM()
        {
            bgmSource.Stop();
        }

        public float GetCurrentTimeSoundBGM()
        {
            if (bgmSource == null)
            {
                return 0;
            }

            return bgmSource.time;
        }

        #endregion

        #region SFX

        public void PlaySoundFX(SoundFXIndex soundIndex, bool isLoop = false)
        {
            if (isMute)
            {
                return;
            }

            EazySoundManager.PlaySound(dictSoundFXs[soundIndex].soundFXClip, isLoop);
        }


        public void StopSoundFX(SoundFXIndex soundFXIndex)
        {
            Audio audio = EazySoundManager.GetAudio(dictSoundFXs[soundFXIndex].soundFXClip);
            if (audio != null)
            {
                audio.Stop();
            }
        }

        public void StopAllSoundFX()
        {
            EazySoundManager.StopAllSounds();
        }

        public bool CheckSoundFXAvailable(SoundFXIndex soundFXIndex)
        {
            Audio audio = EazySoundManager.GetAudio(dictSoundFXs[soundFXIndex].soundFXClip);
            if (audio != null && audio.IsPlaying)
            {
                return true;
            }

            return false;
        }

        #endregion

        public void Mute()
        {
            isMute = true;
            StopSoundBGM();
            StopAllSoundFX();
        }

        public void UnMute()
        {
            for (SoundFXIndex i = SoundFXIndex.Click; i < SoundFXIndex.COUNT; i++)
            {
                StopSoundFX(i);
            }

            isMute = false;
        }
    }

    public enum SoundFXIndex
    {
        Click = 0,
        One,
        Two,
        Three,
        Go,
        SoundMenu,
        GameOver,
        MissNote,
        ConfirmMenu,
        Victory,
        COUNT
    }

    public class DD_SoundItem
    {
        public SoundFXIndex soundFXIndex;
        public AudioClip soundFXClip;
    }
}