using System;

namespace Assets.Scripts
{
    // Unity might have a built in enum but I'll use this for now.
    [Flags]
    public enum Direction
    {
        None = 0,
        Left = 1,
        Top = 2,
        Right = 4,
        Bottom = 8
    }
}
