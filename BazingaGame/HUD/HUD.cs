using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using BazingaGame.Animations;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BazingaGame.Prefabs;

namespace BazingaGame.HUD
{
    public class GameHUD : StatelessGameComponent
    {
        public GameHUD(BazingaGame game, Vector2 hudPosition) : base(game)
        {
            this.hudPosition = hudPosition;
        }
        
        public int Live { get; set; }
        public int Score { get; set; }

        private Vector2 location;
        private SpriteFont fontScore;
        private int liveTotal = 10;
        private Texture2D heart;
        private Vector2 hudPosition;
		private int shadowLenght = 2;

        protected override void LoadContent()
        {
            base.LoadContent();

            fontScore = Game.Content.Load<SpriteFont>("MenuFont");
            heart = Game.Content.Load<Texture2D>("Heart");
           
            location = hudPosition;
            Score = 0;
            Live = liveTotal;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = SpriteBatch;
            SpriteFont font = fontScore;
            string scoreText = "Score: " + Score.ToString();

            spriteBatch.Begin();
            for (int i = 0; i <= liveTotal; i++)
            {
                SpriteBatch.Draw(heart, new Vector2(location.X + i*21, location.Y), Color.White);
            }
			spriteBatch.DrawString(font, scoreText, new Vector2(location.X + shadowLenght, location.Y + 20 + shadowLenght), Color.Black, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, scoreText, new Vector2(location.X, location.Y + 20), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
