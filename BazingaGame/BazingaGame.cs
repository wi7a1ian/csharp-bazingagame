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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using GameInput;

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
        //private KeyboardState _oldKeyboardState;

        //public InputHelper InputHelper { get; private set; }
        public Camera Camera { get { return _camera; } }

        public Song BackgroundSong { get; set; }

		private InputHelper _gameInput;

        public BazingaGame()
        {
            graphics = new GraphicsDeviceManager(this);

			graphics.PreferredBackBufferWidth = GetGameWidthInPixels();
			graphics.PreferredBackBufferHeight = GetGameHeightInPixels();

            //graphics.PreferredBackBufferWidth = 1920;
            //graphics.PreferredBackBufferHeight = 1080;
#if DEBUG
            graphics.IsFullScreen = false;
#else
			graphics.IsFullScreen = true;
#endif
            Content.RootDirectory = "Content";

#if DEBUG
			_gameState = new MainMenuState(this);
#else
			_gameState = new SplashScreenState(this);
#endif

			//InputHelper = new InputHelper(this);

			_gameInput = new InputHelper(this);

            // Frame rate is 30 fps by default for Windows Phone.
            //TargetElapsedTime = TimeSpan.FromTicks(333333);
        }

		public IGameState GetCurentGameState()
		{
			return _gameState;
		}

		public int GetGameWidthInPixels()
		{
#if DEBUG
			return 1920 - 20;
#else
			return 1920;
#endif
		}

		public int GetGameHeightInPixels()
		{
#if DEBUG
			return 1080 - 80;
#else
			return 1080;
#endif
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

            // Utils - Debug View
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
			_gameInput.Update(gameTime);

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || _gameInput.IsNewKeyPress(Keys.Escape))
				Exit();

            var keyboardState = Keyboard.GetState();

			Camera.Update(gameTime, _gameInput);

			if (_gameInput.IsNewKeyPress(Keys.F1))
                EnableOrDisableFlag(DebugViewFlags.Shape);
			if (_gameInput.IsNewKeyPress(Keys.F2))
            {
                EnableOrDisableFlag(DebugViewFlags.DebugPanel);
                EnableOrDisableFlag(DebugViewFlags.PerformanceGraph);
            }
			if (_gameInput.IsNewKeyPress(Keys.F3))
                EnableOrDisableFlag(DebugViewFlags.Joint);
			if (_gameInput.IsNewKeyPress(Keys.F4))
            {
                EnableOrDisableFlag(DebugViewFlags.ContactPoints);
                EnableOrDisableFlag(DebugViewFlags.ContactNormals);
            }
			if (_gameInput.IsNewKeyPress(Keys.F5))
                EnableOrDisableFlag(DebugViewFlags.PolygonPoints);
			if (_gameInput.IsNewKeyPress(Keys.F6))
                EnableOrDisableFlag(DebugViewFlags.Controllers);
			if (_gameInput.IsNewKeyPress(Keys.F7))
                EnableOrDisableFlag(DebugViewFlags.CenterOfMass);
			if (_gameInput.IsNewKeyPress(Keys.F8))
                EnableOrDisableFlag(DebugViewFlags.AABB);

			var _newGameState = _gameState.Update(gameTime, _gameInput);

            if (_newGameState != null && _newGameState != _gameState)
            {
                _newGameState.Initialize();
                // TTA: LoadContent now?
                _newGameState.LoadContent(); // TODO: Make static states to initalize and load content in coresponding Game methods
                _gameState = _newGameState;
            }

            base.Update(gameTime);
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

            base.Draw(gameTime);

            spriteBatch.Begin();

			_gameState.Draw(gameTime, spriteBatch);

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
