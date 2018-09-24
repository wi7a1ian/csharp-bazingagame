using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BazingaGame.Animations
{
    public class AnimatedSprite
    {
        private int _currentFrame;
        private int _totalFrames;
        private List<int> _spriteMap;
        private TimeSpan _lastTime = TimeSpan.Zero;
        private int _fps;

        public Texture2D Texture { get; private set; }
        public int CurrentRow { get; private set; }
        public int FrameWidth { get; private set; }
        public int FrameHeight { get; private set; }

        public int TotalColumns { get; private set; }
        public int TotalRows { get; private set; }
        
        public bool AnimationCompleted { get { return (_currentFrame >= _spriteMap[CurrentRow] - 1 && !Repeat); } }
        public bool IsFlippedHorizontally { get { return Effect == SpriteEffects.FlipHorizontally; } }
        public bool IsFlippedVertically { get { return Effect == SpriteEffects.FlipVertically; } }

        public SpriteEffects Effect = SpriteEffects.None;
        public bool Repeat = true;


        public AnimatedSprite(Texture2D texture, List<int> spriteMap, int fps = 30)
        {
            Texture = texture;
            _spriteMap = spriteMap;
            _fps = fps;

            _currentFrame = 0;
            TotalColumns = spriteMap.Max();
            TotalRows = spriteMap.Count;
            _totalFrames = TotalColumns;

            FrameWidth = Texture.Width / TotalColumns;
            FrameHeight = Texture.Height / TotalRows;
        }

        public void PlaySprite(SpriteState state)
        {
            CurrentRow = ((int)state);
        }

        public void PlaySprite(SpriteState state, bool repeat)
        {
            Repeat = repeat;
            CurrentRow = ((int)state);
        }


        public void Update(GameTime gameTime)
        {
            var tDifference = gameTime.TotalGameTime - _lastTime;
            int elapsedFrames = (int)Math.Floor(tDifference.Milliseconds / 1000.0d * _fps);

            if (elapsedFrames > 0)
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
                _lastTime = gameTime.TotalGameTime;
            }
        }

        public void FlipHorizontally(bool flip = true)
        {
            if (flip)
            {
                Effect |= SpriteEffects.FlipHorizontally;
            }
            else
            {
                Effect &= ~SpriteEffects.FlipHorizontally;
            }
        }

        public void FlipVertically(bool flip = true)
        {
            if (flip)
            {
                Effect |= SpriteEffects.FlipVertically;
            }
            else
            {
                Effect &= ~SpriteEffects.FlipVertically;
            } 
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, float rotation, Vector2 origin)
        {
            
            int row = CurrentRow;
            int column = _currentFrame % _spriteMap[CurrentRow];

            Rectangle sourceRectangle = new Rectangle(FrameWidth * column, FrameHeight * row, FrameWidth, FrameHeight);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, FrameWidth, FrameHeight);

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, rotation, origin, Effect, 1f);
        }
    }
}
