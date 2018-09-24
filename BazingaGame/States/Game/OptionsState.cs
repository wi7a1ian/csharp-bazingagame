using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BazingaGame.UI;
using GameInput;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BazingaGame.States.Game
{
	class OptionsState : SimpleGameObject, IGameState
	{
		private Menu _menu;
		//private IGameState _newGameState;

		public OptionsState(BazingaGame game)
			:base(game)
		{

		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			_menu.Draw(gameTime, spriteBatch);
		}

		public IGameState Update(GameTime gameTime, InputHelper _gameInput)
		{
			var returnedGameState = _menu.Update(gameTime, _gameInput);

			return returnedGameState;
		}

		public void LoadContent()
		{
			_menu.LoadContent();
		}

		public void Initialize()
		{
			_menu = new Menu(Game, "");

			//_menu.AddStateMenuItem("Option1", ???);

			_menu.AddStateMenuItem("Back", typeof(MainMenuState));
		}
	}
}
