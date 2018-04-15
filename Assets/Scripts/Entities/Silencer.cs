using System.Collections;
using Assets.Scripts.Entities;
using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    public class Silencer : BasicEnemy
    {
        /// <summary>
        /// Shot object prafab
        /// </summary>
        public GameObject ShotPrefab;

        private Vector3 _sourcePosition;
        private bool _collidedFlag;

        protected override void Awake()
        {
            _sourcePosition = transform.position;

            base.Awake();
        }

        protected override IEnumerator BlinkCoroutine()
        {
            // Don't use blink sprite
            //return base.BlinkCoroutine();

            yield return new WaitForSeconds(1.0f);

            IsReady = true;

            // TODO: Run AI loop
            Debug.Log("AI Logic Goes Here");
        }

        private IEnumerator InitializeCoroutine()
        {
            yield return new WaitForSeconds(1.0f);

            while (true)
            {
                // 1. Pick a direction
                Velocity = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * MoveSpeed;

                // 2. With a short delay, spawn enemies
                yield return new WaitForSeconds(0.25f);
                //Debug.Log("Enemy 1 Spawned");
                yield return new WaitForSeconds(0.25f);
                //Debug.Log("Enemy 2 Spawned");

                // 3. Wait for wall collision
                while (!_collidedFlag)
                {
                    yield return null;
                }

                _collidedFlag = false;

                // 4. With a short delay, shoot
                yield return new WaitForSeconds(0.25f);
                //Debug.Log("Shot 1 Spawned");
                yield return new WaitForSeconds(0.25f);
                //Debug.Log("Shot 2 Spawned");

                // 5. Wait for another wall collision
                while (!_collidedFlag)
                {
                    yield return null;
                }

                _collidedFlag = false;

                // 6. Go toward middle
                Velocity = (transform.position - _sourcePosition).normalized * MoveSpeed;
                yield return new WaitForSeconds(1.0f);

                // 7. Stop and shoot
                Velocity = Vector2.zero;
                Debug.Log("Quad Shot Fired!");
                yield return new WaitForSeconds(4.0f);
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
    }
}
