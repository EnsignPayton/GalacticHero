using UnityEngine;

namespace Assets.Scripts
{
    public class Crusher : BasicEnemy
    {
        protected override void Start()
        {
            base.Start();

            // sets initial velocity toward the hero
            var hero = GameObject.Find("Hero");
            if (hero != null)
            {
                Velocity = (hero.transform.position - transform.position).normalized * MoveSpeed;
            }
        }
    }
}
