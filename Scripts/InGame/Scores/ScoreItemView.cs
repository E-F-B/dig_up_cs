using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Otoshiai.Utility;
using UnityEngine;

namespace TGJ2021.InGame.Scores
{
    public class ScoreItemView : MonoBehaviour
    {
        [SerializeField] 
        private float _traceSpeed;

        [SerializeField] 
        private AudioSource _audioSource;
        
        public async UniTask TweenToTarget(Transform target)
        {
            var token = this.GetCancellationTokenOnDestroy();
            await transform.DOLocalMove(Vector3.up * 2, 0.5F).SetRelative().WithCancellation(token);
            await TraceToTarget(target, token);
        }

        private async UniTask TraceToTarget(Transform target, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var direction = BulletMath.EulerAngle(transform.position, target.position);
                transform.position += BulletMath.Forward(direction) * _traceSpeed * Time.deltaTime;

                if (Vector3.Distance(transform.position, target.position) < 1)
                {
                    _audioSource.Play();
                    Destroy(gameObject, 2F);
                    GetComponent<SpriteRenderer>().enabled = false;
                    return;
                }

                await UniTask.Yield();
            }
        }
    }
}