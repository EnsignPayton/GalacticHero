using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class Silencer : Script
    {
        // TODO: Plan
        // Main Silencer enemy does not react to hits. Walls and Core do.
        // Each have independent health, hit order should be determined by physics.
        // When core dies, entire Silencer dies. So maybe the core should have the main one...

        // Also hey the silencer should probably have a physical collider but it needs to not be on the middle part. Probably fix in editor.
    }
}
