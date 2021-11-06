using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using MessagePipe;
using TGJ2021.InGame.Messages;
using TGJ2021.Score;

namespace TGJ2021.InGame.Result
{
    public class ResultBoardScore
    {
        private readonly ResultBoardView _resultBoardView;
        private readonly ISubscriber<ScoreCalculateMessage> _subscriber;
        private readonly LocalStorage _localStorage;
        private ScoreCalculateMessage _message;
        private readonly ScoreManager _scoreManager;

        public ResultBoardScore(ResultBoardView boardView, ISubscriber<ScoreCalculateMessage> subscriber,
            LocalStorage localStorage, ScoreManager scoreManager)
        {
            _resultBoardView = boardView;
            _subscriber = subscriber;
            _subscriber.Subscribe((message => _message = message));
            _localStorage = localStorage;
            _scoreManager = scoreManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>スコアをサーバーに送信したらtrue</returns>
        public async UniTask<bool> OpenBoard()
        {
            _resultBoardView.Show();
            _resultBoardView.UpdateScore(_message.currentScore);

            int limit = ScoreManager.RankLimit;
            var scores = await _scoreManager.LoadScore(limit);
            if (IsRankIn(scores, limit))
            {
                _resultBoardView.ShowSendRankingContents(_localStorage.LoadUserName());
                var result = await _resultBoardView.TapSendButtonAsync();
                if (result.isSendScore)
                {
                    await SendScore(_message.currentScore, result.name);
                }
                _resultBoardView.Hide();
                return true;
            }

            await UniTask.Delay(2000);
            _resultBoardView.Hide();
            return false;
        }

        private bool IsRankIn(List<PlayerScore> scores, int limit)
        {
            return scores.Count < limit || scores.Any(score => _message.currentScore > score.Point);
        }

        private async UniTask SendScore(int score, string name)
        {
            var playerScore = new PlayerScore(name, score, _localStorage.LoadUserId());
            await _scoreManager.SaveScore(playerScore);
            _localStorage.SaveUserName(name);
        }
    }
}