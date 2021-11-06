using MessagePipe;
using TGJ2021.InGame.Messages;
using VContainer.Unity;

namespace TGJ2021.InGame
{
    public class BulletUpdater : IInitializable, ITickable
    {
        private readonly ISubscriber<BulletSpawnMessage> _bulletSpawnSubscriber;
        private readonly ISubscriber<BulletDeSpawnMessage> _bulletDeSpawnSubscriber;
        private readonly ISubscriber<PlayerReSpawnMessage> _playerReSpawnSubscriber;
        private readonly ISubscriber<SpellCardMessage> _spellCardSubscriber;
        
        private readonly BulletCounter _bulletCounter;

        public BulletUpdater(ISubscriber<BulletSpawnMessage> spawnSubscriber, ISubscriber<BulletDeSpawnMessage> deSpawnSubscriber, ISubscriber<PlayerReSpawnMessage> playerReSpawnSubscriber, ISubscriber<SpellCardMessage> spellCardSubscriber, BulletCounter bulletCounter)
        {
            _bulletSpawnSubscriber = spawnSubscriber;
            _bulletDeSpawnSubscriber = deSpawnSubscriber;
            _playerReSpawnSubscriber = playerReSpawnSubscriber;
            _spellCardSubscriber = spellCardSubscriber;
            _bulletCounter = bulletCounter;
        }

        public void Tick()
        {
            for (var i = _bulletCounter.Count - 1; i >= 0; i--)
            {
                _bulletCounter[i].Update();
            }
        }

        public void Initialize()
        {
            _bulletSpawnSubscriber.Subscribe(OnSpawnBullet);
            _bulletDeSpawnSubscriber.Subscribe(OnDeSpawnBullet);
            _playerReSpawnSubscriber.Subscribe(OnPlayerReSpawn);
            _spellCardSubscriber.Subscribe(message => _bulletCounter.ForceBanishAll());
        }

        private void OnPlayerReSpawn(PlayerReSpawnMessage message)
        {
            _bulletCounter.ForceBanishAll();
        }
        

        private void OnSpawnBullet(BulletSpawnMessage message) => _bulletCounter.AddBullet(message.SpawnBullet);

        private void OnDeSpawnBullet(BulletDeSpawnMessage deSpawnMessage) => _bulletCounter.RemoveBullet(deSpawnMessage.Bullet);
    }
}