using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BazingaGame
{
    public class GameObject : DrawableGameComponent
    {
        public new BazingaGame Game;

        protected SpriteBatch SpriteBatch;

        public GameObject(BazingaGame game)
            :base(game)
        {
            Game = game;
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(this.Game.GraphicsDevice);

            base.LoadContent();
        }
    }
}
