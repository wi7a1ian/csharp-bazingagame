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
using BazingaGame.Prefabs;
using MapFileModel;

namespace BazingaGame.GameMap
{
    public class Map : StatelessGameComponent
    {
        //private BazingaGame _game;
        private MapTileInfo[][] _mapTiles;
        private List<DecorationInfo> _mapDecorations;

        private List<Tuple<MapDecorationTileType, Texture2D>> _mapDecorationTiles;
        //private Map _map;
        private Texture2D MapSprite;
        private Texture2D BackgroundSprite;
        private SpriteBatch _spriteBatch;

        public const int TileSize = 128;
        public const int MapWidth = 200;
        public const int MapHeight = 100;

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
            _mapDecorationTiles = new List<Tuple<MapDecorationTileType, Texture2D>>();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);

            MapSprite = Game.Content.Load<Texture2D>("MapSprite");
            BackgroundSprite = Game.Content.Load<Texture2D>("Background");

            _mapDecorationTiles.Add(new Tuple<MapDecorationTileType, Texture2D>(MapDecorationTileType.Bush1, Game.Content.Load<Texture2D>("MapObjects/Bush (1)")));
            _mapDecorationTiles.Add(new Tuple<MapDecorationTileType, Texture2D>(MapDecorationTileType.Bush2, Game.Content.Load<Texture2D>("MapObjects/Bush (2)")));
            _mapDecorationTiles.Add(new Tuple<MapDecorationTileType, Texture2D>(MapDecorationTileType.Cactus1, Game.Content.Load<Texture2D>("MapObjects/Cactus (1)")));
            _mapDecorationTiles.Add(new Tuple<MapDecorationTileType, Texture2D>(MapDecorationTileType.Cactus2, Game.Content.Load<Texture2D>("MapObjects/Cactus (2)")));
            _mapDecorationTiles.Add(new Tuple<MapDecorationTileType, Texture2D>(MapDecorationTileType.Cactus3, Game.Content.Load<Texture2D>("MapObjects/Cactus (3)")));
            _mapDecorationTiles.Add(new Tuple<MapDecorationTileType, Texture2D>(MapDecorationTileType.Grass1, Game.Content.Load<Texture2D>("MapObjects/Grass (1)")));
            _mapDecorationTiles.Add(new Tuple<MapDecorationTileType, Texture2D>(MapDecorationTileType.Grass2, Game.Content.Load<Texture2D>("MapObjects/Grass (2)")));
            _mapDecorationTiles.Add(new Tuple<MapDecorationTileType, Texture2D>(MapDecorationTileType.Sign, Game.Content.Load<Texture2D>("MapObjects/Sign")));
            _mapDecorationTiles.Add(new Tuple<MapDecorationTileType, Texture2D>(MapDecorationTileType.SignArrow, Game.Content.Load<Texture2D>("MapObjects/SignArrow")));
            _mapDecorationTiles.Add(new Tuple<MapDecorationTileType, Texture2D>(MapDecorationTileType.Skeleton, Game.Content.Load<Texture2D>("MapObjects/Skeleton")));
            _mapDecorationTiles.Add(new Tuple<MapDecorationTileType, Texture2D>(MapDecorationTileType.Stone, Game.Content.Load<Texture2D>("MapObjects/Stone")));
            _mapDecorationTiles.Add(new Tuple<MapDecorationTileType, Texture2D>(MapDecorationTileType.StoneBlock, Game.Content.Load<Texture2D>("MapObjects/StoneBlock")));
            _mapDecorationTiles.Add(new Tuple<MapDecorationTileType, Texture2D>(MapDecorationTileType.Tree, Game.Content.Load<Texture2D>("MapObjects/Tree")));

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

            LoadMap("Map\\Levels\\first.baz");

            CreateBodies(Game as BazingaGame);

            base.Initialize();
        }

		private void LoadMap(string mapFileName)
		{
			var serializer = new BinaryFormatter();

			// read whole map info
			var mapFileInfo = new MapFileInfo();
			using (var stream = File.OpenRead(mapFileName))
			{
				mapFileInfo = (MapFileInfo)serializer.Deserialize(stream);
			}

			// create array of tiles
			MapSpriteTile[][] mapSpriteTiles = new MapSpriteTile[MapWidth][];
			for (int i = 0; i < _mapTiles.Length; i++)
            {
                mapSpriteTiles[i] = new MapSpriteTile[MapHeight];
            }
			// map readed from map info ground/none tiles to sprite tiles
			MapModel.MapTilesToSprites(mapFileInfo.TileType, mapSpriteTiles);

            _mapDecorations = mapFileInfo.Decorations;

            // assign mapped sprite tile to _mapTiles.TileSprite
            for (int i = 0; i < _mapTiles.Length; i++)
			{
				for (int j = 0; j < _mapTiles[0].Length; j++)
				{
					_mapTiles[i][j].TileSprite = mapSpriteTiles[i][j];
				}
			}
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
                        _mapTiles[i][j].Body.CollisionCategories = BazingaCollisionGroups.Ground;
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

            foreach (var decoration in _mapDecorations)
            {
                var texture = _mapDecorationTiles.Where(p => p.Item1 == decoration.DecorationType).First().Item2;
                _spriteBatch.Draw(texture, new Vector2(decoration.X, decoration.Y), Color.White);
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
