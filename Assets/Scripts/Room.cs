using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Collider2D))]
    public class Room : Script
    {
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
    }
}
