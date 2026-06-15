using ProjectY.Core;
using ProjectY.Domain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace ProjectY.Presentation
{
    public class UnitRenderer
    {
        private Texture2D _scoutTexture;
        private readonly int _tileWidth = 120;
        private readonly int _tileHeight = 140;
        private readonly float _verticalSpacing = 105f;

        public void LoadContent(GraphicsDevice device, ContentManager content)
        {
            try
            {
                _scoutTexture = content.Load<Texture2D>("Units/scout");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading scout texture: " + ex.Message);
                _scoutTexture = CreateFallbackTexture(device, Color.Gray);
            }
        }

        public void Draw(SpriteBatch spriteBatch, World world)
        {
            foreach (var unit in world.Units)
            {
                Texture2D texture = GetTexture(unit);

                var position = GetHexPixelPosition(unit.Position.X, unit.Position.Y);

                spriteBatch.Draw(
                    texture,
                    new Rectangle(
                        (int)position.X + 20,
                        (int)position.Y + 30,
                        _tileWidth - 40,
                        _tileHeight - 60),
                    Color.White
                );
            }
        }

        private Texture2D GetTexture(Unit unit)
        {
            return unit.Type switch
            {
                UnitType.Scout => _scoutTexture,
                _ => _scoutTexture
            };
        }

        private Vector2 GetHexPixelPosition(int x, int y)
        {
            float offsetX = (y % 2 == 1) ? _tileWidth * 0.5f : 0f;
            float posX = x * _tileWidth + offsetX;
            float posY = y * _verticalSpacing;
            return new Vector2(posX, posY);
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
    }
}