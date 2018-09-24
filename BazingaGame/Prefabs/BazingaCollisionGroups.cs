using FarseerPhysics.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BazingaGame.Prefabs
{
    public static class BazingaCollisionGroups
    {
        // Basic
        public static Category Player = Category.Cat1;
        public static Category Ground = Category.Cat2;
        public static Category Box = Category.Cat3;
        public static Category Weapon = Category.Cat4;

        // Combined:
        public static Category PlayerWeapon = Player | Weapon;

        // This stuff Player should never breach
        public static Category SolidObject = Category.Cat2 | Category.Cat3;
    }
}
