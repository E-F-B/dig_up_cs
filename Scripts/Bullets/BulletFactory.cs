using System;
using System.Collections.Generic;
using System.Linq;
using MessagePipe;
using Sirenix.OdinInspector;
using TGJ2021.InGame.Messages;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace TGJ2021
{

    public enum BulletUser
    {
        PlayerBullet,
        EnemyBullet
    }
    
    [Serializable]
    public class BulletMap
    {
        [PreviewField(ObjectFieldAlignment.Center)]
        public Sprite Icon;

        [PreviewField]
        public List<Sprite> colors;
        public BulletType Type;
        public BulletBehaviour BulletPrefab;
        
        private Sprite GetRandomSprite() => colors[Random.Range(0, colors.Count)];

        public Sprite GetSpriteByBulletColor(BulletColor bulletColor)
        {
            if (bulletColor.color == BulletColor.RANDOM)
            {
                return GetRandomSprite();
            }

            int index = bulletColor.color % colors.Count;
            return colors[index];
        }
    }

    public interface IBulletFactory
    {
        IBullet Create(BulletMeta bulletMeta, MoveParameter moveParameter, Vector3 position);
    }
    
    public class BulletFactory : MonoBehaviour, IBulletFactory
    {
        [TableList(ShowIndexLabels = true)]
        public List<BulletMap> _bulletMaps;

        private IPublisher<BulletSpawnMessage> _publisher;
        private IPublisher<BulletDeSpawnMessage> _deSpawnPublisher;

        [SerializeField] private LayerMask playerBulletLayer;
        [SerializeField] private LayerMask enemyBulletLayer;

        private Dictionary<BulletUser, LayerMask> _layerDic = new Dictionary<BulletUser, LayerMask>();

        [Inject]
        public void Construct(IPublisher<BulletSpawnMessage> publisher, IPublisher<BulletDeSpawnMessage> deSpawn)
        {
            _publisher = publisher;
            _deSpawnPublisher = deSpawn;

            _layerDic[BulletUser.PlayerBullet] = playerBulletLayer;
            _layerDic[BulletUser.EnemyBullet] = enemyBulletLayer;
        }

        public IBullet Create(BulletMeta bulletMeta, MoveParameter moveParameter, Vector3 position)
        {
            var map = _bulletMaps.FirstOrDefault(map => map.Type == bulletMeta.Type);
            var bulletBehaviour = Instantiate(map.BulletPrefab, position, Quaternion.identity);
            bulletBehaviour.Initialize(map, bulletMeta);
            bulletBehaviour.SetUp(moveParameter);
            IBullet bullet = map.Type == BulletType.BigWave
                ? (IBullet)new RemainBullet(bulletBehaviour, _deSpawnPublisher, 1000)
                : new Bullet(bulletBehaviour, _deSpawnPublisher, moveParameter);
            _publisher.Publish(new BulletSpawnMessage{SpawnBullet = bullet});
            return bullet;
        }
    }
}