using Cysharp.Threading.Tasks;
using Rewired;
using TGJ2021.Score;
using UnityEngine;

namespace TGJ2021.Ranking
{
    public class RankingBoard
    {
        private readonly RankingBoardView _rankingBoardPrefab;

        private readonly ScoreManager _scoreManager;
        private readonly LocalStorage _localStorage;

        public RankingBoard(RankingBoardView rankingBoardPrefab, ScoreManager scoreManager, LocalStorage localStorage)
        {
            _rankingBoardPrefab = rankingBoardPrefab;
            _localStorage = localStorage;
            _scoreManager = scoreManager;
        }
        
        public async UniTask OpenRankingBoard(RectTransform root)
        {
            var myId = _localStorage.LoadUserId();
            var scores = await _scoreManager.LoadScore(ScoreManager.RankLimit);
            var board = GameObject.Instantiate(_rankingBoardPrefab, root);
            for (var i = 0; i < scores.Count; i++)
            {
                var score = scores[i];
                board.CreateRankRecord(i + 1, score, score.UserId == myId);
            }

            var pad = ReInput.players.GetPlayer(0);
            await UniTask.WaitUntil(() => pad.GetButtonDown("Spell"));
            
            board.Discard();
        }
    }
}