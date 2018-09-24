using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BazingaGame.States;
using Microsoft.Xna.Framework.Input;

namespace BazingaGame.Prefabs
{
    public class StatefulGameComponent : StatelessGameComponent
    {
        protected IGameComponentState State;

        public StatefulGameComponent(BazingaGame game)
            :base(game)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();

            HandleInputState(kState);
            UpdateState(gameTime);

            base.Update(gameTime);
        }

        protected virtual void HandleInputState(KeyboardState input)
        {
            var newState = State.HandleInput(input);

            if (newState != null && State != newState)
            {
                if (State != null)
                {
                    State.ExitState();
                }

                State = newState;
                State.EnterState(this);
            }
        }

        protected virtual void UpdateState(GameTime gameTime)
        {
            if (State != null)
            {
                var newState = State.Update(gameTime);

                if (newState != null && State != newState)
                {
                    State.ExitState();
                    State = newState;
                    State.EnterState(this);
                }
            }
        }
    }
}
