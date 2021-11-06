using Cysharp.Threading.Tasks;
using MessagePipe;
using MoreMountains.Feedbacks;
using TGJ2021.InGame.Messages;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace TGJ2021.InGame.Players
{
    public class UshirodoUI : MonoBehaviour
    {
        [SerializeField] private Sprite _normalOkina;
        [SerializeField] private Sprite _sadOkina;
        [SerializeField] private Sprite _smileOkina;

        [SerializeField] private Image _face;

        [SerializeField] private MMFeedbacks _hitFeedbacks;
        [SerializeField] private MMFeedbacks _playerDeadFeedBacks;
        
        
        [Inject]
        public void Construct(ISubscriber<UshirodoHitMessage> hitSubscriber, ISubscriber<PlayerDeadMessage> playerDeadSubscriber,ISubscriber<PlayerReSpawnMessage> playerReSpawn)
        {
            hitSubscriber.Subscribe(OnHitUshirodo);
            playerDeadSubscriber.Subscribe(OnPlayerDead);
            playerReSpawn.Subscribe((message => _face.sprite = _normalOkina));
        }

        public void OnHitUshirodo(UshirodoHitMessage message)
        {
            // 笑顔のときは何も起きない
            if (_face.sprite.Equals(_smileOkina))
            {
                return;
            }
            
            _hitFeedbacks.PlayFeedbacks();
            HitEffect().Forget();
        }

        private void OnPlayerDead(PlayerDeadMessage message)
        {
            _face.sprite = _smileOkina;
            _playerDeadFeedBacks.PlayFeedbacks();
        }

        private async UniTask HitEffect()
        {
            _face.sprite = _sadOkina;
            await UniTask.Delay(300);
            // 笑顔のときは何も起きない
            if (_face.sprite.Equals(_smileOkina))
            {
                return;
            }
            _face.sprite = _normalOkina;
        }
    }
}