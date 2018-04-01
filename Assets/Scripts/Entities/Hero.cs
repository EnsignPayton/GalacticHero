using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Hero : Entity
    {
        /// <summary>
        /// Maximum number of shots allowed on screen
        /// </summary>
        public int MaximumShots = 5;

        /// <summary>
        /// Shot object prefab
        /// </summary>
        public GameObject ShotPrefab;

        public AudioClip ShootClip;

        private Collider2D _collider;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private AudioSource _audioSource;
        private IList<Shot> _shots;

        #region Script Overrides

        protected override void Start()
        {
            if (ShotPrefab == null)
                Debug.LogError("Shot prefab undefined.");

            _shots = new List<Shot>();
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _audioSource = GetComponent<AudioSource>();

            base.Start();
        }

        protected override void Update()
        {
            Shoot();

            base.Update();
        }

        protected override void FixedUpdate()
        {
            var velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            _rigidbody.position += velocity * MoveSpeed * Time.deltaTime;

            base.FixedUpdate();
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            var enemy = collision.collider.GetComponent<BasicEnemy>();
            if (enemy != null)
            {
                Health--;
            }

            base.OnCollisionEnter2D(collision);
        }

        protected override void OnTriggerEnter2D(Collider2D triggerCollider)
        {
            var enemy = triggerCollider.GetComponent<BasicEnemy>();
            if (enemy != null)
            {
                Health--;
            }

            base.OnTriggerEnter2D(triggerCollider);
        }

        #endregion

        #region Methods

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
                var shotCollider = shotPrefab.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(shotCollider, _collider);
                var shot = shotPrefab.GetComponent<Shot>();
                shot.Source = this;
                shot.IsLeft = isLeft;
                shot.transform.position = transform.position;

                _shots.Add(shot);

                _audioSource.PlayOneShot(ShootClip);
            }
        }

        #endregion
    }
}
