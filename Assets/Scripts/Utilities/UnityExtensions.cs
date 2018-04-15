using UnityEngine;

namespace Assets.Scripts.Utilities
{
    /// <summary>
    /// Utility class containing all extension methods for Unity built-in types
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
    }
}
