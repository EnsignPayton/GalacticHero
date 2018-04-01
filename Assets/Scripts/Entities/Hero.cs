using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Hero : Entity
    {
        #region Fields

        /// <summary>
        /// Maximum number of shots allowed on screen
        /// </summary>
        public int MaximumShots = 5;

        /// <summary>
        /// Time to pass between flame generation
        /// </summary>
        public float FlameCooldown = 1.0f;

        /// <summary>
        /// Shot object prefab
        /// </summary>
        public GameObject ShotPrefab;

        /// <summary>
        /// Flame object prefab
        /// </summary>
        public GameObject FlamePrefab;

        /// <summary>
        /// Audio clip to play when shooting
        /// </summary>
        public AudioClip ShootClip;

        private Collider2D _collider;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private AudioSource _audioSource;

        private IList<Shot> _shots = new List<Shot>();
        private bool _flameReady = true;

        #endregion

        #region Script Overrides

        protected override void Start()
        {
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

            if (velocity != Vector2.zero && _flameReady)
            {
                var flame = Instantiate(FlamePrefab);
                flame.transform.position = transform.position;
                StartCoroutine(FlameDelay());
            }

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

        #endregion

        #region Methods

        private void Shoot()
        {
            bool isLeft;

            if (Input.GetButtonDown("Fire1"))
                isLeft = true;
            else if (Input.GetButtonDown("Fire2"))
                isLeft = false;
            else
                return;

            _spriteRenderer.flipX = isLeft;

            _shots = _shots.Where(x => x != null).ToList();

            if (_shots.Count < MaximumShots)
            {
                var shotPrefab = Instantiate(ShotPrefab);
                var shotCollider = shotPrefab.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(shotCollider, _collider);
                var shot = shotPrefab.GetComponent<Shot>();
                shot.Source = this;
                shot.Direction = isLeft ? new Vector2(-1, 0) : new Vector2(1, 0);
                shot.transform.position = transform.position;

                _shots.Add(shot);

                _audioSource.PlayOneShot(ShootClip);
            }
        }

        private IEnumerator FlameDelay()
        {
            _flameReady = false;

            yield return new WaitForSeconds(FlameCooldown);

            _flameReady = true;

            yield return null;
        }

        #endregion
    }
}
