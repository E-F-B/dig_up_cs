using System;
using TMPro;
using UnityEngine;
using VContainer;

namespace TGJ2021.InGame
{
    public class BulletDangerView : MonoBehaviour
    {
        private BulletCounter _bulletCounter;

        [SerializeField] 
        private TMP_Text countText;

        [SerializeField] private Color _dengerColorStart;
        [SerializeField] private Color _dengerEnd;
        
        [Inject]
        public void Construct(BulletCounter bulletCounter)
        {
            _bulletCounter = bulletCounter;
        }

        private void Update()
        {
            /*
            var rate = _bulletCounter.ScoreRate;
            countText.text = rate.ToString("F2");
            countText.color = Color.Lerp(_dengerColorStart, _dengerEnd, rate - 1F);
            */
        }

        public void UpdateDangerRate(float rate)
        {
            int percent = (int)(rate * 100) - 100;
            countText.text = percent == 100 ? "大百足" : $"{percent}%";
        }
    }
}