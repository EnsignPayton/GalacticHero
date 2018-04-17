using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Utilities
{
    /// <summary>
    /// Utility class containing extension methods for Unity built-in types
    /// </summary>
    public static class UnityExtensions
    {
        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.y);
        }

        public static Vector3 ToVector3(this Vector2 vector2)
        {
            return new Vector3(vector2.x, vector2.y, 0.0f);
        }

        public static T RandomElement<T>(this IList<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count == 0)
                throw new ArgumentException("Must have at least one element", nameof(list));

            int i = Random.Range(0, list.Count);

            return list[i];
        }

        public static void DestroyAll(this ICollection<GameObject> objects)
        {
            if (objects == null)
                throw new ArgumentNullException(nameof(objects));

            foreach (var obj in objects)
            {
                Object.Destroy(obj);
            }

            objects.Clear();
        }
    }
}
