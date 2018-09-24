using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BazingaGame.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameInput;

namespace BazingaGame.States.Game
{
	class MainMenuState : SimpleGameObject, IGameState
    {
        private Menu _menu;
		//private IGameState _newGameState;

        public MainMenuState(BazingaGame game)
			:base(game)
        {
        }

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
			_menu.Draw(gameTime, spriteBatch);
        }

		public IGameState Update(GameTime gameTime, InputHelper _gameInput)
        {
			var returnedGameState = _menu.Update(gameTime, _gameInput	);

			return returnedGameState;
        }

		public void LoadContent()
        {
			_menu.LoadContent();
        }

		public void Initialize()
        {
            _menu = new Menu(Game, "");
            _menu.AddStateMenuItem("Start", typeof(GameMapState));
			_menu.AddStateMenuItem("Options", typeof(OptionsState));
            _menu.AddMenuItem("Exit", EntryType.ExitItem);

            // Audio
#if DEBUG
			//Game.BackgroundSong = Game.Content.Load<Song>(@"Sounds\Main");
			//MediaPlayer.Play(Game.BackgroundSong);
			//MediaPlayer.IsRepeating = true;
			//MediaPlayer.Volume = 0.5f;
#else
			Game.BackgroundSong = Game.Content.Load<Song>(@"Sounds\Main");
			MediaPlayer.Play(Game.BackgroundSong);
			MediaPlayer.IsRepeating = true;
			MediaPlayer.Volume = 0.5f;
#endif
        }
	}
}
