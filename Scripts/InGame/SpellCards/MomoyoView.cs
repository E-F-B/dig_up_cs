using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace TGJ2021.InGame.SpellCards
{
    public class MomoyoView : MonoBehaviour
    {
        [SerializeField] private Transform arrivedPoint;
        [SerializeField] private Transform leftPoint;

        public async UniTask FrameIn()
        {
            var audio = GetComponent<AudioSource>();
            audio.Play();
            await transform.DOLocalMove(arrivedPoint.position, 1F);
            await UniTask.Delay(1000);
        }

        public async UniTask FrameOut()
        {
            await transform.DOLocalMove(leftPoint.position, 1F).SetRelative();
        }
    }
}