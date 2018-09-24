using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using System;
using BazingaGame.Prefabs;
using BazingaGame.GameMap;
using BazingaGame.Display;
using FarseerPhysics;
using FarseerPhysics.DebugView;
using BazingaGame.Particles;
using BazingaGame.States.Game;
using BazingaGame.Input;

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
        private float _displayUnitsToSimUnitsRatio = 1 / ConvertUnits.ToSimUnits(1);
        protected DebugViewXNA DebugView;


        private Camera _camera;
        private IGameState _gameState;
        private KeyboardState _oldKeyboardState;

        public InputHelper InputHelper { get; private set; }
        public Camera Camera { get { return _camera; } }

        public BazingaGame()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1920 - 20;
            graphics.PreferredBackBufferHeight = 1080 - 80;

            //graphics.PreferredBackBufferWidth = 1920;
            //graphics.PreferredBackBufferHeight = 1080;
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";

            _gameState = new SplashScreenState(this);

            InputHelper = new InputHelper(this);

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
            _camera = new Camera(GraphicsDevice.Viewport);
            Settings.MaxPolygonVertices = 12;

            //Camera.SetPlayerToFollow(_gamePlayer);

            //_gameState = new GameMapState(this);

            _gameState.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            if (DebugView == null)
            {
                DebugView = new DebugViewXNA(World);
                DebugView.RemoveFlags(DebugViewFlags.Shape);
                DebugView.RemoveFlags(DebugViewFlags.Joint);
                DebugView.DefaultShapeColor = Color.White;
                DebugView.SleepingShapeColor = Color.LightGray;
                DebugView.LoadContent(GraphicsDevice, Content);
            }

            _gameState.LoadContent();
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

            var keyboardState = Keyboard.GetState();

            Camera.Update(gameTime, keyboardState);

            if (keyboardState.IsKeyDown(Keys.F1) && !_oldKeyboardState.IsKeyDown(Keys.F1))
                EnableOrDisableFlag(DebugViewFlags.Shape);
            if (keyboardState.IsKeyDown(Keys.F2) && !_oldKeyboardState.IsKeyDown(Keys.F2))
            {
                EnableOrDisableFlag(DebugViewFlags.DebugPanel);
                EnableOrDisableFlag(DebugViewFlags.PerformanceGraph);
            }
            if (keyboardState.IsKeyDown(Keys.F3) && !_oldKeyboardState.IsKeyDown(Keys.F3))
                EnableOrDisableFlag(DebugViewFlags.Joint);
            if (keyboardState.IsKeyDown(Keys.F4) && !_oldKeyboardState.IsKeyDown(Keys.F4))
            {
                EnableOrDisableFlag(DebugViewFlags.ContactPoints);
                EnableOrDisableFlag(DebugViewFlags.ContactNormals);
            }
            if (keyboardState.IsKeyDown(Keys.F5) && !_oldKeyboardState.IsKeyDown(Keys.F5))
                EnableOrDisableFlag(DebugViewFlags.PolygonPoints);
            if (keyboardState.IsKeyDown(Keys.F6) && !_oldKeyboardState.IsKeyDown(Keys.F6))
                EnableOrDisableFlag(DebugViewFlags.Controllers);
            if (keyboardState.IsKeyDown(Keys.F7) && !_oldKeyboardState.IsKeyDown(Keys.F7))
                EnableOrDisableFlag(DebugViewFlags.CenterOfMass);
            if (keyboardState.IsKeyDown(Keys.F8) && !_oldKeyboardState.IsKeyDown(Keys.F8))
                EnableOrDisableFlag(DebugViewFlags.AABB);

            var _newGameState = _gameState.Update(gameTime);

            if (_newGameState != null && _newGameState != _gameState)
            {
                _newGameState.Initialize();
                // TTA: LoadContent now?
                _newGameState.LoadContent(); // TODO: Make static states to initalize and load content in coresponding Game methods
                _gameState = _newGameState;
            }

            base.Update(gameTime);

            _oldKeyboardState = keyboardState;
        }

        private void EnableOrDisableFlag(DebugViewFlags flag)
        {
            if ((DebugView.Flags & flag) == flag)
                DebugView.RemoveFlags(flag);
            else
                DebugView.AppendFlags(flag);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _gameState.Draw(gameTime);

            base.Draw(gameTime);

            spriteBatch.Begin();

            Matrix projection = Matrix.CreateOrthographicOffCenter(
                0f, 
                graphics.GraphicsDevice.Viewport.Width / _displayUnitsToSimUnitsRatio,
                graphics.GraphicsDevice.Viewport.Height / _displayUnitsToSimUnitsRatio, 
                0f, 
                0f,
                1f
            );
            // draw the debug view
            DebugView.RenderDebugData(projection, Camera.GetScaledTransformMatrix());

            spriteBatch.End();

            //particleEngine.Draw(spriteBatch);
        }
    }
}
