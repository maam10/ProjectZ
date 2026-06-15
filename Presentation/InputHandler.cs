using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProjectY.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectY.Presentation
{
    public class InputHandler
    {
        private readonly int _tileWidth = 120;
        private readonly int _tileHeight = 140;
        private readonly float _verticalSpacing = 105f;
        private MouseState _previousMouse;
        

        public void Update(World world, float zoomLevel, Vector2 cameraOffset)
        {
            var mouse = Mouse.GetState();

            var worldPosition = new Vector2(mouse.X, mouse.Y) - cameraOffset;
            worldPosition /= zoomLevel;

            var hexCoords = PixelToHex(worldPosition);
            int tileX = hexCoords.X;
            int tileY = hexCoords.Y;

            bool insideMap =
                tileX >= 0 && tileY >= 0 &&
                tileX < world.Map.Width &&
                tileY < world.Map.Height;

            bool leftClick =
                mouse.LeftButton == ButtonState.Pressed &&
                _previousMouse.LeftButton == ButtonState.Released;

            bool rightClick =
                mouse.RightButton == ButtonState.Pressed &&
                _previousMouse.RightButton == ButtonState.Released;

            if (insideMap)
            {
                bool canInteract = world.Visibility.IsVisible(tileX, tileY);

                if (leftClick)
                {
                    if (canInteract)
                    {
                        world.SelectedTile = world.Map.Tiles[tileX, tileY];
                        world.IsActionMenuOpen = true;
                    }
                }

                if (rightClick)
                {
                    world.IsActionMenuOpen = false;
                }
            }

            if (insideMap)
            {
                world.HoveredTile = world.Map.Tiles[tileX, tileY];
            }
            else
            {
                world.HoveredTile = null;
            }


            _previousMouse = mouse;

            var keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Escape))
            {
                world.IsActionMenuOpen = false;
            }
        }

        private Point PixelToHex(Vector2 pixel)
        {
            int baseRow = (int)MathF.Floor((pixel.Y / _verticalSpacing) + 0.5f);
            Point best = new Point(-1, -1);
            float bestDistance = float.MaxValue;

            for (int row = baseRow - 1; row <= baseRow + 1; row++)
            {
                float offsetX = (row % 2 == 1) ? _tileWidth * 0.5f : 0f;
                float colFloat = (pixel.X - offsetX) / _tileWidth;
                int baseCol = (int)MathF.Floor(colFloat + 0.5f);

                for (int col = baseCol - 1; col <= baseCol + 1; col++)
                {
                    float centerX = col * _tileWidth + offsetX + _tileWidth / 2f;
                    float centerY = row * _verticalSpacing + _tileHeight / 2f;
                    float dx = pixel.X - centerX;
                    float dy = pixel.Y - centerY;
                    float dist = dx * dx + dy * dy;

                    if (dist < bestDistance)
                    {
                        bestDistance = dist;
                        best = new Point(col, row);
                    }
                }
            }

            return best;
        }
    }
}
