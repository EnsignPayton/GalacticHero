using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Hero : Entity
    {
        public int MaximumShots = 5;
        public GameObject ShotPrefab = null;

        private SpriteRenderer _spriteRenderer;
        private IList<Shot> _shots;

        #region Script Overrides

        protected override void Start()
        {
            if (ShotPrefab == null)
                Debug.LogError("Shot prefab undefined.");

            _shots = new List<Shot>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            base.Start();
        }

        protected override void Update()
        {
            Move();
            Shoot();

            base.Update();
        }

        protected override void OnTriggerEnter2D(Collider2D triggerCollider)
        {
            var enemy = triggerCollider.GetComponent<Drone>();
            if (enemy != null)
            {
                Health--;
            }

            base.OnTriggerEnter2D(triggerCollider);
        }

        #endregion

        #region Methods

        private void Move()
        {
            var velocity = new Vector3();

            if (Input.GetKey(KeyCode.LeftArrow))
                velocity.x = -MoveSpeed;
            else if (Input.GetKey(KeyCode.RightArrow))
                velocity.x = MoveSpeed;
            else
                velocity.x = 0.0f;

            if (Input.GetKey(KeyCode.UpArrow))
                velocity.y = MoveSpeed;
            else if (Input.GetKey(KeyCode.DownArrow))
                velocity.y = -MoveSpeed;
            else
                velocity.y = 0.0f;

            if (Math.Abs(velocity.x) > 0.001f && Math.Abs(velocity.y) > 0.001f)
            {
                velocity *= (2.0f / 3.0f);
            }

            transform.position += velocity * Time.deltaTime;
        }

        private void Shoot()
        {
            bool isLeft;

            if (Input.GetKeyDown(KeyCode.Z))
                isLeft = true;
            else if (Input.GetKeyDown(KeyCode.X))
                isLeft = false;
            else
                return;

            _spriteRenderer.flipX = isLeft;

            // Clean up old shots
            _shots = _shots.Where(x => x != null).ToList();

            if (_shots.Count < MaximumShots)
            {
                var shotPrefab = Instantiate(ShotPrefab);
                var shot = shotPrefab.GetComponent<Shot>();
                shot.Source = this;
                shot.IsLeft = isLeft;
                shot.transform.position = transform.position;
                _shots.Add(shot);
            }
        }

        #endregion
    }
}
