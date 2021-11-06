using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Rewired;
using TGJ2021.Ranking;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace TGJ2021.Titles
{
    public class TitleSequencer : IInitializable, IAsyncStartable, IDisposable
    {
        private FadeManager _fadeManager;
        private RankingBoard _rankingBoard;
        private RectTransform _rankingRoot;
        private CancellationTokenSource _cancellationTokenSource;

        public TitleSequencer(FadeManager fadeManager, RankingBoard rankingBoard, RectTransform rankingRoot)
        {
            _fadeManager = fadeManager;
            _rankingBoard = rankingBoard;
            _rankingRoot = rankingRoot;
        }
        public void Initialize()
        {

        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await ContentSelectLoop(cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return;
            }
            
            await _fadeManager.FadeIn(0.5F, Ease.Linear);
            await SceneManager.LoadSceneAsync("InGame", LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("InGame"));
            await SceneManager.UnloadSceneAsync("Title");
            await _fadeManager.FadeOut(0.5F, Ease.Linear);
        }

        private async UniTask ContentSelectLoop(CancellationToken token)
        {
            var pad = ReInput.players.GetPlayer(0);
            while (!token.IsCancellationRequested)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                var ingameTask = UniTask.WaitUntil(() => pad.GetButtonDown("Action"), cancellationToken: _cancellationTokenSource.Token);
                var rankingTask = UniTask.WaitUntil(() => pad.GetButtonDown("Spell"), cancellationToken: _cancellationTokenSource.Token);

                var result = await UniTask.WhenAny(ingameTask, rankingTask);

                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }
                
                _cancellationTokenSource.Cancel();

                switch (result)
                {
                    case 0:
                        return;
                    case 1:
                        await _rankingBoard.OpenRankingBoard(_rankingRoot);
                        break;
                }
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}