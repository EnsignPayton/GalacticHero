using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Flame : Script
    {
        /// <summary>
        /// Time until expiration
        /// </summary>
        public float Duration;

        /// <summary>
        /// Destroy object when it expires
        /// </summary>
        protected override void Start()
        {
            StartCoroutine(Timer());

            base.Start();
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(Duration);

            Dispose();

            yield return null;
        }
    }
}
