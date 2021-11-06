using Cysharp.Threading.Tasks;
using MessagePipe;
using Otoshiai.Utility;
using TGJ2021.InGame.Messages;
using TGJ2021.InGame.Players;
using UnityEngine;

namespace TGJ2021.InGame.SpellCards
{
    public class MineBlast
    {
        private readonly BlastBulletView _blastBulletViewPrefab;
        private readonly Player _player;
        private readonly IBulletFactory _bulletFactory;
        private readonly MomoyoView _momoyoView;
        private readonly IPublisher<SpellCardMessage> _spellCardPublisher;

        public MineBlast(Player player, BlastBulletView bulletViewPrefab, IBulletFactory bulletFactory, MomoyoView momoyoView, IPublisher<SpellCardMessage> spellCardPublisher)
        {
            _player = player;
            _blastBulletViewPrefab = bulletViewPrefab;
            _bulletFactory = bulletFactory;
            _momoyoView = momoyoView;
            _spellCardPublisher = spellCardPublisher;
        }
        
        public async UniTask DoSpellSequence()
        {
            _spellCardPublisher.Publish(new SpellCardMessage());
            await MomoyoFrameIn();
            await ShotBlast();
            await MomoyoFrameOut();
        }
        
        private async UniTask MomoyoFrameIn()
        {
            await _momoyoView.FrameIn();
        }

        private async UniTask ShotBlast()
        {
            var direction = BulletMath.EulerAngle(_momoyoView.transform.position, _player.Position);
            var defaultIntervalEuler = 45;
            for (int i = -1; i <= 1; i++)
            {
                var blast = GameObject.Instantiate(_blastBulletViewPrefab, _momoyoView.transform.position,
                    Quaternion.identity);

                var dir = direction + defaultIntervalEuler * i;

                var moveParameter = new MoveParameter(dir + Random.Range(-15, 15), 10, 0, 0);
                blast.SetUp(moveParameter, _bulletFactory);
            }

            await UniTask.Delay(2000);
        }

        private async UniTask MomoyoFrameOut()
        {
            await _momoyoView.FrameOut();
        }
    }
}