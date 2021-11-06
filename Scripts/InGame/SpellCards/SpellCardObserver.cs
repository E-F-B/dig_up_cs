using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using Rewired;
using TGJ2021.InGame.Messages;
using UnityEngine;
using VContainer.Unity;

namespace TGJ2021.InGame.SpellCards
{
    public class SpellCardObserver : IAsyncStartable
    {
        private readonly MineBlast _mineBlast;
        private readonly IAsyncSubscriber<GameEndMessage> _gameEndAsyncSubscriber;
        private readonly SpellCardUI _spellCardUi;

        public SpellCardObserver(MineBlast mineBlast, IAsyncSubscriber<GameEndMessage> gameEndAsyncSubscriber, SpellCardUI spellCard)
        {
            _mineBlast = mineBlast;
            _gameEndAsyncSubscriber = gameEndAsyncSubscriber;
            _spellCardUi = spellCard;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            var pad = ReInput.players.GetPlayer(0);
            var pushBombKeyTask = UniTask.WaitUntil(() => pad.GetButtonDown("Spell"), cancellationToken: cancellation);
            var gameEndTask = _gameEndAsyncSubscriber.FirstAsync(cancellation);

            var result = await UniTask.WhenAny(pushBombKeyTask, gameEndTask); 
            if (cancellation.IsCancellationRequested)
            {
                return;
            }

            // ボムキー押してたらスペカ
            if (result == 0)
            {
                _spellCardUi.HideSpellCard();
                await _mineBlast.DoSpellSequence();                
            }
        }
    }
}