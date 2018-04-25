using System;
using System.Linq;
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
                var hero = child.GetComponent<Hero>();

                // Not boss, door, or child, handle normally
                if (hero == null && (Boss == null || child != Boss.transform) && (Doors == null || !Doors.Contains(child.gameObject)))
                {
                    child.gameObject.SetActive(isActive);
                }
                else if (Boss != null && child == Boss.transform)
                {
                    // Handle boss
                    var silencer = Boss as Silencer;
                    if (silencer == null) throw new NotSupportedException("The only boss defined is a Silencer!");

                    // We definitely have a silencer boss now
                    if (!isActive)
                    {
                        // Handle deactivating like a normal object
                        child.gameObject.SetActive(false);
                    }
                    else if (!silencer.HasDied)
                    {
                        // Completely reset the boss and children
                        child.gameObject.SetActive(true);
                        foreach (Transform part in silencer.transform)
                        {
                            part.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        // Do not set a silencer that has died active
                    }
                }
                else if (Doors != null && Doors.Contains(child.gameObject))
                {
                    // Handle doors
                    var silencer = Boss as Silencer;
                    if (silencer == null)
                    {
                        // Handle like normal if there is no boss
                        child.gameObject.SetActive(isActive);
                    }
                    else if (!isActive || !silencer.HasDied)
                    {
                        // ... or if we're diabling, or boss hasn't died
                        child.gameObject.SetActive(isActive);
                    }
                    else
                    {
                        // Only ignore if we're enabling and boss has died
                    }
                }
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
