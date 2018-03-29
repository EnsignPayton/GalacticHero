using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BasicEnemy : Entity
    {
        public Sprite BlinkSprite;
        public float ScreenBoundOffset;

        protected SpriteRenderer SpriteRenderer;
        protected Sprite NormalSprite;
        protected bool IsReady;
        protected Vector3 Velocity;

        #region Script Overrides

        /// <summary>
        /// Initialize components and choose a random starting velocity.
        /// </summary>
        protected override void Start()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            NormalSprite = SpriteRenderer.sprite;
            IsReady = false;
            Velocity = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * MoveSpeed;

            base.Start();

            StartCoroutine(BlinkCoroutine());
        }

        /// <summary>
        /// Sets velocity according to the basic movement AI found in <see cref="SetVelocity"/>,
        /// calls the base update, and then applies the velocity to the position using <see cref="ApplyVelocity"/>.
        /// Only runs movement AI if <see cref="IsReady"/> is true.
        /// </summary>
        protected override void Update()
        {
            if (IsReady)
                SetVelocity();

            base.Update();

            if (IsReady)
                ApplyVelocity();
        }

        #endregion

        /// <summary>
        /// On Start, pauses and blinks before taking action.
        /// </summary>
        /// <returns>Coroutine enumerator</returns>
        protected virtual IEnumerator BlinkCoroutine()
        {
            yield return new WaitForSeconds(1.0f);

            // Set blink texture
            SpriteRenderer.sprite = BlinkSprite;

            yield return new WaitForSeconds(1.0f);

            SpriteRenderer.sprite = NormalSprite;
            IsReady = true;

            yield return null;
        }

        /// <summary>
        /// Sets the velocity according to basic movement AI.
        /// </summary>
        protected virtual void SetVelocity()
        {
            var position = Camera.main.WorldToViewportPoint(transform.position);

            if (position.x <= ScreenBoundOffset || position.x >= 1.0f - ScreenBoundOffset)
            {
                Velocity.x = -Velocity.x;
            }

            if (position.y <= ScreenBoundOffset || position.y >= 1.0f - ScreenBoundOffset)
            {
                Velocity.y = -Velocity.y;
            }
        }

        /// <summary>
        /// Applies a velocity to the position, scaled to time.
        /// </summary>
        protected virtual void ApplyVelocity()
        {
            transform.position += Velocity * Time.deltaTime;
        }
    }
}
