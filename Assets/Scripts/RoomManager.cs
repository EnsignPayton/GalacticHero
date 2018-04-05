using System.Linq;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class RoomManager : Script
    {
        protected override void Awake()
        {
            foreach (Transform child in transform)
            {
                var room = child.GetComponent<Room>();
                if (room == null) continue;

                bool hasHero = child.Cast<Transform>().Any(x => x.GetComponent<Hero>() != null);

                if (!hasHero)
                {
                    room.SetActive(false);
                }
            }

            base.Awake();
        }
    }
}
