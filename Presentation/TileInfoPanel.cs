using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectY.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectY.Presentation
{
    public class TileInfoPanel
    {
        private readonly Rectangle _panelArea;
        private SpriteFont _font;
        private Texture2D _background;

        public TileInfoPanel(Rectangle panelArea)
        {
            _panelArea = panelArea;
        }

        public void LoadContent(GraphicsDevice device, ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/Font");

            _background = new Texture2D(device, 1, 1);
            _background.SetData(new[] { new Color(0, 0, 0, 150) });
        }

        public void Draw(SpriteBatch spriteBatch, World world)
        {
            if (world.SelectedTile == null)
                return;

            spriteBatch.Draw(_background, _panelArea, Color.White);

            var tile = world.SelectedTile;
            int y = _panelArea.Y + 10;

            DrawLine(spriteBatch, $"Tile [{tile.X}, {tile.Y}]", ref y);
            DrawLine(spriteBatch, $"Terrain: {tile.Terrain}", ref y);

            var colony = world.Colonies
                .FirstOrDefault(c => c.MapPosition.X == tile.X &&
                                     c.MapPosition.Y == tile.Y);

            if (colony != null)
            {
                y += 5;
                DrawLine(spriteBatch, "Colony:", ref y);
                DrawLine(spriteBatch, $"- {colony.Name}", ref y);
                DrawLine(spriteBatch, $"Pop: {colony.Population.Total}", ref y);
            }

            if (tile.HasBuilding)
            {
                DrawLine(spriteBatch, $"Building: {tile.Building.Type}", ref y);
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, string text, ref int y)
        {
            spriteBatch.DrawString(
                _font,
                text,
                new Vector2(_panelArea.X + 10, y),
                Color.White
            );
            y += 20;
        }
    }
}
