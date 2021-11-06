using TMPro;
using UnityEngine;

namespace TGJ2021.InGame.Scores
{
    public class HiScoreView : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _hiScoreText;

        public void UpdateScore(int score)
        {
            _hiScoreText.text = score.ToString();
        }
    }
}