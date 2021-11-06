using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TGJ2021.InGame.SpellCards
{
    public class SpellCardUI : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _overlay;
        [SerializeField] private TMP_Text _spellCountText;

        public void HideSpellCard()
        {
            _spellCountText.text = "×0";
            _overlay.gameObject.SetActive(true);
        }
    }
}