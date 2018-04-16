using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Shot : Script
    {
        #region Fields

        /// <summary>
        /// Horizontal speed
        /// </summary>
        public float Speed = 1.0f;

        /// <summary>
        /// Source entity saved to prevent friendly fire
        /// </summary>
        public Script Source { get; set; }

        /// <summary>
        /// Projectile direction
        /// </summary>
        public Vector2 Direction { get; set; }

        private Rigidbody2D _rigidbody;

        #endregion

        #region Script Overrides

        /// <summary>
        /// Initialize
        /// </summary>
        protected override void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            base.Start();
        }

        protected override void FixedUpdate()
        {
            _rigidbody.position += Direction * Speed * Time.deltaTime;

            base.FixedUpdate();
        }

        /// <summary>
        /// Destroy the shot when it goes offscreen
        /// </summary>
        protected override void OnBecameInvisible()
        {
            Dispose();

            base.OnBecameInvisible();
        }

        /// <summary>
        /// Destroy the shot when it collides with a solid object (ex. a wall)
        /// </summary>
        /// <param name="collision">Collision</param>
        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            // Destroy when shot collides with a solid object
            Dispose();

            base.OnCollisionEnter2D(collision);
        }

        #endregion
    }
}
