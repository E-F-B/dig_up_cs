using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TGJ2021.InGame.Rocks
{
    public class RockBehaviour : MonoBehaviour, IDamageable
    {
        [SerializeField] 
        private SpriteRenderer _spriteRenderer;

        [SerializeField] 
        private AudioSource _audioSource;

        [SerializeField] 
        private CircleCollider2D _collider2D;

        [SerializeField] 
        private GameObject _rockBreakAnimation;
        
        private static readonly int Life = Shader.PropertyToID("_Life");

        public event Action<int, int> Damaged;

        public void Dispose()
        {
            Destroy(gameObject);
        }

        public void UpdateRockTexture(float rate)
        {
            _spriteRenderer.material.SetFloat(Life, rate);
        }

        public void ShowBreakEffect()
        {
            _spriteRenderer.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("BreakRock");
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            _spriteRenderer.enabled = false;
            _collider2D.enabled = false;
            _audioSource.Play();
            var token = this.GetCancellationTokenOnDestroy();
            _rockBreakAnimation.SetActive(true);
        }

        public void Damage(int damage, int breakPoint)
        {
            Damaged?.Invoke(damage, breakPoint);
        }
    }
}