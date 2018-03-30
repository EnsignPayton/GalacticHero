using UnityEngine;

namespace Assets.Scripts
{
    public class Reaver : BasicEnemy
    {
        /// <summary>
        /// Sprite to use when facing left.
        /// </summary>
        public Sprite LeftSprite;

        /// <summary>
        /// Sprite to use when facing right.
        /// </summary>
        public Sprite RightSprite;

        protected override void Update()
        {
            base.Update();

            if (IsReady)
            {
                // Set sprite based on velocity
                if (Velocity.x < 0.0f && SpriteRenderer.sprite != LeftSprite)
                {
                    Debug.Log("Set Reaver Left");
                    SpriteRenderer.sprite = LeftSprite;
                }
                else if (Velocity.x > 0.0f && SpriteRenderer.sprite != RightSprite)
                {
                    Debug.Log("Set Reaver Right");
                    SpriteRenderer.sprite = RightSprite;
                }
            }
        }
    }
}
