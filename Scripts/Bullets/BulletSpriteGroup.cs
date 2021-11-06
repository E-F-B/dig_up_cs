using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace TGJ2021
{
    [CreateAssetMenu]
    public class BulletSpriteGroup : ScriptableObject, ISerializationCallbackReceiver
    {
        [TableList(ShowIndexLabels = true)]
        [PreviewField]
        public Sprite[] _sprites;

        public int Count { get; private set; }

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {
            Count = _sprites.Length;
        }

        public Sprite GetRandomSprite() => _sprites[Random.Range(0, Count)];

        public Sprite GetSpriteByIndex(int index)
        {
            Assert.IsTrue(index <= Count);
            return _sprites[index];
        }
    }
}
