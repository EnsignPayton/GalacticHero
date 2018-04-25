using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utilities;
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
        /// Explosion object prefab
        /// </summary>
        public GameObject BigExplosionPrefab;

        /// <summary>
        /// Audio clip to play when shooting
        /// </summary>
        public AudioClip ShootClip;

        /// <summary>
        /// Another explosion audio clip
        /// </summary>
        public AudioClip ExplosionClip1;

        /// <summary>
        /// Another explosion audio clip
        /// </summary>
        public AudioClip ExplosionClip2;

        /// <summary>
        /// Room manager
        /// </summary>
        public RoomManager RoomManager;

        /// <summary>
        /// Game over UI elements
        /// </summary>
        public GameObject[] GameOverObjects;

        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;

        private IList<Shot> _shots = new List<Shot>();
        private bool _flameReady = true;
        private bool _isGameOver = false;

        #endregion

        #region Properties

        /// <summary>
        /// Room the Hero currently occupies
        /// </summary>
        public Room CurrentRoom => transform.parent.GetComponent<Room>();

        #endregion

        #region Script Overrides

        protected override void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            IsReady = true;

            foreach (var obj in GameOverObjects)
            {
                obj.SetActive(false);
            }

            base.Awake();
        }

        protected override void Update()
        {
            _shots = _shots.Where(x => x != null).ToList();

            if (IsReady)
                Shoot();

            if (_isGameOver)
            {
                var enterPress = Input.GetKey(KeyCode.Return);

                if (enterPress)
                {
                    _isGameOver = false;
                    RoomManager.RestartGame(this);
                }
            }

            base.Update();
        }

        protected override void FixedUpdate()
        {
            if (IsReady)
            {
                var velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

                _rigidbody.position += velocity * MoveSpeed * Time.deltaTime;

                if (velocity != Vector2.zero && _flameReady)
                {
                    var flame = Instantiate(FlamePrefab);
                    flame.transform.position = transform.position;
                    StartCoroutine(FlameDelay());
                }
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

        protected override void OnTriggerEnter2D(Collider2D triggerCollider)
        {
            var room = triggerCollider.GetComponent<Room>();

            if (room != null && CurrentRoom != null && room != CurrentRoom)
            {
                foreach (var shot in _shots)
                {
                    shot.Dispose();
                }

                RoomManager.TransitionRooms(this, room, true);
            }

            base.OnTriggerEnter2D(triggerCollider);
        }

        protected override IEnumerator Die()
        {
            IsReady = false;
            Collider2D.enabled = false;
            AudioSource.PlayOneShot(DeathClip);
            AudioSource.PlayOneShot(ExplosionClip1);
            AudioSource.PlayOneShot(ExplosionClip2);

            // Spawn 12 particles, 3 on each diagonal
            // First inner, then middle, then outer
            var explosions = new List<GameObject>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var explosion = Instantiate(BigExplosionPrefab);
                    var angle = j * Mathf.PI / 2.0f + Mathf.PI / 4;
                    var offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

                    if (i == 0)
                        offset *= 0.16f;
                    else if (i == 1)
                        offset *= 0.24f;
                    else
                        offset *= 0.32f;

                    explosion.transform.position = transform.position + offset;
                    explosions.Add(explosion);
                }

                yield return null;
            }

            yield return new WaitForSeconds(0.1f);
            explosions.DestroyAll();
            Renderer.enabled = false;

            InvokeDeath();

            yield return new WaitForSeconds(1.0f);

            foreach (var obj in GameOverObjects)
            {
                obj.SetActive(true);
            }

            _isGameOver = true;

            yield return null;
        }

        #endregion

        #region Methods

        public void Reset()
        {
            if (InitialPosition != null)
                transform.localPosition = InitialPosition.Value;

            foreach (var obj in GameOverObjects)
            {
                obj.SetActive(false);
            }

            Health = MaxHealth;
            IsDying = false;
            Renderer.enabled = true;
            Collider2D.enabled = true;
            IsReady = true;
        }

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

            if (_shots.Count < MaximumShots)
            {
                var shotPrefab = Instantiate(ShotPrefab);
                var shotCollider = shotPrefab.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(shotCollider, Collider2D);
                var shot = shotPrefab.GetComponent<Shot>();
                shot.Source = this;
                shot.Direction = isLeft ? new Vector2(-1, 0) : new Vector2(1, 0);
                shot.transform.position = transform.position;

                _shots.Add(shot);

                AudioSource.PlayOneShot(ShootClip);
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
