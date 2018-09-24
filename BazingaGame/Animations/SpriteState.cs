using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BazingaGame.Animations
{
    public enum SpriteState : int
    {
        Dead = 0,
        Idle,
        Jump,
        Melee,
        Run,
        Shoot,
        Slide
    }
}
