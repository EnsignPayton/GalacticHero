using System.Collections;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Silencer : BasicEnemy
    {
        #region Fields

        /// <summary>
        /// Shot object prefab
        /// </summary>
        public GameObject ShotPrefab;

        /// <summary>
        /// Enemy spawn prefabs
        /// </summary>
        public GameObject[] EnemyPrefabs;

        /// <summary>
        /// Shot bounce off sound clip
        /// </summary>
        public AudioClip PingClip;

        private Vector3 _sourcePosition;
        private bool _collidedFlag;

        #endregion

        #region Overrides

        protected override void Awake()
        {
            _sourcePosition = transform.position;

            base.Awake();
        }

        protected override IEnumerator BlinkCoroutine()
        {
            // Don't use blink sprite
            //return base.BlinkCoroutine();

            yield return new WaitForSeconds(2.0f);

            IsReady = true;

            while (true)
            {
                // 1. Pick a random direction
                SetRandomVelocity();

                // 2. After short delays, spawn two enemies
                yield return new WaitForSeconds(0.5f);
                SpawnRandomEnemy();
                yield return new WaitForSeconds(0.5f);
                SpawnRandomEnemy();

                // 3. Wait for a few bounces
                yield return new WaitForSeconds(5.0f);

                // 4. Move toward center
                while ((_sourcePosition - transform.position).magnitude > 0.4f)
                {
                    SetVelocity(_sourcePosition - transform.position);
                    yield return new WaitForFixedUpdate();
                }

                // 5. Stop and shoot in diagonals
                Velocity = Vector2.zero;
                Shoot(transform.position + new Vector3(-0.16f, -0.16f), new Vector2(-1, -1).normalized, 0.8f, true);
                Shoot(transform.position + new Vector3(-0.16f, 0.16f), new Vector2(-1, 1).normalized, 0.8f, true);
                Shoot(transform.position + new Vector3(0.16f, -0.16f), new Vector2(1, -1).normalized, 0.8f, true);
                Shoot(transform.position + new Vector3(0.16f, 0.16f), new Vector2(1, 1).normalized, 0.8f, true);
                yield return new WaitForSeconds(4.0f);
            }
        }

        protected override void ReactToHit(Shot shot, Collision2D collision)
        {
            bool isUp = collision.transform.position.y > transform.position.y;
            Shoot(collision.transform.position, new Vector2(0, isUp ? 1 : -1), shot.Speed);
            AudioSource.PlayOneShot(PingClip);

            // Don't take damage
            Health++;
            base.ReactToHit(shot, collision);
        }

        #endregion

        #region Private Methods

        private void SpawnRandomEnemy()
        {
            var enemyPrefab = Instantiate(EnemyPrefabs.RandomElement());
            enemyPrefab.transform.parent = transform;
            enemyPrefab.transform.position = transform.position;
        }

        private void Shoot(Vector3? position = null, Vector2? direction = null, float speed = 1.0f, bool ignoreCollision = false)
        {
            var shotPrefab = Instantiate(ShotPrefab);
            var shot = shotPrefab.GetComponent<Shot>();
            shot.Source = this;
            shot.transform.parent = transform;
            shot.transform.position = position ?? transform.position;
            shot.Direction = direction ?? Vector2.zero;
            shot.Speed = speed;

            if (ignoreCollision)
            {
                var shotCollider = shotPrefab.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(shotCollider, Collider2D);

                foreach (Transform child in transform)
                {
                    var childCollider = child.GetComponent<Collider2D>();
                    if (childCollider != null)
                    {
                        Physics2D.IgnoreCollision(shotCollider, childCollider);
                    }
                }
            }
        }

        #endregion
    }
}
