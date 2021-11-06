using Cysharp.Threading.Tasks;
using Rewired;
using UnityEngine;

namespace TGJ2021.InGame.Result
{

    public class RetryPopupView : MonoBehaviour
    {
        public enum Content
        {
            Retry = 0,
            BackToTitle
        }

        public async UniTask<Content> OpenRetryPopup()
        {
            var pad = ReInput.players.GetPlayer(0);
            gameObject.SetActive(true);
            
            var retryTask = UniTask.WaitUntil(() => pad.GetButtonDown("Action"));
            var titleTask = UniTask.WaitUntil(() => pad.GetButtonDown("Spell"));

            var result = await UniTask.WhenAny(retryTask, titleTask);
            return (Content)result;
        }
    }
}