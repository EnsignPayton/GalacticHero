﻿using UnityEngine;

namespace Assets.Scripts
{
    public class Shot : Script
    {
        /// <summary>
        /// Horizontal speed
        /// </summary>
        public float Speed = 1.0f;

        public Entity Source { get; set; }
        public bool IsLeft { get; set; }

        protected override void Update()
        {
            var velocity = new Vector3();
            velocity.x = IsLeft ? -Speed : Speed;
            transform.position += velocity * Time.deltaTime;
        }

        protected override void OnBecameInvisible()
        {
            // Destroy when shot goes offscreen
            Dispose();
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            // Destroy when shot collides with a solid object
            Dispose();
        }
    }
}