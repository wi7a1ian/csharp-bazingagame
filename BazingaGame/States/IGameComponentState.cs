using BazingaGame.Prefabs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BazingaGame.States
{
    public interface IGameComponentState
    {
        void EnterState(StatefulGameComponent target);
        IGameComponentState HandleInput(KeyboardState input);
        IGameComponentState Update(GameTime gameTime);
        void ExitState();
    }
}
