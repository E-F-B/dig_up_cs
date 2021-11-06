using TMPro;
using UnityEngine;

namespace TGJ2021.InGame.Timers
{
    public class GameTimerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timeText;

        [SerializeField] private AudioSource _gameEnd;

        [SerializeField] private AudioSource _count;
        public void UpdateTime(int time)
        {
            _timeText.text = time.ToString();
        }

        public void Finish()
        {
            _timeText.text = "そこまで！";
            _gameEnd.Play();
        }

        public void CallCountDown()
        {
            _count.Play();
        }
    }
}