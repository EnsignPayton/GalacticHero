using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Drone : Entity
    {
        public Sprite BlinkSprite;

        private SpriteRenderer _spriteRenderer;
        private Sprite _normalSprite;
        private bool _isReady;
        private Vector3 _velocity;

        #region Script Overrides

        protected override void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _normalSprite = _spriteRenderer.sprite;
            _isReady = false;
            Health = MaxHealth;

            base.Start();

            StartCoroutine(Blink());
        }

        protected override void Update()
        {

            if (!_isReady) return;

            // TODO: Movement AI

            base.Update();
        }

        #endregion

        // On start, pauses and blinks before acting
        private IEnumerator Blink()
        {
            yield return new WaitForSeconds(1.0f);

            // Set blink texture
            _spriteRenderer.sprite = BlinkSprite;

            yield return new WaitForSeconds(1.0f);

            _spriteRenderer.sprite = _normalSprite;
            _isReady = true;

            yield return null;
        }
    }
}
