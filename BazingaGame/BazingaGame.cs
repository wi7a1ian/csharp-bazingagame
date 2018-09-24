using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using System;
using BazingaGame.Prefabs;
using BazingaGame.GameMap;
using BazingaGame.GameCamera;
using FarseerPhysics;

namespace BazingaGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class BazingaGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public World World = new World(new Vector2(0f, 9.82f));

        public BazingaGame()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1920 - 20;
            graphics.PreferredBackBufferHeight = 1080 - 80;

            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            //TargetElapsedTime = TimeSpan.FromTicks(333333);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Boxes
            for (int i = 0; i < 20; i++)
            {
                Components.Add(new BazingaBox(this, i*96, 10));
            }
            
            // Map
            _gameMap = new Map(this);
            Components.Add(_gameMap);

            // Player
            _gamePlayer = new Player(this, 400, 100);
            Components.Add(_gamePlayer);

            BazingaCamera.GetCamera().SetPlayerToFollow(_gamePlayer);

            base.Initialize();
        }

        private Map _gameMap;
        private Player _gamePlayer;

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            debugConsoleFont = Content.Load<SpriteFont>("Text");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));

            BazingaCamera.GetCamera().Update(gameTime, Keyboard.GetState());

            if( Keyboard.GetState().IsKeyDown(Keys.U))
            {
                _gameMap.SaveToFile();
            }

            base.Update(gameTime);
        }

        private SpriteFont debugConsoleFont;

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            spriteBatch.Begin();

            spriteBatch.DrawString(debugConsoleFont,
                String.Format("Screen size {0}, {1}", ConvertUnits.ToSimUnits(1920), ConvertUnits.ToSimUnits(1080)),
                new Vector2(10, 60), Color.Red);
            spriteBatch.DrawString(debugConsoleFont, 
                String.Format("Camera position: {0}, {1}", ConvertUnits.ToSimUnits(BazingaCamera.GetCamera().Position.X), ConvertUnits.ToSimUnits(BazingaCamera.GetCamera().Position.Y)), 
                new Vector2(10, 100), Color.Red);
            spriteBatch.DrawString(debugConsoleFont,
                String.Format("Player position: {0}, {1}", _gamePlayer.Body.Position.X, _gamePlayer.Body.Position.Y),
                new Vector2(10, 140), Color.Red);

            spriteBatch.DrawString(debugConsoleFont,
                String.Format("Player X position - camera X position: {0}", _gamePlayer.Body.Position.X - ConvertUnits.ToSimUnits(BazingaCamera.GetCamera().Position.X)),
                new Vector2(10, 180), Color.Red);
            spriteBatch.DrawString(debugConsoleFont,
                String.Format("Player Y position - camera Y position: {0}", _gamePlayer.Body.Position.Y - ConvertUnits.ToSimUnits(BazingaCamera.GetCamera().Position.Y)),
                new Vector2(10, 220), Color.Red);

            spriteBatch.DrawString(debugConsoleFont,
                String.Format("Camerra acceleration: {0}", ConvertUnits.ToSimUnits(BazingaCamera.GetCamera()._followAcceleration)),
                new Vector2(10, 260), Color.Red);

            spriteBatch.End();
        }
    }
}
