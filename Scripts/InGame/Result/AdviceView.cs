using TGJ2021.InGame.Result;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace TGJ2021
{
    public class AdviceView : MonoBehaviour
    {
        [SerializeField] private AdviceSetting _adviceSetting;


        [SerializeField] private Text _text;

        [SerializeField] private Image _image;

        private void Start()
        {
            var advices = _adviceSetting.Advices;
            var adv = advices[Random.Range(0, advices.Count)];

            _text.text = adv._adviceText;
            _image.sprite = adv._sprite;
        }
    }
}
