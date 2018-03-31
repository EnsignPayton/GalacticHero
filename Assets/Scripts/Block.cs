using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class Block : Script
    {
        public Collider2D LeftCollider;

        public Collider2D TopCollider;

        public Collider2D RightCollider;

        public Collider2D BottomCollider;
    }
}
