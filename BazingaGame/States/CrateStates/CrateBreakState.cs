using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BazingaGame.Prefabs.Dynamic;
using BazingaGame.Prefabs;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using System;

namespace BazingaGame.States.CrateStates
{
    /// <summary>
    /// Finite State Mchine Pattern
    /// http://gameprogrammingpatterns.com/state.html
    /// </summary>
    public class CrateBreakState : IGameComponentState
    {
        private Crate crate;

        public void EnterState(StatefulGameComponent target)
        {
            crate = target as Crate;

            crate.Destroy();
        }

        public void ExitState()
        {
            // nop
        }

        public IGameComponentState HandleInput(KeyboardState input)
        {
            return null;
        }

        public IGameComponentState Update(GameTime gameTime)
        {
            return null;
        }
    }
}
