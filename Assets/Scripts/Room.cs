using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Collider2D))]
    public class Room : Script
    {
        #region Fields

        /// <summary>
        /// Max vertical or horizontal distance from center
        /// </summary>
        public float Size = 1.0f;

        /// <summary>
        /// Optional boss enemy for this room.
        /// </summary>
        public Entity Boss;

        /// <summary>
        /// Optional door objects for this room
        /// </summary>
        public GameObject[] Doors;

        #endregion

        #region Methods

        /// <summary>
        /// Sets all child objects except Hero active or inactive
        /// </summary>
        /// <param name="isActive">Active toggle</param>
        public void SetActive(bool isActive)
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Hero>() == null)
                    child.gameObject.SetActive(isActive);
            }
        }

        protected override void Awake()
        {
            if (Boss != null)
            {
                Boss.DeathEvent += Boss_EntityDied;
            }

            base.Awake();
        }

        private void Boss_EntityDied(object sender, System.EventArgs e)
        {
            if (Doors == null) return;
            foreach (var door in Doors)
            {
                door.SetActive(false);
            }
        }

        #endregion
    }
}
