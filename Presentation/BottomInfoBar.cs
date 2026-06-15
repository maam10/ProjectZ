using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectZ.Core;
using ProjectZ.Core.Buildings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectZ.Presentation
{
    public class BottomInfoBar
    {
        private SpriteFont _font;
        private Texture2D _background;

        private int _height = 20;

        public void LoadContent(GraphicsDevice device, ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/UIFont");

            _background = new Texture2D(device, 1, 1);
            _background.SetData(new[] { new Color(0, 0, 0, 180) });
        }

        public void Draw(SpriteBatch spriteBatch, World world, int screenWidth, int screenHeight)
        {
            var rect = new Rectangle(
                0,
                screenHeight - _height,
                screenWidth,
                _height
            );

            spriteBatch.Draw(_background, rect, Color.White);

            if (world.HoveredTile == null)
                return;       

            var tile = world.HoveredTile;
            var visibility = world.Visibility.GetState(tile.X, tile.Y);

            if (visibility == VisibilityState.Hidden)
            {
                spriteBatch.DrawString(
                    _font,
                    $"Pos: [{tile.X},{tile.Y}]   Unknown",
                    new Vector2(10, screenHeight - _height + 5),
                    Color.Gray
                );
                return;
            }

            string text =
                $"Pos: [{tile.X},{tile.Y}]   " +
                $"Terreno: {tile.Terrain}";

            var colony = world.GetColonyAt(tile.X, tile.Y);

            if (colony != null && visibility == VisibilityState.Visible)
            {
                text =
                    $"   Colony: {colony.Name}" +
                    $"   Pop: {colony.Population.Total}" +
                    $"   Age: {colony.Population.AverageAgeYears:0.0}" +
                    $"   Growth: {colony.Population.GrowthRate:P0}" +
                    $"   F: {colony.Resources.Food}" +
                    $"   W: {colony.Resources.Wood}" +
                    $"   G: {colony.Resources.Gold}";
            }
            else if (tile.HasBuilding && visibility == VisibilityState.Visible)
            {
                var building = tile.Building;
                var definition = BuildingCatalog.Get(building.Type);

                text =
                    $"Pos: [{tile.X},{tile.Y}]   " +
                    $"Building: {definition.DisplayName}" +
                    FormatProduction(definition.FoodProduction, "Food") +
                    FormatProduction(definition.WoodProduction, "Wood");
            }
            spriteBatch.DrawString(
                _font,
                text,
                new Vector2(10, screenHeight - _height + 5),
                Color.White
            );

        }

        private static string FormatProduction(int amount, string resource)
        {
            return amount > 0
                ? $"   +{amount} {resource}/turn"
                : string.Empty;
        }
    }
}
