using TMPro;
using UnityEngine;

namespace TGJ2021.InGame.Scores
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;

        public void UpdateScore(int score)
        {
            _scoreText.text = score.ToString();
        }
    }
}