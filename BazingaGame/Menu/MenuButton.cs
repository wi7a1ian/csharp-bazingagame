using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BazingaGame.UI
{
    /// <summary>
    /// Helper class represents a single entry in a Menu. By default this
    /// just draws the entry text string, but it can be customized to display menu
    /// entries in different ways. This also provides an event that will be raised
    /// when the menu entry is selected.
    /// </summary>
    public sealed class MenuButton: GameObject
    {
        private Vector2 _baseOrigin;
        private bool _flip;

        private float _scale;

        /// <summary>
        /// Tracks a fading selection effect on the entry.
        /// </summary>
        /// <remarks>
        /// The entries transition out of the selection effect when they are deselected.
        /// </remarks>
        private float _selectionFade;

        private Texture2D _sprite;

        /// <summary>
        /// Constructs a new menu entry with the specified text.
        /// </summary>
        public MenuButton(BazingaGame game, Texture2D sprite, bool flip, Vector2 position)
            :base(game)
        {
            _scale = 1f;
            _sprite = sprite;
            _baseOrigin = new Vector2(_sprite.Width / 2f, _sprite.Height / 2f);
            Hover = false;
            _flip = flip;
            Position = position;
        }

        /// <summary>
        /// Gets or sets the position at which to draw this menu entry.
        /// </summary>
        public Vector2 Position { get; set; }

        public bool Hover { get; set; }

        /// <summary>
        /// Updates the menu entry.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;
            _selectionFade = Hover ? Math.Min(_selectionFade + fadeSpeed, 1f) : Math.Max(_selectionFade - fadeSpeed, 0f);
            _scale = 1f + 0.1f * _selectionFade;
        }

        public void Collide(Vector2 position)
        {
            Rectangle collisonBox = new Rectangle((int)(Position.X - _sprite.Width / 2f), (int)(Position.Y - _sprite.Height / 2f), (_sprite.Width), (_sprite.Height));

            Hover = collisonBox.Contains((int)position.X, (int)position.Y);
        }

        /// <summary>
        /// Draws the menu entry. This can be overridden to customize the appearance.
        /// </summary>
        public void Draw()
        {
            Color color = Color.Lerp(Color.White, new Color(255, 210, 0), _selectionFade);

            SpriteBatch.Draw(_sprite, Position - _baseOrigin * _scale, null, color, 0f, Vector2.Zero, _scale, _flip ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
        }
    }
}
