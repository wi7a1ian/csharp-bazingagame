using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BazingaGame.States.Game;
using GameInput;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BazingaGame.Prefabs;

namespace BazingaGame.UI
{
    public enum EntryType
    {
        State,
        Separator,
        ExitItem
    }

    /// <summary>
    /// Helper class represents a single entry in a Menu. By default this
    /// just draws the entry text string, but it can be customized to display menu
    /// entries in different ways. This also provides an event that will be raised
    /// when the menu entry is selected.
    /// </summary>
    public sealed class MenuEntry: StatelessGameComponent
    {
        private Vector2 _baseOrigin;

        private float _height;
        private Menu _menu;

        private float _scale;

        /// <summary>
        /// Tracks a fading selection effect on the entry.
        /// </summary>
        /// <remarks>
        /// The entries transition out of the selection effect when they are deselected.
        /// </remarks>
        private float _selectionFade;
		private bool _isSelected;

        private EntryType _type;
        private float _width;
		private Type _nextGameState;

        /// <summary>
        /// Constructs a new menu entry with the specified text.
        /// </summary>
		public MenuEntry(BazingaGame game, Menu menu, string text, EntryType type, Type nextGameState = null)
            :base(game)
        {
            Text = text;
            _type = type;
            _menu = menu;
            _scale = 0.9f;
            Alpha = 1.0f;
			_isSelected = false;
			_nextGameState = nextGameState;
        }

        /// <summary>
        /// Gets or sets the text of this menu entry.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the position at which to draw this menu entry.
        /// </summary>
        public Vector2 Position { get; set; }

        public float Alpha { get; set; }

        //public GameScreen Screen { get; private set; }

        public void Initialize()
        {
            SpriteFont font = _menu.FontMenu;

            _baseOrigin = new Vector2(font.MeasureString(Text).X, font.MeasureString("M").Y) * 0.5f;

            _width = font.MeasureString(Text).X * 0.8f;
            _height = font.MeasureString("M").Y * 0.8f;
        }

        public bool IsExitItem()
        {
            return _type == EntryType.ExitItem;
        }

        public bool IsSelectable()
        {
            return _type != EntryType.Separator;
        }

        /// <summary>
        /// Updates the menu entry.
        /// </summary>
		public IGameState Update(bool isSelected, GameTime gameTime, InputHelper inputHelper)
        {
            // there is no such thing as a selected item on Windows Phone, so we always
            // force isSelected to be false
#if WINDOWS_PHONE
            isSelected = false;
#endif
			IGameState returnedGameState = null;

			_isSelected = isSelected;
            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly
            // popping to the new state.
            if (_type != EntryType.Separator)
            {
                float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;
                if (isSelected)
                    _selectionFade = Math.Min(_selectionFade + fadeSpeed, 1f);
                else
                    _selectionFade = Math.Max(_selectionFade - fadeSpeed, 0f);

				_scale = 0.7f + 0.1f * _selectionFade * (_isSelected ? 2f : 1f);
            }

			if (inputHelper.IsNewKeyPress(Keys.Enter) && IsExitItem() && _isSelected)
			{
				Game.Exit();
			}

			if (inputHelper.IsNewKeyPress(Keys.Enter) && _nextGameState != null && _isSelected)
			{
				returnedGameState = (IGameState)_nextGameState.GetConstructor(new Type[] { typeof(BazingaGame) }).Invoke(new object[] { Game });
			}

			return returnedGameState;
        }

        /// <summary>
        /// Draws the menu entry. This can be overridden to customize the appearance.
        /// </summary>
		public void Draw(SpriteBatch spriteBatch)
        {
            SpriteFont font = _menu.FontMenu;

            // Draw the selected entry in yellow, otherwise white
			Color color = _type == EntryType.Separator ? Color.DarkOrange : Color.Lerp(new Color(0xa1, 0x77, 0x4d), new Color(0xe9, 0xaa, 0x45), _selectionFade);
            color *= Alpha;
			// #e9aa45

			//spriteBatch.Begin();
            // Draw text, centered on the middle of each line.
			spriteBatch.DrawString(font, Text, Position - _baseOrigin * _scale + Vector2.One, Color.DarkSlateGray * Alpha * Alpha, 0, Vector2.Zero, _scale, SpriteEffects.None, 1f);
			spriteBatch.DrawString(font, Text, Position - _baseOrigin * _scale, color, 0, Vector2.Zero, _scale, SpriteEffects.None, 1f);

			//spriteBatch.End();
        }

        /// <summary>
        /// Queries how much space this menu entry requires.
        /// </summary>
        public int GetHeight()
        {
            return (int)_height;
        }

        /// <summary>
        /// Queries how wide the entry is, used for centering on the screen.
        /// </summary>
        public int GetWidth()
        {
            return (int)_width;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }
    }
}
