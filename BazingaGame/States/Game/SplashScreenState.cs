using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BazingaGame.States.Game
{
    class SplashScreenState : IGameState
    {
        private BazingaGame Game;
        //private Texture2D _splashBazinga;
        //private Texture2D _splashKroll;

        private List<Texture2D> _splashScreens;
        private SpriteBatch spriteBatch;
        private int _splashScreenShowTimeInSeconds;
        private int _currentSplashScreenIndex;
        private GameTime _lastSplashScreenChanged;

        public SplashScreenState(BazingaGame game)
        {
            Game = game;

            _splashScreens = new List<Texture2D>();
            _currentSplashScreenIndex = 0;
            _splashScreenShowTimeInSeconds = 4;

            _lastSplashScreenChanged = new GameTime();
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_splashScreens[_currentSplashScreenIndex], new Rectangle(0, 0, _splashScreens[_currentSplashScreenIndex].Width, _splashScreens[_currentSplashScreenIndex].Height), Color.White);

            //if (gameTime.TotalGameTime.Seconds <= _splashScreenShowTimeInSeconds)
            //{
            //    spriteBatch.Draw(_splashKroll, new Rectangle(0, 0, _splashKroll.Width, _splashKroll.Height), Color.White);
            //}
            //else
            //{
            //    spriteBatch.Draw(_splashBazinga, new Rectangle(0, 0, _splashKroll.Width, _splashKroll.Height), Color.White);
            //}

            spriteBatch.End();
        }

        KeyboardState _oldState;

        public IGameState Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.Seconds - _lastSplashScreenChanged.TotalGameTime.Seconds >= _splashScreenShowTimeInSeconds)
            {
                _currentSplashScreenIndex++;
                _lastSplashScreenChanged = new GameTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime);
            }

            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Space) && !_oldState.IsKeyDown(Keys.Space))
            {
                _currentSplashScreenIndex++;
            }

            if (_currentSplashScreenIndex >= _splashScreens.Count)
            {
                return new GameMapState(Game);
            }

            _oldState = state;

            return null;
        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            _splashScreens.Add(Game.Content.Load<Texture2D>("Splash/Kroll"));
            _splashScreens.Add(Game.Content.Load<Texture2D>("Splash/Bazinga"));            
        }

        public void Initialize()
        {
            
        }
    }
}
