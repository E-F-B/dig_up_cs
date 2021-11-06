using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MessagePipe;
using Rewired;
using TGJ2021.InGame.Messages;
using TGJ2021.InGame.Result;
using TGJ2021.InGame.Rocks;
using TGJ2021.Ranking;
using TGJ2021.TGJ2021.Music;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace TGJ2021.InGame
{
    public class GameSequencer : IAsyncStartable
    {
        private readonly IAsyncPublisher<GameStartMessage> _gameStartPublisher;
        private readonly IAsyncPublisher<GameEndMessage> _gameEndPublisher;

        private readonly RockSpawner _rockSpawner;
        private readonly FadeManager _fadeManager;
        private readonly BGMPlayer _bgmPlayer;
        private readonly ResultBoardScore _resultBoardScore;
        private readonly RankingBoard _rankingBoard;
        private readonly RectTransform _rankingBoardContentRoot;
        private readonly RetryPopupView _retryPopupView;

        public GameSequencer(IAsyncPublisher<GameStartMessage> gameStartPublisher,
            IAsyncPublisher<GameEndMessage> gameEndPublisher, RockSpawner spawner, FadeManager fadeManager,
            BGMPlayer bgmPlayer, ResultBoardScore resultBoardScore, RankingBoard rankingBoard,
            RectTransform rankingBoardContentRoot, RetryPopupView retryPopupView)
        {
            _gameStartPublisher = gameStartPublisher;
            _gameEndPublisher = gameEndPublisher;
            _rockSpawner = spawner;
            _fadeManager = fadeManager;
            _bgmPlayer = bgmPlayer;
            _resultBoardScore = resultBoardScore;
            _rankingBoard = rankingBoard;
            _rankingBoardContentRoot = rankingBoardContentRoot;
            _retryPopupView = retryPopupView;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            var pad = ReInput.players.GetPlayer(0);

            var ingameLoopTokenSource = new CancellationTokenSource();

            var retryTask = UniTask.WaitUntil(() => pad.GetButtonDown("Pause"), cancellationToken: cancellation);
            var playGameTask = PlayGame(cancellation, ingameLoopTokenSource);
            var isRetry = await UniTask.WhenAny(retryTask, playGameTask);
            if (isRetry == 0)
            {
                ingameLoopTokenSource.Cancel();
                await MoveSceneAsync("InGame");
                return;
            }
            
            if (cancellation.IsCancellationRequested)
            {
                return;
            }

            bool isRankIn = await _resultBoardScore.OpenBoard();
            if (cancellation.IsCancellationRequested)
            {
                return;
            }
            
            if (isRankIn)
            {
                await _rankingBoard.OpenRankingBoard(_rankingBoardContentRoot);
                if (cancellation.IsCancellationRequested)
                {
                    return;
                }
            }

            var result = await _retryPopupView.OpenRetryPopup();
            if (cancellation.IsCancellationRequested)
            {
                return;
            }
            var nextScene = result == RetryPopupView.Content.Retry ? "InGame" : "Title";
            await MoveSceneAsync(nextScene);
        }

        private async UniTask PlayGame(CancellationToken token, CancellationTokenSource ingameLoopTokenSource)
        {
            //_bgmPlayer.Play(BGM.InGame);
            await _rockSpawner.InitializeSpawnAsync(20);
            await UniTask.Delay(1000, cancellationToken: token);
            if (token.IsCancellationRequested)
            {
                return;
            }
            
            await _gameStartPublisher.PublishAsync(new GameStartMessage { InGameLoopToken = ingameLoopTokenSource.Token },
                token);
            if (token.IsCancellationRequested)
            {
                return;
            }
            
            ingameLoopTokenSource.Cancel();

            await _gameEndPublisher.PublishAsync(new GameEndMessage(), token);
        }

        private async UniTask MoveSceneAsync(string sceneName)
        {
            await _fadeManager.FadeIn(0.5F, Ease.Linear);
            await SceneManager.UnloadSceneAsync("InGame");

            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            await _fadeManager.FadeOut(0.5F, Ease.Linear);
        }
    }
}