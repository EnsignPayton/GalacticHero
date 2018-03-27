using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Hero : MonoBehaviour
    {
        public float MoveSpeed = 1.0f;
        public GameObject ShotPrefab = null;

        private SpriteRenderer SpriteRenderer { get; set; }

        public void Start()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();

            if (ShotPrefab == null)
                Debug.LogError("Shot prefab undefined.");
        }

        public void Update()
        {
            Move();
            Shoot();
        }

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

            SpriteRenderer.flipX = isLeft;

            var shot = Instantiate(ShotPrefab);
            shot.transform.position = transform.position;
        }
    }
}
