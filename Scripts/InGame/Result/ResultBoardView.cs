using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TGJ2021.InGame.Result
{
    public class ResultBoardView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;

        [SerializeField] private TMP_InputField _nameField;

        [SerializeField] private Button _sendButton;

        [SerializeField] private GameObject _sendScoreRoot;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public void UpdateScore(int score)
        {
            _scoreText.text = score.ToString();
        }

        public void ShowSendRankingContents(string currentUserName)
        {
            _sendScoreRoot.SetActive(true);
            _nameField.text = currentUserName;
        }

        public async UniTask<(string name, bool isSendScore)> TapSendButtonAsync()
        {
            var pad = Rewired.ReInput.players.GetPlayer(0);
            var result = await UniTask.WhenAny(_sendButton.OnClickAsync(),
                UniTask.WaitUntil(() => pad.GetButtonDown("Pause")));

            return result == 0 ? (_nameField.text, true) : (string.Empty, false);
        }
    }
}