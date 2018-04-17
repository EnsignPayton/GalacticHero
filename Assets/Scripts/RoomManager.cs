using System.Collections;
using System.Linq;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class RoomManager : Script
    {
        /// <summary>
        /// Room to spawn the player initially
        /// </summary>
        public Room StartRoom;

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

        protected override void Start()
        {
            base.Start();

            Screen.SetResolution(Camera.main.pixelHeight, Camera.main.pixelHeight, false);
        }

        public void RestartGame(Hero hero)
        {
            Debug.Log("Restarting the game...");
            StartCoroutine(RoomTransition(hero, hero.CurrentRoom, StartRoom, false, true));
        }

        public void TransitionRooms(Hero hero, Room newRoom, bool animate)
        {
            Debug.Log($"Transitioning to {newRoom.name}");
            StartCoroutine(RoomTransition(hero, hero.CurrentRoom, newRoom, animate, false));
        }

        private static IEnumerator RoomTransition(Hero hero, Room oldRoom, Room newRoom, bool animate, bool isRestarting)
        {
            hero.IsReady = false;

            oldRoom.SetActive(false);
            hero.transform.parent = newRoom.transform;

            var initialPosition = Camera.main.transform.position;

            var finalPosition = Camera.main.transform.position;
            finalPosition.x = newRoom.transform.position.x;
            finalPosition.y = newRoom.transform.position.y;

            var difference = finalPosition - initialPosition;

            if (animate)
            {
                for (int i = 1; i <= 45; i++)
                {
                    var tempPosition = initialPosition + (difference * i / 45.0f);
                    Camera.main.transform.position = tempPosition;

                    yield return null;
                }
            }

            Camera.main.transform.position = finalPosition;
            newRoom.SetActive(true);

            if (isRestarting)
            {
                hero.Reset();
            }
            else
            {
                var localPosition = hero.transform.localPosition;
                localPosition.x = Mathf.Clamp(localPosition.x, -newRoom.Size, newRoom.Size);
                localPosition.y = Mathf.Clamp(localPosition.y, -newRoom.Size, newRoom.Size);
                hero.transform.localPosition = localPosition;
            }

            hero.IsReady = true;

            yield return null;
        }
    }
}
