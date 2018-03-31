using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BasicEnemy : Entity
    {
        #region Fields

        /// <summary>
        /// Sprite to use when running a blink on initialization.
        /// </summary>
        public Sprite BlinkSprite;

        /// <summary>
        /// Percent distance from edge of the screen to turn around.
        /// </summary>
        public float ScreenBoundOffset;

        /// <summary>
        /// SpriteRenderer component
        /// </summary>
        protected SpriteRenderer SpriteRenderer;

        /// <summary>
        /// Default sprite
        /// </summary>
        protected Sprite NormalSprite;

        /// <summary>
        /// Ready to move and attack
        /// </summary>
        protected bool IsReady;

        /// <summary>
        /// Velocity used for movement AI
        /// </summary>
        protected Vector3 Velocity;

        #endregion

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

        #region Virtual Methods

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

            if ((position.x <= ScreenBoundOffset && Velocity.x < 0.0f) ||
                (position.x >= 1.0f - ScreenBoundOffset && Velocity.x > 0.0f) ||
                (WallDirection & Direction.Left) == Direction.Left ||
                (WallDirection & Direction.Right) == Direction.Right)
            {
                Velocity.x = -Velocity.x;
            }

            if ((position.y <= ScreenBoundOffset && Velocity.y < 0.0f) ||
                (position.y >= 1.0f - ScreenBoundOffset && Velocity.y > 0.0f) ||
                (WallDirection & Direction.Top) == Direction.Top ||
                (WallDirection & Direction.Bottom) == Direction.Bottom)
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

        #endregion
    }
}
