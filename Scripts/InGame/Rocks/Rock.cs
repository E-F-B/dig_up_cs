using Cysharp.Threading.Tasks;
using MessagePipe;
using TGJ2021.InGame.Messages;
using TGJ2021.InGame.Players;
using TGJ2021.InGame.ShotStrategy;

namespace TGJ2021.InGame.Rocks
{
    public enum RockSize
    {
        Small,
        Middle,
        Large
    }

    public enum RockType
    {
        /// <summary>
        /// 拡散弾
        /// </summary>
        Spread = 0,

        /// <summary>
        /// 狙い撃ち
        /// </summary>
        Shoot,
        /// <summary>
        /// 龍珠がザクザク
        /// </summary>
        Rare
    }

    public readonly struct RockMeta
    {
        public readonly RockSize Size;
        public readonly RockType Type;

        public RockMeta(RockSize size, RockType type)
        {
            Size = size;
            Type = type;
        }
    }
    
    public interface IRock
    {
        
    }
    
    
    public class Rock : IRock
    {
        private RockBehaviour _rockBehaviour;
        private readonly Player _player;
        private readonly IPublisher<RockBreakMessage> _rockBreakPublisher;
        private readonly IPublisher<AddScoreMessage> _scorePublisher;
        private readonly IDanmaku _danmaku;
        private readonly int _score;
        private readonly RockMeta _rockMeta;

        private int _life;
        private readonly int _max;
        private int _breakPoint;

        private float LifeRate => 1F - (float)_life / (float)_max;

        public Rock(RockBehaviour behaviour, Player player, IPublisher<RockBreakMessage> rockBreakPublisher, IPublisher<AddScoreMessage> scorePublisher, IDanmaku danmaku, int life, int score, RockMeta rockMeta)
        {
            _rockBehaviour = behaviour;
            _player = player;
            _rockBehaviour.Damaged += OnDamaged;
            _rockBreakPublisher = rockBreakPublisher;
            _scorePublisher = scorePublisher;
            _danmaku = danmaku;
            _life = life;
            _max = life;
            _score = score;
            _rockMeta = rockMeta;
        }

        private void OnDamaged(int damage, int breakPoint)
        {
            _life -= damage;
            _breakPoint += breakPoint;

            if (_life <= 0)
            {
                _rockBehaviour.Damaged -= OnDamaged;
                _rockBreakPublisher.Publish(new RockBreakMessage
                    { 
                        BreakPosition = _rockBehaviour.transform.position,
                        Meta = _rockMeta,
                        ScorePoint = _score
                    });

                _danmaku.Emit(_rockBehaviour.transform.position, _player.Position, _max > breakPoint);
                _scorePublisher.Publish(new AddScoreMessage
                {
                    Point = _score
                });
                
                BreakAsync().Forget();
            }
            else
            {
                _rockBehaviour.UpdateRockTexture(LifeRate);
            }
        }

        private async UniTask BreakAsync()
        {
            _rockBehaviour.ShowBreakEffect();
            var token = _rockBehaviour.GetCancellationTokenOnDestroy();
            await UniTask.Delay(1000, cancellationToken: token);
            if (!token.IsCancellationRequested)
            {
                _rockBehaviour.Dispose();
            }
        }
    }
}