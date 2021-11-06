using System;
using Cysharp.Threading.Tasks;
using MessagePipe;
using TGJ2021.InGame.Messages;
using TGJ2021.InGame.Players;
using UnityEngine;
using VContainer.Unity;

namespace TGJ2021.InGame.Scores
{
    public class ScoreItemSpawner : IInitializable
    {
        private readonly Func<Vector3, ScoreItemView> _scoreItemFactory;
        private readonly ISubscriber<RockBreakMessage> _rockBreakSubscriber;
        private readonly Player _player;

        public ScoreItemSpawner(Func<Vector3, ScoreItemView> scoreItemFactory, ISubscriber<RockBreakMessage> rockBreakSubscriber, Player player)
        {
            _scoreItemFactory = scoreItemFactory;
            _rockBreakSubscriber = rockBreakSubscriber;
            _player = player;
        }

        public void Initialize()
        {
            _rockBreakSubscriber.Subscribe(message =>
            {
                if (message.ScorePoint <= 0)
                {
                    return;
                }
                
                var item = _scoreItemFactory.Invoke(message.BreakPosition);
                item.TweenToTarget(_player.Transform).Forget();
            });
        }
    }
}