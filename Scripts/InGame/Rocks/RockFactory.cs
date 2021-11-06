using MessagePipe;
using TGJ2021.InGame.Messages;
using TGJ2021.InGame.Players;
using TGJ2021.InGame.ShotStrategy;
using UnityEngine;

namespace TGJ2021.InGame.Rocks
{
    
    public class RockFactory
    {
        private readonly Transform _parent;
        private readonly IBulletFactory _bulletFactory;
        private readonly IPublisher<RockBreakMessage> _publisher;
        private readonly IPublisher<AddScoreMessage> _scorePublisher;
        private readonly RockSettings _rockSettings;
        
        private readonly Player _player;

        public RockFactory(Transform parent, Player player, IBulletFactory bulletFactory,
            IPublisher<RockBreakMessage> publisher, IPublisher<AddScoreMessage> scorePublisher, RockSettings rockSettings)
        {
            _parent = parent;
            _bulletFactory = bulletFactory;
            _player = player;
            _publisher = publisher;
            _scorePublisher = scorePublisher;
            _rockSettings = rockSettings;
        }

        public IRock Create(RockMeta rockMeta, Vector3 position)
        {
            var map = _rockSettings.FindRock(rockMeta.Size, rockMeta.Type);
            var prefab = map.Prefab;
            
            var instance = GameObject.Instantiate(prefab, position, Quaternion.identity, _parent);
            var danmaku = CreateDanmaku(_bulletFactory, map.DanmakuSetting);
            return new Rock(instance, _player, _publisher, _scorePublisher, danmaku, map.Life, map.GetScore(),
                new RockMeta(map.Size, map.Type));
        }

        private IDanmaku CreateDanmaku(IBulletFactory bulletFactory, DanmakuSetting danmakuSetting)
        {
            switch (danmakuSetting.ShotType)
            {
                case ShotType.Spread:
                    var spreadMeta = danmakuSetting.BuildSpreadShotMeta();
                    return new SpreadShot(bulletFactory, spreadMeta);
                case ShotType.Snipe:
                    var snipeMeta = danmakuSetting.BuildSnipeShotMeta();
                    return new SnipeShot(bulletFactory, snipeMeta);
                default:
                    return null;
            }
        }
    }
}