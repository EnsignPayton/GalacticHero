using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Collider2D))]
    public class Silencer : Script
    {
        /// <summary>
        /// Shot object prafab
        /// </summary>
        public GameObject ShotPrefab;

        private Collider2D _collider;

        protected override void Awake()
        {
            _collider = GetComponent<Collider2D>();

            base.Awake();
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            var shot = collision.collider.GetComponent<Shot>();

            if (shot?.Source != null && shot.Source is Hero)
            {
                var shotPrefab = Instantiate(ShotPrefab);

                var newCollider = shotPrefab.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(newCollider, _collider);
                Physics2D.IgnoreCollision(newCollider, collision.collider);

                var newShot = shotPrefab.GetComponent<Shot>();
                newShot.Source = this;
                newShot.transform.position = collision.transform.position;

                bool isUp = collision.transform.position.y > transform.position.y;
                newShot.Direction = isUp ? new Vector2(0, 1) : new Vector2(0, -1);
            }

            base.OnCollisionEnter2D(collision);
        }
    }
}
