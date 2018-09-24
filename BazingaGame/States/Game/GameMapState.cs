using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BazingaGame.GameMap;
using BazingaGame.Particles;
using BazingaGame.Prefabs;
using BazingaGame.Prefabs.Dynamic;
using BazingaGame.HUD;
using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInput;

namespace BazingaGame.States.Game
{
	class GameMapState : SimpleGameObject, IGameState
    {
        private Map _gameMap;
        private GameHUD _gameHUD;
        private BazingaPlayer _gamePlayer;
        private ParticleEngine particleEngine;
        private SpriteFont _debugConsoleFont;

        private bool _renderDebugInfo = false;

        public GameMapState(BazingaGame game)
			:base(game)
        {

        }

		public void SetNewGameState(IGameState newGameState)
		{
			
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_renderDebugInfo)
            {
                spriteBatch.DrawString(_debugConsoleFont,
					String.Format("Screen size {0}, {1}", ConvertUnits.ToSimUnits(Game.GetGameWidthInPixels()), ConvertUnits.ToSimUnits(Game.GetGameHeightInPixels())),
                    new Vector2(1000, 60), Color.Red);
                spriteBatch.DrawString(_debugConsoleFont,
                    String.Format("Camera position: {0}, {1}", ConvertUnits.ToSimUnits(Game.Camera.Position.X), ConvertUnits.ToSimUnits(Game.Camera.Position.Y)),
                    new Vector2(1000, 100), Color.Red);
                spriteBatch.DrawString(_debugConsoleFont,
                    String.Format("Player position: {0}, {1}", _gamePlayer.Body.Position.X, _gamePlayer.Body.Position.Y),
                    new Vector2(1000, 140), Color.Red);

                spriteBatch.DrawString(_debugConsoleFont,
                    String.Format("Player X position - camera X position: {0}", _gamePlayer.Body.Position.X - ConvertUnits.ToSimUnits(Game.Camera.Position.X)),
                    new Vector2(1000, 180), Color.Red);
                spriteBatch.DrawString(_debugConsoleFont,
                    String.Format("Player Y position - camera Y position: {0}", _gamePlayer.Body.Position.Y - ConvertUnits.ToSimUnits(Game.Camera.Position.Y)),
                    new Vector2(1000, 220), Color.Red);

                spriteBatch.DrawString(_debugConsoleFont,
                    String.Format("Camera acceleration: {0}", ConvertUnits.ToSimUnits(Game.Camera._followAcceleration)),
                    new Vector2(1000, 260), Color.Red);
            }
        }

		public IGameState Update(GameTime gameTime, InputHelper _gameInput)
        {
            //Game.World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));
            Game.World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));

			if (_gameInput.IsNewKeyPress(Keys.F9))
                _renderDebugInfo = !_renderDebugInfo;

            particleEngine.Update();

            return null;
        }

        public void Initialize()
        {
            // Boxes (just for testing purphoses)
            for (int i = 0; i < 20; i++)
            {
                //Game.Components.Add(new Box(Game, i * 96, 10));
            }

            // Map
            _gameMap = new Map(Game);
            Game.Components.Add(_gameMap);

            // Player
            _gamePlayer = new BazingaPlayer(Game, 400, 100);
            Game.Components.Add(_gamePlayer);

            // HUD
            _gameHUD = new GameHUD(Game, new Vector2(20, 20));
            Game.Components.Add(_gameHUD);

            var box = new Crate(Game as BazingaGame, 430, -100);
            Game.Components.Add(box);

            Texture2D texture = Game.Content.Load<Texture2D>("Particles/Particle"); //new Texture2D(graphics.GraphicsDevice, 10, 10)

            particleEngine = new ParticleEngine(Game, texture, new Vector2(800, 10));
            Game.Components.Add(particleEngine);

#if DEBUG
			Game.Camera.SetPlayerToFollow(_gamePlayer);
#else
			Game.Camera.SetPlayerToFollow(_gamePlayer);
#endif
        }

        public void LoadContent()
        {
            _debugConsoleFont = Game.Content.Load<SpriteFont>("Text");
        }
    }
}
