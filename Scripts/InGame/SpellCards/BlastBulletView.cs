using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace TGJ2021.InGame.SpellCards
{
    public class BlastBulletView : MonoBehaviour
    {
        [SerializeField] private int countMilliSecond;
        
        private bool _isStop;

        private MoveParameter _moveParameter;
        private IBulletFactory _bulletFactory;
        public void SetUp(MoveParameter moveParameter, IBulletFactory factory)
        {
            _moveParameter = moveParameter;
            _bulletFactory = factory;

            var euler = transform.eulerAngles;
            euler.z = moveParameter.Direction;
            transform.eulerAngles = euler;
            
            Moving().Forget();
        }

        private async UniTask Moving()
        {
            await this.GetAsyncTriggerEnter2DTrigger().OnTriggerEnter2DAsync();
            await CountDown();
        }

        private async UniTask CountDown()
        {
            await UniTask.Delay(countMilliSecond);
            var meta = new BulletMeta(BulletType.BigWave, BulletUser.PlayerBullet, BulletColor.RandomColor);
            _bulletFactory.Create(meta, new MoveParameter(), transform.position);
            Destroy(gameObject);
        }
        
        private void Update()
        {
            if (_isStop)
            {
                return;
            }

            transform.position += _moveParameter.Velocity * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _isStop = true;
        }
    }
}