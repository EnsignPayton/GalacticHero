﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class Hero : Script
    {
        public float MoveSpeed = 1.0f;
        public int MaximumShots = 5;
        public GameObject ShotPrefab = null;

        public IList<Shot> Shots { get; set; }
        private SpriteRenderer SpriteRenderer { get; set; }
        private BoxCollider2D Collider { get; set; }

        #region Script Overrides

        protected override void Start()
        {
            if (ShotPrefab == null)
                Debug.LogError("Shot prefab undefined.");

            Shots = new List<Shot>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Collider = GetComponent<BoxCollider2D>();
        }

        protected override void Update()
        {
            Move();
            Shoot();
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
        }

        protected override void OnTriggerEnter2D(Collider2D triggerCollider)
        {
            var shot = triggerCollider.GetComponent<Shot>();
            if (shot != null && !shot.IsPlayer)
            {
                // Hit
            }
        }

        #endregion

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

            SpriteRenderer.flipX = isLeft;

            // Clean up old shots
            Shots = Shots.Where(x => x != null).ToList();

            if (Shots.Count < MaximumShots)
            {
                var shotPrefab = Instantiate(ShotPrefab);
                var shot = shotPrefab.GetComponent<Shot>();
                shot.IsPlayer = true;
                shot.IsLeft = isLeft;
                shot.transform.position = transform.position;
                Shots.Add(shot);
            }
        }
    }
}
