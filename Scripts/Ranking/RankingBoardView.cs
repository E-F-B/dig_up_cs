using TGJ2021.Score;
using UnityEngine;

namespace TGJ2021.Ranking
{
    public class RankingBoardView : MonoBehaviour
    {
        [SerializeField] private RankRecordView _rankRecordPrefab;

        [SerializeField] private RectTransform _contentRoot;

        public void CreateRankRecord(int rank, PlayerScore playerScore, bool isHighlight)
        {
            var record = Instantiate(_rankRecordPrefab, _contentRoot);
            record.ShowScore(rank, playerScore);
            if (isHighlight)
            {
                record.HighlightMyScore();
            }
        }

        public void Discard()
        {
            Destroy(gameObject);
        }
    }
}