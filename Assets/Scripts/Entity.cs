using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class Entity : Script
    {
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
        /// Current health total
        /// </summary>
        public int Health { get; set; }

        protected AudioSource AudioSource { get; set; }

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
