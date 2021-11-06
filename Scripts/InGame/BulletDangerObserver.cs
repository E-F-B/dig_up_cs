using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using VContainer.Unity;

namespace TGJ2021.InGame
{
    public class BulletDangerObserver : ITickable, IDisposable
    {
        private readonly BulletDangerView _bulletDangerView;
        private readonly BulletCounter _bulletCounter;
        private readonly PostProcessVolume _postProcessVolume;
        private Vignette _vignette;
        private Color _defaultColor;
        private float _defaultIntensity;

        private Color _maxColor = Color.red;
        private float _maxIntensity = 0.7F;

        public BulletDangerObserver(BulletDangerView bulletDangerView, BulletCounter bulletCounter, PostProcessVolume volume)
        {
            _bulletDangerView = bulletDangerView;
            _bulletCounter = bulletCounter;
            _postProcessVolume = volume;
            _vignette = _postProcessVolume.profile.GetSetting<Vignette>();
            _defaultColor = _vignette.color.value;
            _defaultIntensity = _vignette.intensity.value;
        }

        public void Tick()
        {
            var rate = _bulletCounter.ScoreRate;
            _bulletDangerView.UpdateDangerRate(rate);
            _vignette.color.Override(Color.Lerp(_defaultColor, _maxColor, rate - 1F));
            _vignette.intensity.Override(Mathf.Lerp(_defaultIntensity, _maxIntensity, rate - 1F));

        }

        public void Dispose()
        {
            _vignette.color.Override(_defaultColor);
            _vignette.intensity.Override(_defaultIntensity);
        }
    }
}