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
    public class CrateSolidState : IGameComponentState
    {
        private Crate crate;
        private IGameComponentState nextState;

        public void EnterState(StatefulGameComponent target)
        {
            crate = target as Crate;

            crate.SetDefaultBodyFixture();
            crate.Body.OnCollision += OnPlayerCollision;
        }

        private bool OnPlayerCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            bool isCollidingWithPlayerWeapon = (fixtureB.CollisionCategories & BazingaCollisionGroups.PlayerWeapon) == BazingaCollisionGroups.PlayerWeapon;

            if (contact.IsTouching && isCollidingWithPlayerWeapon)
            {
                nextState = new CrateBreakState();
            }

            return true;
        }

        public void ExitState()
        {
            crate.Body.OnCollision -= OnPlayerCollision;
        }

        public IGameComponentState HandleInput(KeyboardState input)
        {
            return null;
        }

        public IGameComponentState Update(GameTime gameTime)
        {
            if(nextState != null)
            {
                return nextState;
            }
            return null;
        }
    }
}
