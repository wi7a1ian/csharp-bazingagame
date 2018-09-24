using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BazingaGame.UI;

namespace BazingaGame.States.Game
{
    class MainMenuState : IGameState
    {
        private BazingaGame Game;
        private Menu _menu;

        public MainMenuState(BazingaGame game)
        {
            Game = game;
        }

        public void Draw(GameTime gameTime)
        {

        }

        public IGameState Update(GameTime gameTime)
        {
            return null;
        }

        public void LoadContent()
        {

        }

        public void Initialize()
        {
            _menu = new Menu(Game, "Bazinga Game");

            _menu.AddMenuItem("Start", EntryType.Separator);

            _menu.AddMenuItem("Exit", EntryType.ExitItem);

            Game.Components.Add(_menu);
        }
    }
}
