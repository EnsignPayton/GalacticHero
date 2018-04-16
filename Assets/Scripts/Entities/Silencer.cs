using System.Collections;
using System.Collections.Generic;
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
        /// Big Explosion prefab
        /// </summary>
        public GameObject BigExplosionPrefab;

        /// <summary>
        /// Shot bounce off sound clip
        /// </summary>
        public AudioClip PingClip;

        /// <summary>
        /// Explosion sound clips. Should always contain 3 clips.
        /// </summary>
        public AudioClip[] ExplosionClips;

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

            while (IsReady)
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

        protected override IEnumerator Die()
        {
            // Stop interaction
            IsReady = false;
            StopCoroutine(Blink);

            // Disable children
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            // Ignore hero
            var hero = FindObjectOfType<Hero>();
            var heroCollider = hero.GetComponent<Collider2D>();
            IgnoreAllColliders(heroCollider);

            // Sink to floor
            Rigidbody.velocity = Vector2.zero;
            Rigidbody.AddForce(new Vector2(0, -0.8f), ForceMode2D.Impulse);

            // Scream
            AudioSource.PlayOneShot(DeathClip);

            for (int i = 0; i < 10; i++)
            {
                AudioSource.PlayOneShot(ExplosionClips[0]);
                var explosion = Instantiate(BigExplosionPrefab);
                var bounds = Renderer.bounds.size / 2.0f;
                explosion.transform.position = transform.position +
                    new Vector3(Random.Range(-bounds.x, bounds.x), Random.Range(-bounds.y, bounds.y));

                yield return new WaitForSeconds(0.2f);

                Destroy(explosion);
            }

            AudioSource.PlayOneShot(ExplosionClips[1]);
            var explosions = new List<GameObject>();
            for (int i = 0; i < 20; i++)
            {
                // Create explosions that move upward
                var explosion = Instantiate(BigExplosionPrefab);
                var bounds = Renderer.bounds.size / 2.0f;
                explosion.transform.position = transform.position +
                    new Vector3(Random.Range(-bounds.x, bounds.x), Random.Range(-bounds.y, bounds.y));
                var explosionRigidbody = explosion.AddComponent<Rigidbody2D>();
                explosionRigidbody.AddForce(new Vector2(0, 12));
                explosions.Add(explosion);
            }

            yield return new WaitForSeconds(0.9f);
            foreach (var explosion in explosions)
            {
                Destroy(explosion);
            }
            explosions.Clear();

            AudioSource.PlayOneShot(ExplosionClips[2]);

            // Create and rotate 4 jets
            for (int i = 0; i < 32; i++)
            {
                var angle = i % 4 * Mathf.PI / 2 + Mathf.PI / 4;
                var direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
                var explosion = Instantiate(BigExplosionPrefab);
                explosion.transform.position = transform.position;

                var explosionRigidbody = explosion.AddComponent<Rigidbody2D>();
                explosionRigidbody.AddForce(direction * 2.0f, ForceMode2D.Impulse);

                explosions.Add(explosion);
                yield return null;
            }

            foreach (var explosion in explosions)
            {
                Destroy(explosion);
                yield return null;
            }
            explosions.Clear();

            gameObject.SetActive(false);
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
                IgnoreAllColliders(shotCollider);
            }
        }

        private void IgnoreAllColliders(Collider2D otherCollider)
        {
            Physics2D.IgnoreCollision(otherCollider, Collider2D);

            foreach (Transform child in transform)
            {
                var childCollider = child.GetComponent<Collider2D>();
                if (childCollider != null)
                {
                    Physics2D.IgnoreCollision(otherCollider, childCollider);
                }
            }
        }

        #endregion
    }
}
