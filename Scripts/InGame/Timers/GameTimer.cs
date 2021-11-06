using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using TGJ2021.InGame.Messages;
using VContainer.Unity;

namespace TGJ2021.InGame.Timers
{
    public class GameTimer : IInitializable
    {
        private readonly GameTimerView _timerView;
        private readonly InGameSettings _gameSettings;
        private readonly IAsyncSubscriber<GameStartMessage> _gameStartSubscriber;
        private readonly IPublisher<TimeUpMessage> _timeUpPublisher;

        public GameTimer(GameTimerView timerView, InGameSettings inGameSettings, IAsyncSubscriber<GameStartMessage> gameStartSubscriber, IPublisher<TimeUpMessage> timeUpPublisher)
        {
            _gameSettings = inGameSettings;
            _timerView = timerView;
            _gameStartSubscriber = gameStartSubscriber;
            _timeUpPublisher = timeUpPublisher;
        }
        
        private async UniTask CountDownTimeAsync(int time, CancellationToken token)
        {
            int count = 0;
            while (!token.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
                count++;
                var currentTime = time - count;
                _timerView.UpdateTime(currentTime);
                
                if (count >= time)
                {
                    _timerView.Finish();
                    _timeUpPublisher.Publish(new TimeUpMessage());
                    break;
                }
                if (currentTime <= 10)
                {
                    _timerView.CallCountDown();
                }
            }
        }

        public void Initialize()
        {
            _gameStartSubscriber.Subscribe((async (message, token) =>
            {
                await CountDownTimeAsync(_gameSettings.PlayTime, token);
            }));
            _timerView.UpdateTime(_gameSettings.PlayTime);
        }
    }
}