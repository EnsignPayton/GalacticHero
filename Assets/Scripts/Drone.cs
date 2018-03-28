using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Drone : Script
    {
        public Sprite BlinkSprite;
        public int MaxHealth = 1;

        private SpriteRenderer SpriteRenderer { get; set; }
        private Sprite NormalSprite { get; set; }
        private bool IsReady { get; set; }
        private int Health { get; set; }

        protected override void Start()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            NormalSprite = SpriteRenderer.sprite;
            IsReady = false;
            Health = MaxHealth;

            StartCoroutine(Blink());
        }

        protected override void Update()
        {
            if (Health <= 0)
            {
                Dispose();
            }

            if (IsReady)
            {
            }
        }

        protected override void OnTriggerEnter2D(Collider2D triggerCollider)
        {
            var shot = triggerCollider.GetComponent<Shot>();
            if (shot != null && shot.IsPlayer)
            {
                Health--;
                shot.Dispose();
            }
        }

        // On start, pauses and blinks before acting
        private IEnumerator Blink()
        {
            yield return new WaitForSeconds(1.0f);

            // Set blink texture
            SpriteRenderer.sprite = BlinkSprite;

            yield return new WaitForSeconds(1.0f);

            SpriteRenderer.sprite = NormalSprite;
            IsReady = true;

            yield return null;
        }
    }
}
