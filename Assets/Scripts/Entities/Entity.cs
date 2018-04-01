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
        /// Wall collision direction
        /// </summary>
        protected Direction WallDirection = Direction.None;

        #endregion

        /// <summary>
        /// Current health total
        /// </summary>
        public int Health { get; set; }

        #region Script Overrides

        /// <summary>
        /// Set health to max on start
        /// </summary>
        protected override void Start()
        {
            Health = MaxHealth;
            AudioSource = GetComponent<AudioSource>();
            base.Start();
        }

        /// <summary>
        /// Die when health reaches zero
        /// </summary>
        protected override void Update()
        {
            if (Health <= 0) Dispose();
            base.Update();
        }

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
        /// Reacts to being hit
        /// </summary>
        /// <param name="triggerCollider">Collision Object</param>
        protected override void OnTriggerEnter2D(Collider2D triggerCollider)
        {
            var shot = triggerCollider.GetComponent<Shot>();
            if (shot != null && shot.Source != null && shot.Source != this)
            {
                Health--;
                shot.Dispose();
            }

            base.OnTriggerEnter2D(triggerCollider);
        }

        /// <summary>
        /// Play death sound and delay disposal until its done
        /// </summary>
        /// <param name="disposing"></param>
        /// <param name="delay"></param>
        protected override void Dispose(bool disposing, float delay = 0)
        {
            if (IsDisposed) return;

            if (DeathClip != null)
            {
                AudioSource.PlayOneShot(DeathClip);
                delay = DeathClip.length;
                var render = GetComponent<Renderer>();
                render.enabled = false;
            }

            base.Dispose(disposing, delay);
        }

        #endregion
    }
}
