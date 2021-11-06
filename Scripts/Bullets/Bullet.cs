using MessagePipe;
using TGJ2021.InGame.Messages;
using UnityEngine;

namespace TGJ2021
{
    public interface IBullet
    {
        void Update();
        void ForceBanish();
    }
    
    public class Bullet : IBullet
    {
        private BulletBehaviour _bulletBehaviour;
        private IPublisher<BulletDeSpawnMessage> _deSpawnPublisher;
        private readonly MoveParameter _moveParameter;

        public Bullet(BulletBehaviour bulletBehaviour, IPublisher<BulletDeSpawnMessage> deSpawnPublisher, MoveParameter moveParameter)
        {
            _bulletBehaviour = bulletBehaviour;
            _bulletBehaviour.Hit += OnHit;
            _deSpawnPublisher = deSpawnPublisher;
            _moveParameter = moveParameter;
        }

        public void Update()
        {
            _bulletBehaviour.UpdateState(_moveParameter);
        }
        

        private void OnHit(GameObject target)
        {
            _bulletBehaviour.SendDamageEvent(target, _moveParameter.Power, _moveParameter.BreakPoint);
            _bulletBehaviour.Banish();
            _deSpawnPublisher.Publish(new BulletDeSpawnMessage { Bullet = this });
        }

        public void ForceBanish()
        {
            _bulletBehaviour.Banish();
        }
    }
}