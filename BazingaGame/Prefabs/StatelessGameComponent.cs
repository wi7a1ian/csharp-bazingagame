using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BazingaGame.Prefabs
{
    public class StatelessGameComponent : DrawableGameComponent
    {
        public new BazingaGame Game;

        protected SpriteBatch SpriteBatch;

        public StatelessGameComponent(BazingaGame game)
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
