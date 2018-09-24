using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace BazingaGame.Input
{
    /// <summary>
    ///   an enum of all available mouse buttons.
    /// </summary>
    public enum MouseButtons
    {
        LeftButton,
        MiddleButton,
        RightButton,
        ExtraButton1,
        ExtraButton2
    }

    public class InputHelper: GameObject
    {
        private readonly List<GestureSample> _gestures = new List<GestureSample>();

        private bool _handleVirtualStick;

        private bool _cursorIsVisible;
        private Texture2D _cursorSprite;

        //private ScreenManager _manager;
        private Viewport _viewport;

        /// <summary>
        ///   Constructs a new input state.
        /// </summary>
        public InputHelper(BazingaGame game)
            :base(game)
        {
            KeyboardState = new KeyboardState();
            GamePadState = new GamePadState();
            MouseState = new MouseState();
            VirtualState = new GamePadState();

            PreviousKeyboardState = new KeyboardState();
            PreviousGamePadState = new GamePadState();
            PreviousMouseState = new MouseState();
            PreviousVirtualState = new GamePadState();

            //_manager = manager;

            _cursorIsVisible = false;
            IsCursorMoved = false;

            IsCursorValid = true;

            Cursor = Vector2.Zero;

            _handleVirtualStick = false;
        }

        public GamePadState GamePadState { get; private set; }

        public KeyboardState KeyboardState { get; private set; }

        public MouseState MouseState { get; private set; }

        public GamePadState VirtualState { get; private set; }

        public GamePadState PreviousGamePadState { get; private set; }

        public KeyboardState PreviousKeyboardState { get; private set; }

        public MouseState PreviousMouseState { get; private set; }

        public GamePadState PreviousVirtualState { get; private set; }

        public bool ShowCursor
        {
            get { return _cursorIsVisible && IsCursorValid; }
            set { _cursorIsVisible = value; }
        }

        public bool EnableVirtualStick
        {
            get { return _handleVirtualStick; }
            set { _handleVirtualStick = value; }
        }

        public Vector2 Cursor { get; private set; }

        public bool IsCursorMoved { get; private set; }

        public bool IsCursorValid { get; private set; }

        public void LoadContent()
        {
            _cursorSprite = Game.Content.Load<Texture2D>("Common/Cursor");

            _viewport = Game.GraphicsDevice.Viewport;
        }

        /// <summary>
        ///   Reads the latest state of the keyboard and gamepad and mouse/touchpad.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            PreviousKeyboardState = KeyboardState;
            PreviousGamePadState = GamePadState;
            PreviousMouseState = MouseState;

            if (_handleVirtualStick)
                PreviousVirtualState = VirtualState;

            KeyboardState = Keyboard.GetState();
            GamePadState = GamePad.GetState(PlayerIndex.One);
            MouseState = Mouse.GetState();

            if (_handleVirtualStick)
            {
                VirtualState = GamePad.GetState(PlayerIndex.One).IsConnected ? GamePad.GetState(PlayerIndex.One) : HandleVirtualStickWin();
            }

            _gestures.Clear();
            while (TouchPanel.IsGestureAvailable)
            {
                _gestures.Add(TouchPanel.ReadGesture());
            }

            // Update cursor
            Vector2 oldCursor = Cursor;
            if (GamePadState.IsConnected && GamePadState.ThumbSticks.Left != Vector2.Zero)
            {
                Vector2 temp = GamePadState.ThumbSticks.Left;
                Cursor += temp * new Vector2(300f, -300f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Mouse.SetPosition((int)Cursor.X, (int)Cursor.Y);
            }
            else
            {
                Cursor = new Vector2(MouseState.X, MouseState.Y);
            }

            Cursor = new Vector2(MathHelper.Clamp(Cursor.X, 0f, _viewport.Width), MathHelper.Clamp(Cursor.Y, 0f, _viewport.Height));

            if (IsCursorValid && oldCursor != Cursor)
                IsCursorMoved = true;
            else
                IsCursorMoved = false;

            IsCursorValid = _viewport.Bounds.Contains(MouseState.X, MouseState.Y);

        }

        public void Draw()
        {
            if (_cursorIsVisible && IsCursorValid)
            {
                SpriteBatch.Begin();
                SpriteBatch.Draw(_cursorSprite, Cursor, null, Color.White, 0f, new Vector2(_cursorSprite.Width / 2f, _cursorSprite.Height / 2f), 1f, SpriteEffects.None, 0f);
                SpriteBatch.End();
            }

        }

        private GamePadState HandleVirtualStickWin()
        {
            Vector2 leftStick = Vector2.Zero;
            List<Buttons> buttons = new List<Buttons>();

            if (KeyboardState.IsKeyDown(Keys.A))
                leftStick.X -= 1f;
            if (KeyboardState.IsKeyDown(Keys.S))
                leftStick.Y -= 1f;
            if (KeyboardState.IsKeyDown(Keys.D))
                leftStick.X += 1f;
            if (KeyboardState.IsKeyDown(Keys.W))
                leftStick.Y += 1f;
            if (KeyboardState.IsKeyDown(Keys.Space))
                buttons.Add(Buttons.A);
            if (KeyboardState.IsKeyDown(Keys.LeftControl))
                buttons.Add(Buttons.B);
            if (leftStick != Vector2.Zero)
                leftStick.Normalize();

            return new GamePadState(leftStick, Vector2.Zero, 0f, 0f, buttons.ToArray());
        }

        private GamePadState HandleVirtualStickWP7()
        {
            List<Buttons> buttons = new List<Buttons>();
            Vector2 stick = Vector2.Zero;

            return new GamePadState(stick, Vector2.Zero, 0f, 0f, buttons.ToArray());
        }

        /// <summary>
        ///   Helper for checking if a key was newly pressed during this update.
        /// </summary>
        public bool IsNewKeyPress(Keys key)
        {
            return (KeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key));
        }

        public bool IsNewKeyRelease(Keys key)
        {
            return (PreviousKeyboardState.IsKeyDown(key) && KeyboardState.IsKeyUp(key));
        }

        /// <summary>
        ///   Helper for checking if a button was newly pressed during this update.
        /// </summary>
        public bool IsNewButtonPress(Buttons button)
        {
            return (GamePadState.IsButtonDown(button) && PreviousGamePadState.IsButtonUp(button));
        }

        public bool IsNewButtonRelease(Buttons button)
        {
            return (PreviousGamePadState.IsButtonDown(button) && GamePadState.IsButtonUp(button));
        }

        /// <summary>
        ///   Helper for checking if a mouse button was newly pressed during this update.
        /// </summary>
        public bool IsNewMouseButtonPress(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LeftButton:
                    return (MouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released);
                case MouseButtons.RightButton:
                    return (MouseState.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton == ButtonState.Released);
                case MouseButtons.MiddleButton:
                    return (MouseState.MiddleButton == ButtonState.Pressed && PreviousMouseState.MiddleButton == ButtonState.Released);
                case MouseButtons.ExtraButton1:
                    return (MouseState.XButton1 == ButtonState.Pressed && PreviousMouseState.XButton1 == ButtonState.Released);
                case MouseButtons.ExtraButton2:
                    return (MouseState.XButton2 == ButtonState.Pressed && PreviousMouseState.XButton2 == ButtonState.Released);
                default:
                    return false;
            }
        }


        /// <summary>
        /// Checks if the requested mouse button is released.
        /// </summary>
        /// <param name="button">The button.</param>
        public bool IsNewMouseButtonRelease(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LeftButton:
                    return (PreviousMouseState.LeftButton == ButtonState.Pressed && MouseState.LeftButton == ButtonState.Released);
                case MouseButtons.RightButton:
                    return (PreviousMouseState.RightButton == ButtonState.Pressed && MouseState.RightButton == ButtonState.Released);
                case MouseButtons.MiddleButton:
                    return (PreviousMouseState.MiddleButton == ButtonState.Pressed && MouseState.MiddleButton == ButtonState.Released);
                case MouseButtons.ExtraButton1:
                    return (PreviousMouseState.XButton1 == ButtonState.Pressed && MouseState.XButton1 == ButtonState.Released);
                case MouseButtons.ExtraButton2:
                    return (PreviousMouseState.XButton2 == ButtonState.Pressed && MouseState.XButton2 == ButtonState.Released);
                default:
                    return false;
            }
        }

        /// <summary>
        ///   Checks for a "menu select" input action.
        /// </summary>
        public bool IsMenuSelect()
        {
            return IsNewKeyPress(Keys.Space) || IsNewKeyPress(Keys.Enter) || IsNewButtonPress(Buttons.A) || IsNewButtonPress(Buttons.Start) || IsNewMouseButtonPress(MouseButtons.LeftButton);
        }

        public bool IsMenuPressed()
        {
            return KeyboardState.IsKeyDown(Keys.Space) || KeyboardState.IsKeyDown(Keys.Enter) || GamePadState.IsButtonDown(Buttons.A) || GamePadState.IsButtonDown(Buttons.Start) || MouseState.LeftButton == ButtonState.Pressed;
        }

        public bool IsMenuReleased()
        {
            return IsNewKeyRelease(Keys.Space) || IsNewKeyRelease(Keys.Enter) || IsNewButtonRelease(Buttons.A) || IsNewButtonRelease(Buttons.Start) || IsNewMouseButtonRelease(MouseButtons.LeftButton);
        }

        /// <summary>
        ///   Checks for a "menu cancel" input action.
        /// </summary>
        public bool IsMenuCancel()
        {
            return IsNewKeyPress(Keys.Escape) || IsNewButtonPress(Buttons.Back);
        }
    }
}
