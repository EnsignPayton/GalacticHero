using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Drone : Entity
    {
        public Sprite BlinkSprite;
        public float ScreenBoundOffset;

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
            _velocity = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * MoveSpeed;

            base.Start();

            StartCoroutine(Blink());
        }

        protected override void Update()
        {
            if (_isReady)
            {
                var position = Camera.main.WorldToViewportPoint(transform.position);

                if (position.x <= ScreenBoundOffset || position.x >= 1.0f - ScreenBoundOffset)
                {
                    _velocity.x = -_velocity.x;
                }

                if (position.y <= ScreenBoundOffset || position.y >= 1.0f - ScreenBoundOffset)
                {
                    _velocity.y = -_velocity.y;
                }

                transform.position += _velocity * Time.deltaTime;
            }

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
