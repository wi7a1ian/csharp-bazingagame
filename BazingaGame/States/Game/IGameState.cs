using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameInput;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
		//IGameState Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState oldKeyboardState);
		IGameState Update(GameTime gameTime, InputHelper inputHelper);
		//void SetNewGameState(IGameState newGameState);
        void LoadContent();
        void Initialize();
    }
}
