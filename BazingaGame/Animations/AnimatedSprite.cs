using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BazingaGame.Animations
{
    class AnimatedSprite
    {
        private int _currentFrame;
        private int _totalFrames;
        private List<int> _spriteMap;

        public Texture2D Texture { get; set; }
        public int CurrentRow = 0;
        public int FrameWidth { get; private set; }
        public int FrameHeight { get; private set; }

        public int TotalColumns { get; private set; }
        public int TotalRows { get; private set; }
        public SpriteEffects Effect = SpriteEffects.None;
        public bool Repeat = true;

        public bool AnimationCompleted { get { return (_currentFrame >= _spriteMap[CurrentRow] - 1 && !Repeat); } }

        public AnimatedSprite(Texture2D texture, List<int> spriteMap)
        {
            Texture = texture;
            _spriteMap = spriteMap;
            _currentFrame = 0;
            TotalColumns = spriteMap.Max();
            TotalRows = spriteMap.Count;
            _totalFrames = TotalColumns;

            FrameWidth = Texture.Width / TotalColumns;
            FrameHeight = Texture.Height / TotalRows;
        }

        public void Update()
        {
            if (_currentFrame >= _spriteMap[CurrentRow]-1)
            {
                if (Repeat)
                {
                    _currentFrame = 0;
                }
            }
            else
            {
                _currentFrame++;
            }
            
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, float rotation, Vector2 origin)
        {
            
            int row = CurrentRow;// (int)((float)currentFrame / (float)Columns);
            int column = _currentFrame % _spriteMap[CurrentRow];

            Rectangle sourceRectangle = new Rectangle(FrameWidth * column, FrameHeight * row, FrameWidth, FrameHeight);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, FrameWidth, FrameHeight);

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, rotation, origin, Effect, 1f);
        }
    }
}
