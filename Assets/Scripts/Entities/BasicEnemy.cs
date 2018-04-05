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
            Velocity = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * MoveSpeed;

            base.OnEnable();

            StartCoroutine(BlinkCoroutine());
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
            var shot = collision.collider.GetComponent<Shot>();
            if (shot == null)
            {
                var normal = collision.contacts[0].normal;

                Velocity += -2.0f * Vector2.Dot(Velocity, normal) * normal;
            }

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

        #endregion
    }
}
