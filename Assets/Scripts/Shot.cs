using UnityEngine;

namespace Assets.Scripts
{
    public class Shot : Script
    {
        public float Speed = 4.0f;

        // TODO: Think this through and fix it
        public bool IsPlayer { get; set; }
        public Hero Source { get; set; }
        public bool IsLeft { get; set; }

        protected override void Update()
        {
            var velocity = new Vector3();
            velocity.x = IsLeft ? -Speed : Speed;
            transform.position += velocity * Time.deltaTime;
        }

        protected override void OnBecameInvisible()
        {
            Dispose();
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            Dispose();
        }

        protected override void OnDestroy()
        {
            if (Source != null)
                Source.Shots.Remove(this);
        }
    }
}
