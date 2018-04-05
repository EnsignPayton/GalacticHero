using System;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class Entity : Script
    {
        #region Fields

        /// <summary>
        /// Maximum health total
        /// </summary>
        public int MaxHealth = 1;

        /// <summary>
        /// Speed to scale movement
        /// </summary>
        public float MoveSpeed = 1.0f;

        /// <summary>
        /// Death sound clip
        /// </summary>
        public AudioClip DeathClip;

        /// <summary>
        /// AudioSource component
        /// </summary>
        protected AudioSource AudioSource;

        /// <summary>
        /// Renderer component
        /// </summary>
        protected Renderer Renderer;

        /// <summary>
        /// Collider2D component
        /// </summary>
        protected Collider2D Collider2D;

        protected Vector3? InitialPosition;

        /// <summary>
        /// Current health total
        /// </summary>
        [NonSerialized]
        public int Health;

        #endregion

        #region Script Overrides

        /// <summary>
        /// Set health to max on startup
        /// </summary>
        protected override void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
            Renderer = GetComponent<Renderer>();
            Collider2D = GetComponent<Collider2D>();

            base.Awake();
        }

        /// <summary>
        /// Die when health reaches zero
        /// </summary>
        protected override void Update()
        {
            if (Health <= 0)
                enabled = false;

            base.Update();
        }

        /// <summary>
        /// React to being hit
        /// </summary>
        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            var shot = collision.collider.GetComponent<Shot>();
            if (shot != null && shot.Source != null && shot.Source != this)
            {
                Health--;
                shot.Dispose();
            }

            base.OnCollisionEnter2D(collision);
        }

        /// <summary>
        /// Enable components and reset health on enable
        /// </summary>
        protected override void OnEnable()
        {
            if (InitialPosition == null)
            {
                InitialPosition = transform.localPosition;
            }

            Renderer.enabled = true;
            Collider2D.enabled = true;
            Health = MaxHealth;

            base.OnEnable();
        }

        /// <summary>
        /// Play death sound on disable
        /// </summary>
        protected override void OnDisable()
        {
            if (DeathClip != null && AudioSource.isActiveAndEnabled)
            {
                AudioSource.PlayOneShot(DeathClip);
                Renderer.enabled = false;
                Collider2D.enabled = false;
            }

            if (InitialPosition != null)
            {
                transform.localPosition = InitialPosition.Value;
            }

            base.OnDisable();
        }

        #endregion
    }
}
