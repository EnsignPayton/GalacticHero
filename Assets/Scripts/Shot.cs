using UnityEngine;

namespace Assets.Scripts
{
    public class Shot : Script
    {
        /// <summary>
        /// Horizontal speed
        /// </summary>
        public float Speed = 1.0f;

        /// <summary>
        /// Source entity saved to prevent friendly fire
        /// </summary>
        public Entity Source { get; set; }

        /// <summary>
        /// Left or right flag, will be removed when we suppport full 2D
        /// </summary>
        public bool IsLeft { get; set; }

        #region Script Overrides

        /// <summary>
        /// Move the shot
        /// </summary>
        protected override void Update()
        {
            var velocity = new Vector3();
            velocity.x = IsLeft ? -Speed : Speed;
            transform.position += velocity * Time.deltaTime;

            base.Update();
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

        /// <summary>
        /// Destroy the shot when it collides with a wall (in the case we define walls as triggers)
        /// </summary>
        /// <param name="triggerCollider">Trigger Collider</param>
        protected override void OnTriggerEnter2D(Collider2D triggerCollider)
        {
            if (triggerCollider.GetComponent<Block>())
            {
                Dispose();
            }

            base.OnTriggerEnter2D(triggerCollider);
        }

        #endregion
    }
}
