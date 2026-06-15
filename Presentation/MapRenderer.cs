using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectZ.Core;
using ProjectZ.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectZ.Presentation
{
    public class MapRenderer
    {
        private readonly int _tileWidth = 120;
        private readonly int _tileHeight = 140;
        //private readonly float _hexSize = 70f; // radius for pointy-top hex
        private readonly float _verticalSpacing = 105f; // 0.75 * tileHeight

        private Texture2D _grass;
        private Texture2D _forest;
        private Texture2D _mountain;
        private Texture2D _water;
        private Texture2D _colonyIcon;
        private Texture2D _highlight;
        private Texture2D _farm;
        private Texture2D _woodCamp;
        private Texture2D _fogOverlay;
        private Texture2D _selectionBorder;

        public void LoadContent(GraphicsDevice device, ContentManager content)
        {
            try 
            {
                _grass = content.Load<Texture2D>("HexTiles/Terrain/Grass/grass_05");
                _forest = content.Load<Texture2D>("HexTiles/Terrain/Grass/grass_12");
                _mountain = content.Load<Texture2D>("HexTiles/Terrain/Grass/grass_14");
                _water = content.Load<Texture2D>("HexTiles/Terrain/Water/water_01");
                _colonyIcon = content.Load<Texture2D>("HexTiles/Medieval/medieval_church");
                
                _farm = content.Load<Texture2D>("HexTiles/Medieval/medieval_farm");
                _woodCamp = content.Load<Texture2D>("HexTiles/Medieval/medieval_lumber");

                _highlight = content.Load<Texture2D>("HexTiles/Terrain/Grass/grass_05");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading textures: " + ex.Message);
                _grass = CreateFallbackTexture(device, Color.Green);
                _forest = CreateFallbackTexture(device, Color.DarkGreen);
                _mountain = CreateFallbackTexture(device, Color.Gray);
                _water = CreateFallbackTexture(device, Color.Blue);
                _colonyIcon = CreateFallbackTexture(device, Color.LightGray);
                _farm = CreateFallbackTexture(device, Color.Gold);
                _woodCamp = CreateFallbackTexture(device, Color.Brown);
                _fogOverlay = CreateFallbackTexture(device, Color.White);

                _highlight = CreateFallbackTexture(device, Color.Yellow);
                
            }
           //_woodCamp = new Texture2D(device, 1, 1);
           //     _woodCamp.SetData(new[] { Color.SaddleBrown });

        _fogOverlay = new Texture2D(device, 1, 1);
        _fogOverlay.SetData(new[] { Color.White });
            _selectionBorder = new Texture2D(device, 1, 1);
            _selectionBorder.SetData(new[] { Color.White });
        }
        private Texture2D CreateFallbackTexture(GraphicsDevice device, Color color)
        {
            var texture = new Texture2D(device, 64, 64); // Tamanho padrão para fallbacks
            var data = new Color[64 * 64];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = color;
            }
            texture.SetData(data);
            return texture;
        }

        public void Draw(SpriteBatch spriteBatch, World world)
        {
            for (int x = 0; x < world.Map.Width; x++)
                for (int y = 0; y < world.Map.Height; y++)
                {
                    var tile = world.Map.Tiles[x, y];
                    var texture = GetTexture(tile.Terrain);
                    var position = GetHexPixelPosition(x, y);

                    spriteBatch.Draw(
                        texture,
                        new Rectangle(
                            (int)position.X,
                            (int)position.Y,
                            _tileWidth,
                            _tileHeight),
                        Color.White
                    );
                }
            DrawSelection(spriteBatch, world);
            DrawColonies(spriteBatch, world);
            DrawBuildings(spriteBatch, world);
            DrawFogOfWar(spriteBatch, world);
        }

        private void DrawColonies(SpriteBatch spriteBatch, World world)
        {
            foreach (var colony in world.Colonies)
            {
                if (!world.Visibility.IsVisible(colony.MapPosition.X, colony.MapPosition.Y))
                {
                    continue;
                }

                var position = GetHexPixelPosition(colony.MapPosition.X, colony.MapPosition.Y);
                var rect = new Rectangle(
                    (int)position.X + 12,
                    (int)position.Y + 20,
                    _tileWidth - 24,
                    _tileHeight - 40
                );

                spriteBatch.Draw(_colonyIcon, rect, Color.White);
            }
        }

        private void DrawSelection(SpriteBatch spriteBatch, World world)
        {
            var tile = world.SelectedTile;
            if (tile == null) return;
            if (!world.Visibility.IsVisible(tile.X, tile.Y)) return;

            var position = GetHexPixelPosition(tile.X, tile.Y);
            var center = new Vector2(position.X + _tileWidth / 2f, position.Y + _tileHeight / 2f);
            
            // Para hexágono pointy-top: raio = altura/2, laterais = raio * sqrt(3)/2
            float hexRadius = _tileHeight / 2f;
            float hexWidthHalf = hexRadius * MathF.Sqrt(3) / 2f;

            var hexPoints = new[]
            {
                new Vector2(center.X, center.Y - hexRadius),                    // Topo
                new Vector2(center.X + hexWidthHalf, center.Y - hexRadius / 2f), // Topo-direita
                new Vector2(center.X + hexWidthHalf, center.Y + hexRadius / 2f), // Fundo-direita
                new Vector2(center.X, center.Y + hexRadius),                    // Fundo
                new Vector2(center.X - hexWidthHalf, center.Y + hexRadius / 2f), // Fundo-esquerda
                new Vector2(center.X - hexWidthHalf, center.Y - hexRadius / 2f)  // Topo-esquerda
            };

            DrawHexBorder(spriteBatch, _selectionBorder, hexPoints, Color.Yellow * 0.85f, 4f);
        }

        private void DrawBuildings(SpriteBatch spriteBatch, World world)
        {
            for (int x = 0; x < world.Map.Width; x++)
                for (int y = 0; y < world.Map.Height; y++)
                {
                    var tile = world.Map.Tiles[x, y];
                    if (!tile.HasBuilding) continue;
                    if (!world.Visibility.IsVisible(x, y)) continue;

                    var texture = GetBuildingTexture(tile.Building);
                    if (texture == null) continue;

                    var position = GetHexPixelPosition(x, y);
                    spriteBatch.Draw(
                        texture,
                        new Rectangle(
                            (int)position.X + 12,
                            (int)position.Y + 20,
                            _tileWidth - 24,
                            _tileHeight - 40),
                        Color.White
                    );
                }
        }

        private void DrawFogOfWar(SpriteBatch spriteBatch, World world)
        {
            for (int x = 0; x < world.Map.Width; x++)
                for (int y = 0; y < world.Map.Height; y++)
                {
                    var visibility = world.Visibility.GetState(x, y);

                    if (visibility == VisibilityState.Visible)
                    {
                        continue;
                    }

                    var color = visibility switch
                    {
                        VisibilityState.Explored => Color.Black * 0.55f,
                        _ => Color.Black * 0.95f
                    };

                    var position = GetHexPixelPosition(x, y);
                    spriteBatch.Draw(
                        _highlight,
                        new Rectangle(
                            (int)position.X,
                            (int)position.Y,
                            _tileWidth,
                            _tileHeight),
                        color
                    );
                }
        }

        private Texture2D GetBuildingTexture(Building building)
        {
            return building.Type switch
            {
                BuildingType.Farm => _farm,
                BuildingType.WoodCamp => _woodCamp,
                _ => null
            };
        }

        private Texture2D GetTexture(TerrainType terrain)
        {
            return terrain switch
            {
                TerrainType.Grass => _grass,
                TerrainType.Forest => _forest,
                TerrainType.Mountain => _mountain,
                TerrainType.Water => _water,
                _ => _grass
            };
        }

        private void DrawHexBorder(SpriteBatch spriteBatch, Texture2D pixel, Vector2[] points, Color color, float thickness)
        {
            for (int i = 0; i < points.Length; i++)
            {
                var start = points[i];
                var end = points[(i + 1) % points.Length];
                DrawLine(spriteBatch, pixel, start, end, color, thickness);
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, Texture2D pixel, Vector2 start, Vector2 end, Color color, float thickness)
        {
            var edge = end - start;
            float angle = MathF.Atan2(edge.Y, edge.X);
            float length = edge.Length();

            spriteBatch.Draw(pixel, start, null, color, angle, Vector2.Zero, new Vector2(length, thickness), SpriteEffects.None, 0f);
        }

        private Vector2 GetHexPixelPosition(int x, int y)
        {
            float offsetX = (y % 2 == 1) ? _tileWidth * 0.5f : 0f;
            float posX = x * _tileWidth + offsetX;
            float posY = y * _verticalSpacing;
            return new Vector2(posX, posY);
        }
    }
}

