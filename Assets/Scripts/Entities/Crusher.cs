using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Crusher : BasicEnemy
    {
        public float FollowDelay = 2.0f;

        private Hero _hero;

        protected override void Start()
        {
            base.Start();

            // sets initial velocity toward the hero
            _hero = FindObjectOfType<Hero>();
            if (_hero != null)
            {
                StartCoroutine(FollowHero());
            }
        }

        private IEnumerator FollowHero()
        {
            while (true)
            {
                Velocity = (_hero.transform.position - transform.position).normalized * MoveSpeed;

                yield return new WaitForSeconds(FollowDelay);
            }
        }
    }
}
