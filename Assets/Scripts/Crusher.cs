using UnityEngine;

namespace Assets.Scripts
{
    public class Crusher : BasicEnemy
    {
        private Hero _hero;

        protected override void Start()
        {
            base.Start();

            // sets initial velocity toward the hero
            _hero = FindObjectOfType<Hero>();
            if (_hero != null)
            {
                Velocity = (_hero.transform.position - transform.position).normalized * MoveSpeed;
            }
        }

        protected override void SetVelocity()
        {
            if (_hero != null)
            {
                var position = Camera.main.WorldToViewportPoint(transform.position);

                if (position.x <= ScreenBoundOffset || position.x >= 1.0f - ScreenBoundOffset ||
                    position.y <= ScreenBoundOffset || position.y >= 1.0f - ScreenBoundOffset)
                {
                    Velocity = (_hero.transform.position - transform.position).normalized * MoveSpeed;
                }
            }
            else
            {
                // Only call when no hero is found
                base.SetVelocity();
            }
        }
    }
}
