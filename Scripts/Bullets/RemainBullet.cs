using Cysharp.Threading.Tasks;
using MessagePipe;
using TGJ2021.InGame.Messages;
using UnityEngine;

namespace TGJ2021
{
    /// <summary>
    /// 一定時間発生し続ける弾
    /// </summary>
    public class RemainBullet : IBullet
    {
        private BulletBehaviour _bulletBehaviour;
        private IPublisher<BulletDeSpawnMessage> _deSpawnPublisher;
        private readonly MoveParameter _moveParameter;
        private readonly int _remainTime;

        public RemainBullet(BulletBehaviour bulletBehaviour, IPublisher<BulletDeSpawnMessage> deSpawnPublisher, int remainTime)
        {
            _bulletBehaviour = bulletBehaviour;
            _bulletBehaviour.Hit += OnHit;
            _deSpawnPublisher = deSpawnPublisher;
            _moveParameter = new MoveParameter(0, 0, 9999, 9999);
            _remainTime = remainTime;
            CountDown().Forget();
        }

        private async UniTask CountDown()
        {
            await _bulletBehaviour.TweenScale(0F, _bulletBehaviour.transform.localScale.x, 0.2F);
            var token = _bulletBehaviour.GetCancellationTokenOnDestroy();
            await UniTask.Delay(_remainTime, cancellationToken: token);
            if (!token.IsCancellationRequested)
            {
                TimeUp();
            }
        }

        private void TimeUp()
        {
            _bulletBehaviour.Banish();
            _deSpawnPublisher.Publish(new BulletDeSpawnMessage { Bullet = this });
        }

        public void Update()
        {
            _bulletBehaviour.UpdateState(_moveParameter);
        }
        

        private void OnHit(GameObject target)
        {
            _bulletBehaviour.SendDamageEvent(target, _moveParameter.Power, _moveParameter.BreakPoint);
        }

        public void ForceBanish()
        {
            _bulletBehaviour.Banish();
        }
    }
}