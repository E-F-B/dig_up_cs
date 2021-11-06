using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace TGJ2021
{
    public class FadeManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _fadeCanvas;
        
        public async UniTask FadeIn(float duration, Ease ease)
        {
            await DOTween.To(() => 0F,
                    value => _fadeCanvas.alpha = value,
                    1F,
                    duration)
                .SetEase(ease);
        }

        public async UniTask FadeOut(float duration, Ease ease)
        {
            await DOTween.To(() => 1F,
                    value => _fadeCanvas.alpha = value,
                    0F,
                    duration)
                .SetEase(ease);
        }
    }    
}

