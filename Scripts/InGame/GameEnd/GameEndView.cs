using Cysharp.Threading.Tasks;
using MessagePipe;
using TGJ2021.InGame.Messages;
using TMPro;
using UnityEngine;
using VContainer;

namespace TGJ2021
{
    public class GameEndView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _finishText;

        [Inject]
        public void Construct(IAsyncSubscriber<GameEndMessage> gameEndSubscriber)
        {
            gameEndSubscriber.Subscribe((async (message, token) =>
            {
                _finishText.gameObject.SetActive(true);
                await UniTask.Delay(2000, cancellationToken: token);
            }));
        }
    }
}
