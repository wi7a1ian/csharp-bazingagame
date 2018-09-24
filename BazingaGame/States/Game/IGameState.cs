using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BazingaGame.States.Game
{
    /// <summary>
    /// Finite State Mchine Pattern
    /// http://gameprogrammingpatterns.com/state.html
    /// </summary>
    public interface IGameState
    {
        //void Enter(BazingaPlayer player);
        //IGameState HandleInput(KeyboardState input);
        void Draw(GameTime gameTime);
        IGameState Update(GameTime gameTime);
        void LoadContent();
        void Initialize();
    }
}
