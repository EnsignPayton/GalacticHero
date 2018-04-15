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
                // TODO: Create Shots
                yield return new WaitForSeconds(4.0f);

                Debug.Log("Loop Finished");
            }
        }

        protected override void ReactToHit(Shot shot, Collision2D collision)
        {
            var reflectPrefab = Instantiate(ShotPrefab);
            var reflectCollider = reflectPrefab.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(reflectCollider, Collider2D);

            var reflectShot = reflectPrefab.GetComponent<Shot>();
            reflectShot.Source = this;
            reflectShot.transform.position = collision.transform.position;

            bool isUp = collision.transform.position.y > transform.position.y;
            reflectShot.Direction = isUp ? new Vector2(0, 1) : new Vector2(0, -1);

            // Don't take damage
            Health++;
            base.ReactToHit(shot, collision);
        }

        #endregion

        private void SpawnRandomEnemy()
        {
            var enemyPrefab = Instantiate(EnemyPrefabs.RandomElement());
            var enemyCollider = enemyPrefab.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(enemyCollider, Collider2D);

            foreach (Transform child in transform)
            {
                var childCollider = child.GetComponent<Collider2D>();

                if (childCollider != null)
                {
                    Physics2D.IgnoreCollision(enemyCollider, childCollider);
                }
            }

            enemyPrefab.transform.position = transform.position;

            Debug.Log($"Spawned Enemy {enemyPrefab.gameObject.name}");
        }
    }
}
