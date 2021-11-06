using System;
using System.Collections.Generic;
using E7.Introloop;
using UnityEngine;

namespace TGJ2021.TGJ2021.Music
{
    public enum BGM
    {
        InGame
    }
    public class BGMPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        
        [SerializeField] private AudioClip _main;

        private Dictionary<BGM, AudioClip> _bgmList = new Dictionary<BGM, AudioClip>();
        private BGM _current;

        public IntroloopAudio IntroloopAudio;

        private void Awake()
        {
            _bgmList[BGM.InGame] = _main;
            //Play(BGM.InGame);
        }

        private void Start()
        {
            Play(BGM.InGame);
        }

        private void Play(BGM bgm)
        {
            IntroloopPlayer.Instance.Play(IntroloopAudio);
            return;
            var clip = _bgmList[bgm];
            if (_audioSource.clip != null && _audioSource.clip.Equals(clip))
            {
                return;
            }

            _audioSource.clip = clip;
            _audioSource.Play();
            _current = BGM.InGame;
        }

        public void Stop()
        {
            _audioSource.Stop();
        }
    }
}