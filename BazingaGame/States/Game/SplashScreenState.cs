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
	class SplashScreenState : SimpleGameObject, IGameState
    {
        //private BazingaGame Game;
        //private Texture2D _splashBazinga;
        //private Texture2D _splashKroll;

        private List<Texture2D> _splashScreens;
        //private SpriteBatch spriteBatch;
        private int _splashScreenShowTimeInSeconds;
        private int _currentSplashScreenIndex;
        private GameTime _lastSplashScreenChanged;

        public SplashScreenState(BazingaGame game)
			:base(game)
        {
            _splashScreens = new List<Texture2D>();
            _currentSplashScreenIndex = 0;
            _splashScreenShowTimeInSeconds = 4;

            _lastSplashScreenChanged = new GameTime();
        }

        public void Initialize()
        {
            
        }

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_splashScreens[_currentSplashScreenIndex], new Rectangle(0, 0, _splashScreens[_currentSplashScreenIndex].Width, _splashScreens[_currentSplashScreenIndex].Height), Color.White);
		}

		public IGameState Update(GameTime gameTime, InputHelper _gameInput)
		{
			if (gameTime.TotalGameTime.Seconds - _lastSplashScreenChanged.TotalGameTime.Seconds >= _splashScreenShowTimeInSeconds)
			{
				_currentSplashScreenIndex++;
				_lastSplashScreenChanged = new GameTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime);
			}

			if (_gameInput.IsNewKeyPress(Keys.Space))
			{
				_currentSplashScreenIndex++;
			}

			if (_currentSplashScreenIndex >= _splashScreens.Count)
			{
				return new MainMenuState(Game);
			}

			return null;
		}

		public void LoadContent()
		{
			_splashScreens.Add(Game.Content.Load<Texture2D>("Splash/Kroll"));
			_splashScreens.Add(Game.Content.Load<Texture2D>("Splash/Bazinga"));            
		}
	}
}
