using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Otoshiai.Utility;
using TohoResources;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TGJ2021
{
    public interface IAttackable
    {
        
    }

    public interface IDamageable : IEventSystemHandler
    {
        void Damage(int damage, int breakPoint);
    }

    public interface IBreakable
    {
        void Break();
    }
    
    public class BulletBehaviour : MonoBehaviour
    {
        [SerializeField] 
        private SpriteRenderer _currentSprite;
        
        private MoveParameter _moveParameter;

        public event Action<GameObject> Hit;

        private BulletMap _bulletMap;

        private CircleCollider2D _collider2D;
        
        public void Initialize(BulletMap bulletMap, BulletMeta bulletMeta)
        {
            _bulletMap = bulletMap;
            gameObject.layer =  LayerMask.NameToLayer(bulletMeta.User.ToString());
            _currentSprite.sprite = _bulletMap.GetSpriteByBulletColor(bulletMeta.Color);
            _collider2D = GetComponent<CircleCollider2D>();
        }

        public void SetUp(MoveParameter moveParameter)
        {
            var euler = transform.eulerAngles;
            euler.z = moveParameter.Direction;
            transform.eulerAngles = euler;
        }
        
        
        public void UpdateState(MoveParameter moveParameter)
        {
            transform.position += moveParameter.Velocity * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Hit?.Invoke(other.gameObject);
        }

        public void SendDamageEvent(GameObject target, int power, int breakPoint)
        {
            ExecuteEvents.Execute<IDamageable>(target, null, ((handler, data) => handler.Damage(power, breakPoint)));            
        }

        public async UniTask TweenScale(float from, float to, float duration)
        {
            await DOTween.To(() => from,
                    value => transform.localScale = Vector3.one * value,
                    to,
                    duration)
                .SetEase(Ease.Linear);
        }

        /// <summary>
        /// 消滅
        /// </summary>
        public void Banish()
        {
            BanishAsync().Forget();
        }

        private async UniTask BanishAsync()
        {
            var token = this.GetCancellationTokenOnDestroy();
            _collider2D.enabled = false;
            await DOTween.To(
                () => 0F,
                value => _currentSprite.color = Color.Lerp(Color.white, new Color(1F, 1F, 1F, 0), value),
                1F,
                0.3F);

            if (!token.IsCancellationRequested)
            {
                Destroy(gameObject);
            }
        }
    }
}
