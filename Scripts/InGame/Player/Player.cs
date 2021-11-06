using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using Rewired;
using TGJ2021.InGame.Messages;
using UnityEngine;
using VContainer.Unity;

namespace TGJ2021.InGame.Players
{
    public enum MoveMode
    {
        Normal,
        Slow
    }
    
    public class Player : IInitializable
    {
        private readonly PlayerBehaviour _behaviour;
        private int _directionEuler = -1;

        private readonly IAsyncSubscriber<GameStartMessage> _gameStartSubscriber;
        private readonly IAsyncSubscriber<GameEndMessage> _gameEndSubscriber;

        private readonly IPublisher<PlayerDeadMessage> _playerDeadPublisher;
        private readonly IPublisher<PlayerReSpawnMessage> _playerReSpawnPublisher;

        private bool _isDead;

        private MoveMode _moveMode = MoveMode.Normal;
        private Rewired.Player _pad;
        public Player(PlayerBehaviour playerBehaviour, IAsyncSubscriber<GameStartMessage> asyncSubscriber,
            IAsyncSubscriber<GameEndMessage> gameEndSubscriber, IPublisher<PlayerDeadMessage> playerDeadPublisher,
            IPublisher<PlayerReSpawnMessage> playerReSpawnPublisher)
        {
            _behaviour = playerBehaviour;
            _gameStartSubscriber = asyncSubscriber;
            _gameEndSubscriber = gameEndSubscriber;
            _playerDeadPublisher = playerDeadPublisher;
            _playerReSpawnPublisher = playerReSpawnPublisher;
            _pad = ReInput.players.GetPlayer(0);
        }

        public Vector3 Position => _behaviour.transform.position;
        public Transform Transform => _behaviour.transform;

        private async UniTask MoveLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var currentDirection = GetDirection();
                if (currentDirection >= 0)
                {
                    _directionEuler = currentDirection;
                    switch (_moveMode)
                    {
                        case MoveMode.Normal:
                            _behaviour.Move(_directionEuler);
                            break;
                        case MoveMode.Slow:
                            _behaviour.SlowMove(_directionEuler);
                            break;
                    }
                }
                await UniTask.WaitWhile(IsDead, cancellationToken: token);
                await UniTask.Yield();
            }
        }

        private Interval _shotInterval = new Interval(100);

        private async UniTask ShotLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.WaitWhile(IsDead, cancellationToken: token);
                await ShotAsync(token);
            }
        }
        
        private async UniTask ShotAsync(CancellationToken token)
        {
            await _shotInterval.CountIntervalAsync();

            if (_moveMode == MoveMode.Slow)
            {
                _behaviour.ConcentrateShot(_directionEuler);
            }
            else
            {
                _behaviour.SpreadShot(_directionEuler);
            }

        }

        private async UniTask ChangeMoveModeLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.WaitUntil(() => _pad.GetButtonDown("Slow"), cancellationToken: token);
                _moveMode = MoveMode.Slow;
                _behaviour.ShowCollision();
                await UniTask.WaitUntil(() => _pad.GetButtonUp("Slow"), cancellationToken: token);
                _moveMode = MoveMode.Normal;
                _behaviour.HideCollision();
            }
        }

        public void Initialize()
        {
            _gameStartSubscriber.Subscribe(((message, token) =>
            {
                EnablePlayerControl(message.InGameLoopToken);
                return UniTask.CompletedTask;
            }));

            _gameEndSubscriber.Subscribe(((message, token) =>
            {
                _behaviour.Damaged -= OnDamaged;
                return UniTask.CompletedTask;
            }));

            _behaviour.Damaged += OnDamaged;
        }

        private void OnDamaged(int damage)
        {
            Die();
        }

        private void Die()
        {
            _isDead = true;
            _playerDeadPublisher.Publish(new PlayerDeadMessage());
            _behaviour.Dead();
            ReSpawnAsync().Forget();            
        }

        private bool IsDead() => _isDead;

        private async UniTask ReSpawnAsync()
        {
            await UniTask.Delay(3000);
            _behaviour.ReSpawn();
            _isDead = false;
            _playerReSpawnPublisher.Publish(new PlayerReSpawnMessage());
            _behaviour.UpdateWalkAnimation(_directionEuler);
        }

        private void EnablePlayerControl(CancellationToken token)
        {
            ShotLoopAsync(token).Forget();
            MoveLoopAsync(token).Forget();
            ChangeMoveModeLoopAsync(token).Forget();
        }

        private int GetDirection()
        {
            float horizontal = _pad.GetAxisRaw("Horizontal");
            float vertical = _pad.GetAxisRaw("Vertical");
            bool left = horizontal < 0;
            bool right = horizontal > 0;
            bool up = vertical > 0; //Input.GetKey(KeyCode.UpArrow);
            bool down = vertical < 0; //Input.GetKey(KeyCode.DownArrow);

            if (left & up)
            {
                return 45;
            }
            else if (left & down)
            {
                return 135;
            }
            else if (right & up)
            {
                return 315;
            }
            else if (right & down)
            {
                return 225;
            }
            else if (up)
            {
                return 0;
            }
            else if (down)
            {
                return 180;
            }
            else if (left)
            {
                return 90;
            }
            else if (right)
            {
                return 270;
            }
            else
            {
                return -1;
            }
        }
    }


    public class Interval
    {
        private int _milliSecond;

        public Interval(int milliSecond)
        {
            _milliSecond = milliSecond;
        }

        public UniTask CountIntervalAsync()
        {
            return UniTask.Delay(_milliSecond);
        }
    }
}