using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BazingaGame.GameMap;
using BazingaGame.Particles;
using BazingaGame.Prefabs;
using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BazingaGame.States.Game
{
    class GameMapState : IGameState
    {
        private BazingaGame Game;
        private Map _gameMap;
        private BazingaPlayer _gamePlayer;
        private ParticleEngine particleEngine;
        private SpriteFont _debugConsoleFont;

        private bool _renderDebugInfo = false;

        SpriteBatch spriteBatch;

        public GameMapState(BazingaGame game)
        {
            Game = game;
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (_renderDebugInfo)
            {
                spriteBatch.DrawString(_debugConsoleFont,
                    String.Format("Screen size {0}, {1}", ConvertUnits.ToSimUnits(1920), ConvertUnits.ToSimUnits(1080)),
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

            spriteBatch.End();
        }

        private KeyboardState _oldKeyboardState;

        public IGameState Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            //Game.World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));
            Game.World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));

            if (keyboardState.IsKeyDown(Keys.F9) && !_oldKeyboardState.IsKeyDown(Keys.F9))
                _renderDebugInfo = !_renderDebugInfo;

            particleEngine.Update();

            _oldKeyboardState = keyboardState;

            return null;
        }

        public void Initialize()
        {
            // Boxes
            for (int i = 0; i < 20; i++)
            {
                Game.Components.Add(new Box(Game, i * 96, 10));
            }

            // Map
            _gameMap = new Map(Game);
            Game.Components.Add(_gameMap);

            // Player
            _gamePlayer = new BazingaPlayer(Game, 400, 100);
            Game.Components.Add(_gamePlayer);

            Texture2D texture = Game.Content.Load<Texture2D>("Particles/Particle"); //new Texture2D(graphics.GraphicsDevice, 10, 10)

            particleEngine = new ParticleEngine(Game, texture, new Vector2(800, 10));
            Game.Components.Add(particleEngine);
        }



        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            _debugConsoleFont = Game.Content.Load<SpriteFont>("Text");
        }
    }
}
