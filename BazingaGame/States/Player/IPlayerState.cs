using BazingaGame.Prefabs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BazingaGame.States.Player
{
    /// <summary>
    /// Finite State Mchine Pattern
    /// http://gameprogrammingpatterns.com/state.html
    /// </summary>
    public interface IPlayerState
    {
        void Enter(BazingaPlayer player);
        void Exit(BazingaPlayer player);
        IPlayerState HandleInput(BazingaPlayer player, KeyboardState input);
        IPlayerState Update(BazingaPlayer player, GameTime gameTime);
    }
}
