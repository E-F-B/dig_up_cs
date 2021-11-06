using TGJ2021.Score;
using TMPro;
using UnityEngine;

namespace TGJ2021.Ranking
{
    public class RankRecordView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _rank;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _point;
        [SerializeField] private TMP_Text _date;

        public void ShowScore(int rank, PlayerScore playerScore)
        {
            _rank.text = $"{rank}位";
            _name.text = playerScore.PlayerName;
            _point.text = playerScore.Point.ToString();
            _date.text = playerScore.Date.ToString("yyyy/MM/dd");
        }

        public void HighlightMyScore()
        {
            _name.color = Color.red;
        }
    }
}