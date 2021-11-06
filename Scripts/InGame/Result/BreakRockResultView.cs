using System;
using TGJ2021.InGame.Rocks;
using TMPro;
using UnityEngine;
using VContainer;

namespace TGJ2021.InGame.Result
{
    public class BreakRockResultView : MonoBehaviour
    {
        private BreakRockCounter _rockCounter;

        [SerializeField] private TMP_Text _redSmallText;
        [SerializeField] private TMP_Text _redMiddleText;
        [SerializeField] private TMP_Text _redLargeText;
        [SerializeField] private TMP_Text _blueSmallText;
        [SerializeField] private TMP_Text _blueMiddleText;
        [SerializeField] private TMP_Text _blueLargeText;
        [SerializeField] private TMP_Text _yellowSmallText;
        [SerializeField] private TMP_Text _yellowMiddleText;
        [SerializeField] private TMP_Text _yellowLargeText;
        
        [Inject]
        public void Construct(BreakRockCounter rockCounter)
        {
            _rockCounter = rockCounter;
        }

        private void OnEnable()
        {
            _redSmallText.text = _rockCounter.GetBrokeRockCount(RockSize.Small, RockType.Shoot).ToString();
            _redMiddleText.text = _rockCounter.GetBrokeRockCount(RockSize.Middle, RockType.Shoot).ToString();
            _redLargeText.text = _rockCounter.GetBrokeRockCount(RockSize.Large, RockType.Shoot).ToString();
            _blueSmallText.text = _rockCounter.GetBrokeRockCount(RockSize.Small, RockType.Spread).ToString();
            _blueMiddleText.text = _rockCounter.GetBrokeRockCount(RockSize.Middle, RockType.Spread).ToString();
            _blueLargeText.text = _rockCounter.GetBrokeRockCount(RockSize.Large, RockType.Spread).ToString();
            _yellowSmallText.text = _rockCounter.GetBrokeRockCount(RockSize.Small, RockType.Rare).ToString();
            _yellowMiddleText.text = _rockCounter.GetBrokeRockCount(RockSize.Middle, RockType.Rare).ToString();
            _yellowLargeText.text = _rockCounter.GetBrokeRockCount(RockSize.Large, RockType.Rare).ToString();
        }
    }
}