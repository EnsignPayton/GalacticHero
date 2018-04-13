using System.Collections;
using Assets.Scripts.Entities;

namespace Assets.Scripts
{
    public class Silencer : Script
    {
        protected override void Start()
        {
            base.Start();
        }

        private IEnumerator PeriodicMovement()
        {
            // Or while enabled, while alive, etc.
            while (true)
            {
                // Move in first arc

                // Move in second arc

                // Move in third arc

                // Stop, thn repeat
            }
        }
    }
}
