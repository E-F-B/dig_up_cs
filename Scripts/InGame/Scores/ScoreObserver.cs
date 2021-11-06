using Cysharp.Threading.Tasks;
using MessagePipe;
using TGJ2021.InGame.Messages;
using VContainer.Unity;

namespace TGJ2021.InGame.Scores
{
    public class ScoreObserver : IInitializable
    {
        private ISubscriber<AddScoreMessage> _scoreSubscriber;
        private IAsyncSubscriber<GameEndMessage> _gameEndSubscriber;
        private IPublisher<ScoreCalculateMessage> _scorePublisher;
        private readonly ScoreView _scoreView;
        private readonly HiScoreView _hiScoreView;
        private readonly LocalStorage _localStorage;
        private readonly BulletCounter _bulletCounter;


        private int _currentScore = 0;

        public ScoreObserver(ISubscriber<AddScoreMessage> scoreSubscriber, ScoreView scoreView,
            IAsyncSubscriber<GameEndMessage> gameEndSubscriber, LocalStorage storage, HiScoreView hiScoreView,
            IPublisher<ScoreCalculateMessage> scorePublisher, BulletCounter bulletCounter)
        {
            _scoreSubscriber = scoreSubscriber;
            _scoreView = scoreView;
            _gameEndSubscriber = gameEndSubscriber;
            _localStorage = storage;
            _hiScoreView = hiScoreView;
            _scorePublisher = scorePublisher;
            _bulletCounter = bulletCounter;
        }

        public void Initialize()
        {
            var dispose = _scoreSubscriber.Subscribe((message =>
            {
                _currentScore += (int)(message.Point * _bulletCounter.ScoreRate);
                _scoreView.UpdateScore(_currentScore);
            }));

            _gameEndSubscriber.Subscribe((async (message, token) =>
            {
                // ゲーム終了後も余弾で岩が壊される可能性があるのでちょっと待つ
                await UniTask.Delay(3000, cancellationToken: token);
                SaveScore();
                dispose.Dispose();
            }));

            var hiScore = _localStorage.LoadHiScore();
            _hiScoreView.UpdateScore(hiScore);
        }

        private void SaveScore()
        {
            var beforeHiScore = _localStorage.LoadHiScore();
            if (_currentScore > beforeHiScore)
            {
                _localStorage.SaveHiScore(_currentScore);
            }

            _scorePublisher.Publish(new ScoreCalculateMessage
                { currentScore = _currentScore, hiScore = beforeHiScore });
        }
    }
}