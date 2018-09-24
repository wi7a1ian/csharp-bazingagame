using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BazingaGame.GameMap;
using BazingaGame.Display;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace BazingaGame.GameMap
{
    public class Map : GameObject
    {
        //private BazingaGame _game;
        private MapTileInfo[][] _mapTiles;
        //private Map _map;
        private Texture2D MapSprite;
        private Texture2D BackgroundSprite;
        private SpriteBatch _spriteBatch;

        public const int TileSize = 128;
        public const int MapWidth = 30;
        public const int MapHeight = 16;

        private enum MapSpriteTile
        {
            PlatformLeftEdge,
            PlatformMiddle,
            PlatformRightEdge,
            GroundMiddle,
            GroundLeftEdge,
            GroundRightEdge,
            GroundLeftEnd,
            GroundRightEnd,
            GroundLeftInnerCorner,
            GroundRightInnerCorner,
            GroundLeft,
            GroundRight,
            GroundBottom,
            Ground,
            GroundLeftoOuterCorner,
            GroundRightOuterCorner,
            None
        }

        private struct MapTileInfo
        {
            public MapSpriteTile TileSprite;
            public bool IsPassable;
            public Vector2 Origin;
            public Body Body;
        }

        public Map(BazingaGame game)
            :base(game)
        {

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);

            MapSprite = this.Game.Content.Load<Texture2D>("MapSprite");
            BackgroundSprite = this.Game.Content.Load<Texture2D>("Background");

            base.LoadContent();
        }

        public override void Initialize()
        {
            _mapTiles = new MapTileInfo[MapWidth][];

            for (int i = 0; i < _mapTiles.Length; i++)
            {
                _mapTiles[i] = new MapTileInfo[MapHeight];
            }

            for (int i = 0; i < _mapTiles.Length; i++)
            {
                for (int j = 0; j < _mapTiles[0].Length; j++)
                {
                    _mapTiles[i][j].TileSprite = MapSpriteTile.None;
                }
            }

            CreateSampleMap();

            CreateBodies(Game as BazingaGame);

            base.Initialize();
        }

        private void CreateBodies(BazingaGame game)
        {
            for (int i = 0; i < _mapTiles.Length; i++)
            {
                for (int j = 0; j < _mapTiles[0].Length; j++)
                {
                    if(_mapTiles[i][j].TileSprite != MapSpriteTile.None)
                    {
                        _mapTiles[i][j].IsPassable = false;
                        _mapTiles[i][j].Origin = new Vector2(TileSize / 2f, TileSize / 2f);
                        _mapTiles[i][j].Body = BodyFactory.CreateRectangle(game.World, ConvertUnits.ToSimUnits(TileSize), ConvertUnits.ToSimUnits(TileSize), 1f);
                        _mapTiles[i][j].Body.BodyType = BodyType.Static;
                        _mapTiles[i][j].Body.Position = ConvertUnits.ToSimUnits((i) * TileSize + TileSize / 2f, (j) * TileSize + TileSize / 2f);
                        _mapTiles[i][j].Body.Friction = 1f;
                        _mapTiles[i][j].Body.Restitution = 0f;
                        _mapTiles[i][j].Body.CollisionCategories = Category.Cat2;
                        _mapTiles[i][j].Body.CollidesWith = Category.All;
                    }
                    else
                    {
                        _mapTiles[i][j].IsPassable = true;
                        _mapTiles[i][j].Origin = new Vector2(TileSize / 2f, TileSize / 2f);
                        _mapTiles[i][j].Body = null;
                    }

                    if (_mapTiles[i][j].TileSprite == MapSpriteTile.GroundRight)
                    {
                        _mapTiles[i][j].Body.Friction = 0;

                    }
                }
            }
        }

        private void CreateSampleMap()
        {
            for (int i = 0; i < _mapTiles.Length; i++)
            {
                _mapTiles[i][7].TileSprite = MapSpriteTile.GroundMiddle;
            }

            for (int i = 0; i < _mapTiles[0].Length; i++)
            {
                _mapTiles[0][i].TileSprite = MapSpriteTile.GroundRight;
            }

            _mapTiles[0][7].TileSprite = MapSpriteTile.GroundLeftInnerCorner;
            _mapTiles[1][7].TileSprite = MapSpriteTile.GroundLeftEnd;

            _mapTiles[3][3].TileSprite = MapSpriteTile.PlatformLeftEdge;
            _mapTiles[4][3].TileSprite = MapSpriteTile.PlatformMiddle;
            _mapTiles[5][3].TileSprite = MapSpriteTile.PlatformMiddle;
            _mapTiles[6][3].TileSprite = MapSpriteTile.PlatformRightEdge;
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(BackgroundSprite, new Vector2(), new Rectangle(0, 0, 1920, 1080), Color.White, 0f, new Vector2(), 1f, SpriteEffects.None, 0f);
            _spriteBatch.End();

            _spriteBatch.Begin(transformMatrix: Game.Camera.GetTransformMatrix());
            for (int i = 0; i < _mapTiles.Length; i++)
            {
                for (int j = 0; j < _mapTiles[0].Length; j++)
                {
                    if (_mapTiles[i][j].TileSprite != MapSpriteTile.None)
                    {
                        _spriteBatch.Draw(MapSprite,
                            //ConvertUnits.ToDisplayUnits(_mapTiles[i][j].Body.Position),
                            ConvertUnits.ToDisplayUnits(_mapTiles[i][j].Body.Position.X, _mapTiles[i][j].Body.Position.Y),
                            new Rectangle(TileSize * (int)(_mapTiles[i][j].TileSprite), 0, TileSize, TileSize), 
                            Color.White,
                            _mapTiles[i][j].Body.Rotation,
                            _mapTiles[i][j].Origin,
                            1f,
                            SpriteEffects.None,
                            0.1f);
                    }
                }
            }

            _spriteBatch.End();
        }

        [Serializable]
        private struct MapTileFileInfo
        {
            public MapSpriteTile TileSprite;
        }

        public void SaveToFile()
        {
            var TileSprite = new MapTileFileInfo[_mapTiles.Length][];

            for (int i = 0; i < _mapTiles.Length; i++)
            {
                TileSprite[i] = new MapTileFileInfo[_mapTiles[0].Length];
            }

            for (int i = 0; i < _mapTiles.Length; i++)
            {
                for (int j = 0; j < _mapTiles[0].Length; j++)
                {
                    TileSprite[i][j].TileSprite = _mapTiles[i][j].TileSprite;
                }
            }


            
        }
    }
}
