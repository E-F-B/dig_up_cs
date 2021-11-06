using MessagePipe;
using Otoshiai.Utility;
using TGJ2021.InGame.Messages;
using UnityEngine;
using VContainer;

namespace TGJ2021.InGame.Players
{
    public class UshirodoBehaviour : MonoBehaviour, IDamageable
    {
        private float _fixPosition;

        private IPublisher<UshirodoHitMessage> _publisher;

        [Inject]
        public void Construct(IPublisher<UshirodoHitMessage> publisher)
        {
            _publisher = publisher;
        }

        public void SetUpFixPosition(Transform parent)
        {
            var distance = Vector3.Distance(parent.position, transform.position);
            _fixPosition = distance;
        }
        
        public void UpdatePosition(float directionEuler, Vector3 parentPosition)
        {
            // 後度は180度後にある
            var angle = directionEuler + 180;
            var euler = transform.eulerAngles;
            euler.z = directionEuler;
            transform.eulerAngles = euler;
            
            var direction = BulletMath.Forward(angle);
            transform.position = parentPosition + direction * _fixPosition;
        }

        public void Damage(int damage, int breakPoint)
        {
            _publisher.Publish(new UshirodoHitMessage());
        }
    }
}