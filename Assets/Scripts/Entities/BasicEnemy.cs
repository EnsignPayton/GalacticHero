using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
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
        protected Vector2 Velocity;

        /// <summary>
        /// Rigidbody2D component
        /// </summary>
        protected Rigidbody2D Rigidbody;

        #endregion

        #region Script Overrides

        /// <summary>
        /// Initialize components and choose a random starting velocity.
        /// </summary>
        protected override void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            NormalSprite = SpriteRenderer.sprite;
            IsReady = false;
            Velocity = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * MoveSpeed;

            base.Start();

            StartCoroutine(BlinkCoroutine());
        }

        /// <summary>
        /// Sets velocity according to the basic movement AI found in <see cref="SetVelocity"/> if <see cref="IsReady"/> is true.
        /// </summary>
        protected override void Update()
        {
            if (IsReady)
            {
                SetVelocity();
            }

            base.Update();
        }

        /// <summary>
        /// Applies the set velocity if <see cref="IsReady"/> is true.
        /// </summary>
        protected override void FixedUpdate()
        {
            if (IsReady)
            {
                Rigidbody.position += Velocity * Time.deltaTime;
            }

            base.FixedUpdate();
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            // TODO: Bounce

            base.OnCollisionEnter2D(collision);
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
                (position.x >= 1.0f - ScreenBoundOffset && Velocity.x > 0.0f))
            {
                Velocity.x = -Velocity.x;
            }

            if ((position.y <= ScreenBoundOffset && Velocity.y < 0.0f) ||
                (position.y >= 1.0f - ScreenBoundOffset && Velocity.y > 0.0f))
            {
                Velocity.y = -Velocity.y;
            }
        }

        #endregion
    }
}
