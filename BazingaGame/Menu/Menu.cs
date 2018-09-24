using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BazingaGame.States.Game;
using GameInput;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BazingaGame.UI
{
	public class Menu : SimpleGameObject
    {
        public Menu(BazingaGame game, string menuTitle)
            :base(game)
        {
            _menuTitle = menuTitle;
            _selectedEntry = 0;

            //TransitionOnTime = TimeSpan.FromSeconds(0.7);
            //TransitionOffTime = TimeSpan.FromSeconds(0.7);
            //HasCursor = true;

			_backgroundOffset = -100;
        }

		public Texture2D LogoSprite { get; private set; }
		private Texture2D BackgroundSprite;

        private const float NumEntries = 15;
        //private const float NumEntries = 9;
        private List<MenuEntry> _menuEntries = new List<MenuEntry>();
        private string _menuTitle;
        private Vector2 _titlePosition;
        private Vector2 _titleOrigin;
        private int _selectedEntry;
        private float _menuBorderTop;
        private float _menuBorderBottom;
        private float _menuBorderMargin;
        private float _menuOffset;
        private float _maxOffset;

		private int _backgroundOffset;

        //private Texture2D _texScrollButton;
        //private Texture2D _texSlider;

		//private MenuButton _scrollUp;
		//private MenuButton _scrollDown;
		//private MenuButton _scrollSlider;
        //private bool _scrollLock;

        public SpriteFont FontMenu { get; private set; }

		private Type _nextGameState;

        /// <summary>
        /// Gets the current position of the screen transition, ranging
        /// from zero (fully active, no transition) to one (transitioned
        /// fully off to nothing).
        /// </summary>
        protected float TransitionPosition { get; set; }

        public void AddMenuItem(string name, EntryType type)
        {
            MenuEntry entry = new MenuEntry(Game, this, name, type);
            //Game.Components.Add(entry);
            _menuEntries.Add(entry);
        }

		public void AddStateMenuItem(string name, Type nextGameState)
		{
			_nextGameState = nextGameState;
			MenuEntry entry = new MenuEntry(Game, this, name, EntryType.State, nextGameState);
			//Game.Components.Add(entry);
			_menuEntries.Add(entry);			
		}

        public void LoadContent()
        {
            //base.LoadContent();

            FontMenu = Game.Content.Load<SpriteFont>("MenuFont");

            Viewport viewport = Game.GraphicsDevice.Viewport;// ScreenManager.GraphicsDevice.Viewport;
            SpriteFont font = FontMenu;

            //_texScrollButton = ScreenManager.Content.Load<Texture2D>("Common/arrow");
            //_texSlider = ScreenManager.Content.Load<Texture2D>("Common/slider");

            //float scrollBarPos = viewport.Width / 2f;
            for (int i = 0; i < _menuEntries.Count; ++i)
            {
                _menuEntries[i].Initialize();
                //scrollBarPos = Math.Min(scrollBarPos, (viewport.Width - _menuEntries[i].GetWidth()) / 2f);
            }

            //scrollBarPos -= _texScrollButton.Width + 2f;

            _titleOrigin = font.MeasureString(_menuTitle) / 2f;
            _titlePosition = new Vector2(viewport.Width / 2f, font.MeasureString("M").Y / 2f + 10f);

            _menuBorderMargin = font.MeasureString("M").Y * 0.8f;
            _menuBorderTop = (viewport.Height - _menuBorderMargin * (NumEntries - 1)) / 2f;
            _menuBorderBottom = (viewport.Height + _menuBorderMargin * (NumEntries - 1)) / 2f;

            _menuOffset = 500f;
            _maxOffset = Math.Max(0f, (_menuEntries.Count - NumEntries) * _menuBorderMargin);

			LogoSprite = Game.Content.Load<Texture2D>("Menu/Logo");
			BackgroundSprite = Game.Content.Load<Texture2D>("Background");

            //_scrollUp = new MenuButton(_texScrollButton, false, new Vector2(scrollBarPos, _menuBorderTop - _texScrollButton.Height), this);
            //_scrollDown = new MenuButton(_texScrollButton, true, new Vector2(scrollBarPos, _menuBorderBottom + _texScrollButton.Height), this);
            //_scrollSlider = new MenuButton(_texSlider, false, new Vector2(scrollBarPos, _menuBorderTop), this);

            //_scrollLock = false;
        }

		//public override void UnloadContent()
		//{
		//	for (int i = 0; i < _menuEntries.Count; ++i)
		//	{
		//		Game.Components.Remove(_menuEntries[i]);
		//		//scrollBarPos = Math.Min(scrollBarPos, (viewport.Width - _menuEntries[i].GetWidth()) / 2f);
		//	}

		//	//base.UnloadContent();
		//}

        ///// <summary>
        ///// Returns the index of the menu entry at the position of the given mouse state.
        ///// </summary>
        ///// <returns>Index of menu entry if valid, -1 otherwise</returns>
        //private int GetMenuEntryAt(Vector2 position)
        //{
        //    int index = 0;
        //    foreach (MenuEntry entry in _menuEntries)
        //    {
        //        float width = entry.GetWidth();
        //        float height = entry.GetHeight();
        //        Rectangle rect = new Rectangle((int)(entry.Position.X - width / 2f), (int)(entry.Position.Y - height / 2f), (int)width, (int)height);

        //        if (rect.Contains((int)position.X, (int)position.Y) && entry.Alpha > 0.1f)
        //            return index;

        //        ++index;
        //    }
        //    return -1;
        //}

        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        public void HandleInput(InputHelper input, GameTime gameTime)
        {
            // Mouse or touch on a menu item
            //int hoverIndex = GetMenuEntryAt(input.Cursor);
            //if (hoverIndex > -1 && _menuEntries[hoverIndex].IsSelectable() && !_scrollLock)
            //    _selectedEntry = hoverIndex;
            //else
            //    _selectedEntry = -1;

			//_scrollSlider.Hover = false;
			//if (input.IsCursorValid)
			//{
			//	_scrollUp.Collide(input.Cursor);
			//	_scrollDown.Collide(input.Cursor);
			//	_scrollSlider.Collide(input.Cursor);
			//}
			//else
			//{
			//	_scrollUp.Hover = false;
			//	_scrollDown.Hover = false;
			//	//_scrollLock = false;
			//}

            // Accept or cancel the menu? 
            if (input.IsMenuSelect() && _selectedEntry != -1)
            {
                if (_menuEntries[_selectedEntry].IsExitItem())
                    Game.Exit();

                //TODO:

                //else if (_menuEntries[_selectedEntry].Screen != null)
                //{
                //    ScreenManager.AddScreen(_menuEntries[_selectedEntry].Screen);

                //    if (_menuEntries[_selectedEntry].Screen is IDemoScreen)
                //        ScreenManager.AddScreen(new MessageBoxScreen((_menuEntries[_selectedEntry].Screen as IDemoScreen).GetDetails()));
                //}
            }
            else if (input.IsMenuCancel())
                Game.Exit();

            //if (input.IsMenuPressed())
            //{
            //    if (_scrollUp.Hover)
            //    {
            //        _menuOffset = Math.Max(_menuOffset - 200f * (float)gameTime.ElapsedGameTime.TotalSeconds, 0f);
            //        _scrollLock = false;
            //    }
            //    if (_scrollDown.Hover)
            //    {
            //        _menuOffset = Math.Min(_menuOffset + 200f * (float)gameTime.ElapsedGameTime.TotalSeconds, _maxOffset);
            //        _scrollLock = false;
            //    }
            //    if (_scrollSlider.Hover)
            //    {
            //        _scrollLock = true;
            //    }
            //}

            //if (input.IsMenuReleased())
            //    _scrollLock = false;

            //if (_scrollLock)
            //{
            //    _scrollSlider.Hover = true;
            //    _menuOffset = Math.Max(Math.Min(((input.Cursor.Y - _menuBorderTop) / (_menuBorderBottom - _menuBorderTop)) * _maxOffset, _maxOffset), 0f);
            //}

            if(input.KeyboardState.GetPressedKeys().Contains(Keys.Down))
            {
                _selectedEntry++;
            }

            if (input.KeyboardState.GetPressedKeys().Contains(Keys.Up))
            {
                _selectedEntry--;
            }
        }

        /// <summary>
        /// Allows the screen the chance to position the menu entries. By default
        /// all menu entries are lined up in a vertical list, centered on the screen.
        /// </summary>
        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Vector2 position = Vector2.Zero;
            position.Y = _menuBorderTop + _menuOffset;

            // update each menu entry's location in turn
            for (int i = 0; i < _menuEntries.Count; ++i)
            {
                position.X = Game.GraphicsDevice.Viewport.Width / 2f;
                //if (ScreenState == ScreenState.TransitionOn)
                //    position.X -= transitionOffset * 256;
                //else
                    position.X += transitionOffset * 256;

                // set the entry's position
                _menuEntries[i].Position = position;

                if (position.Y < _menuBorderTop)
                    _menuEntries[i].Alpha = 1f - Math.Min(_menuBorderTop - position.Y, _menuBorderMargin) / _menuBorderMargin;
                else if (position.Y > _menuBorderBottom)
                    _menuEntries[i].Alpha = 1f - Math.Min(position.Y - _menuBorderBottom, _menuBorderMargin) / _menuBorderMargin;
                else
                    _menuEntries[i].Alpha = 1f;

                // move down for the next entry the size of this entry
                position.Y += _menuEntries[i].GetHeight();
            }
            //Vector2 scrollPos = _scrollSlider.Position;
            //scrollPos.Y = MathHelper.Lerp(_menuBorderTop, _menuBorderBottom, _menuOffset / _maxOffset);
            //_scrollSlider.Position = scrollPos;
        }

        /// <summary>
        /// Updates the menu.
        /// </summary>
		public IGameState Update(GameTime gameTime, InputHelper gameInput)
        {
            //base.Update(gameTime);

			IGameState returnedGameState = null;

			if (gameInput.IsNewKeyPress(Keys.W) || gameInput.IsNewKeyPress(Keys.Up))
			{
				if(_selectedEntry > 0)
				{
					_selectedEntry--;
				}
			}

			if (gameInput.IsNewKeyPress(Keys.S) || gameInput.IsNewKeyPress(Keys.Down))
			{
				if (_selectedEntry < _menuEntries.Count - 1)
				{
					_selectedEntry++;
				}
			}

            // Update each nested MenuEntry object.
            for (int i = 0; i < _menuEntries.Count; ++i)
            {
                bool isSelected = (i == _selectedEntry);
				var entryGameState = _menuEntries[i].Update(isSelected, gameTime, gameInput);

				if (entryGameState != null)
				{
					returnedGameState = entryGameState;
				}
            }

			_backgroundOffset++;

            //_scrollUp.Update(gameTime);
            //_scrollDown.Update(gameTime);
            //_scrollSlider.Update(gameTime);

			return returnedGameState;
        }

        /// <summary>
        /// Draws the menu.
        /// </summary>
		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

            //SpriteBatch spriteBatch = SpriteBatch;
            SpriteFont font = FontMenu;

            //spriteBatch.Begin();

			spriteBatch.Draw(BackgroundSprite, new Vector2(), new Rectangle(0, -_backgroundOffset, 1920, 1080), Color.White, 0f, new Vector2(), 1f, SpriteEffects.None, 0f);

			spriteBatch.Draw(LogoSprite, new Vector2((Game.GetGameWidthInPixels() - LogoSprite.Width) / 2, 40), Color.White);

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            Vector2 transitionOffset = new Vector2(0f, (float)Math.Pow(TransitionPosition, 2) * 100f);

            spriteBatch.DrawString(font, _menuTitle, _titlePosition - transitionOffset + Vector2.One * 2f, Color.Black, 0, _titleOrigin, 1f, SpriteEffects.None, 10f);
            spriteBatch.DrawString(font, _menuTitle, _titlePosition - transitionOffset, new Color(255, 210, 0), 0, _titleOrigin, 1f, SpriteEffects.None, 10f);

			// Draw each menu entry in turn.
			for (int i = 0; i < _menuEntries.Count; ++i)
			{
				_menuEntries[i].Draw(spriteBatch);
			}
			
            //_scrollUp.Draw();
            //_scrollSlider.Draw();
            //_scrollDown.Draw();
            //spriteBatch.End();

			
        }
    }
}
