using System;
using Otoshiai.Utility;
using TohoResources;
using UnityEngine;
using VContainer;

namespace TGJ2021.InGame.Players
{
    public class PlayerBehaviour : MonoBehaviour, IDamageable
    {
        [SerializeField] private PlayerParameter _playerParameter;

        [SerializeField]
        private CharacterAnimationPlayer _animationPlayer;

        [SerializeField] 
        private SpriteRenderer _collision;

        [SerializeField] 
        private GameObject _playerDeadAnimation;

        private IBulletFactory _bulletFactory;

        [SerializeField] 
        private UshirodoBehaviour _ushirodoBehaviour;

        public event Action<int> Damaged;
        
        [Inject]
        public void Construct(IBulletFactory bulletFactory)
        {
            _bulletFactory = bulletFactory;
            _ushirodoBehaviour.SetUpFixPosition(transform);
        }
        
        public void Move(int euler)
        {
            var direction = BulletMath.Forward(euler);
            transform.position += direction * _playerParameter.speed * Time.deltaTime;
            _animationPlayer.PlayWalkAnimation(GetDirectionByEuler(euler));
            _ushirodoBehaviour.UpdatePosition(euler, transform.position);
        }
        
        public void SlowMove(int euler)
        {
            var direction = BulletMath.Forward(euler);
            transform.position += direction * _playerParameter.slowSpeed * Time.deltaTime;
            _animationPlayer.PlayWalkAnimation(GetDirectionByEuler(euler));
            _ushirodoBehaviour.UpdatePosition(euler, transform.position);
        }

        public void UpdateWalkAnimation(int euler)
        {
            _animationPlayer.PlayWalkAnimation(GetDirectionByEuler(euler));
            _ushirodoBehaviour.UpdatePosition(euler, transform.position);
        }

        public void ConcentrateShot(float direction)
        {
            var moveParameter = new MoveParameter(direction, _playerParameter.normalShotSpeed,
                _playerParameter.normalShotPower, 1);
            var fix = BulletMath.Forward(direction + 90);
            var position = transform.position;
            int centerIndex = (_playerParameter.shotCount - 1) / 2;
            for (int i = -centerIndex; i <= centerIndex; i++)
            {
                var meta = new BulletMeta(i % 2 == 0 ? BulletType.Marisa_Shot_Left : BulletType.Marisa_Shot_Right,
                    BulletUser.PlayerBullet, BulletColor.RandomColor);

                _bulletFactory.Create(meta, moveParameter, position + fix * 0.3F * i);
            }
        }

        public void SpreadShot(float direction)
        {
            int count = _playerParameter.shotCount;
            // 可動最高範囲は常に90
            int interval = 70 / count;
            int centerIndex = (count - 1) / 2;
            float speed = _playerParameter.normalShotSpeed;
            for (int i = -centerIndex; i <= centerIndex; i++)
            {
                var moveParameter =
                    new MoveParameter(direction + (interval * i), speed, _playerParameter.normalShotPower, 2);
                var meta = new BulletMeta(i % 2 == 0 ? BulletType.Marisa_Shot_Left : BulletType.Marisa_Shot_Right,
                    BulletUser.PlayerBullet, BulletColor.RandomColor);
                _bulletFactory.Create(meta, moveParameter, transform.position);
            }
        }

        public void Bomb()
        {
            var meta = new BulletMeta(BulletType.BigWave, BulletUser.PlayerBullet, BulletColor.RandomColor);
            _bulletFactory.Create(meta, new MoveParameter(), transform.position);            
        }

        public void Damage(int damage, int breakPoint)
        {
            Damaged?.Invoke(damage);
        }

        public void Dead()
        {
            gameObject.SetActive(false);
            var dead = Instantiate(_playerDeadAnimation, transform.position, Quaternion.identity);
            Destroy(dead, 1F);
        }

        public void ReSpawn()
        {
            gameObject.SetActive(true);
        }

        public void ShowCollision()
        {
            _collision.enabled = true;
        }

        public void HideCollision()
        {
            _collision.enabled = false;
        }

        private TohoResources.Direction GetDirectionByEuler(int euler)
        {
            switch (euler)
            {
                case 0:
                    return Direction.Up;
                case 45:
                    return Direction.UpLeft;
                case 90:
                    return Direction.Left;
                case 135:
                    return Direction.DownLeft;
                case 180:
                    return Direction.Down;
                case 225:
                    return Direction.DownRight;
                case 270:
                    return Direction.Right;
                case 315:
                    return Direction.UpRight;
                default:
                    return Direction.Down;
            }
        }
    }


    [Serializable]
    public class PlayerParameter
    {
        public float speed;
        public float slowSpeed;
        public int normalShotPower;
        public float normalShotSpeed;
        public int shotCount;
    }
}