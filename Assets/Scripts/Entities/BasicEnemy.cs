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
        /// SpriteRenderer component
        /// </summary>
        protected SpriteRenderer SpriteRenderer;

        /// <summary>
        /// Default sprite
        /// </summary>
        protected Sprite NormalSprite;

        /// <summary>
        /// Velocity used for movement AI
        /// </summary>
        protected Vector2 Velocity;

        /// <summary>
        /// Rigidbody2D component
        /// </summary>
        protected Rigidbody2D Rigidbody;

        /// <summary>
        /// Blick Coroutine
        /// </summary>
        protected Coroutine Blink;

        #endregion

        #region Script Overrides

        protected override void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            NormalSprite = SpriteRenderer.sprite;

            base.Awake();
        }

        protected override void OnEnable()
        {
            IsReady = false;
            SetRandomVelocity();

            base.OnEnable();

            Blink = StartCoroutine(BlinkCoroutine());
        }

        /// <summary>
        /// Applies the set velocity if <see cref="Entity.IsReady"/> is true.
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
            // Bounce off things that are not a shot
            var shot = collision.collider.GetComponent<Shot>();
            if (shot == null)
            {
                var normal = collision.contacts[0].normal;

                Velocity += -2.0f * Vector2.Dot(Velocity, normal) * normal;
            }

            base.OnCollisionEnter2D(collision);
        }

        #endregion

        #region Protected Methods

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

        protected void SetVelocity(Vector2 velocity)
        {
            Velocity = velocity.normalized * MoveSpeed;
        }

        protected void SetVelocity(float xSpeed, float ySpeed)
        {
            SetVelocity(new Vector2(xSpeed, ySpeed));
        }

        protected void SetRandomVelocity()
        {
            SetVelocity(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        }

        #endregion
    }
}
