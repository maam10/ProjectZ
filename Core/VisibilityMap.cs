using System;

namespace ProjectZ.Core
{
    public class VisibilityMap
    {
        private readonly VisibilityState[,] _tiles;

        public int Width { get; }
        public int Height { get; }

        public VisibilityMap(int width, int height)
        {
            Width = width;
            Height = height;
            _tiles = new VisibilityState[width, height];
        }

        public VisibilityState GetState(int x, int y)
        {
            if (!IsInside(x, y))
            {
                return VisibilityState.Hidden;
            }

            return _tiles[x, y];
        }

        public bool IsVisible(int x, int y)
        {
            return GetState(x, y) == VisibilityState.Visible;
        }

        public bool IsKnown(int x, int y)
        {
            return GetState(x, y) != VisibilityState.Hidden;
        }

        public void ResetVisibleToExplored()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if (_tiles[x, y] == VisibilityState.Visible)
                    {
                        _tiles[x, y] = VisibilityState.Explored;
                    }
                }
        }

        public void RevealAround(int centerX, int centerY, int radius)
        {
            int radiusSquared = radius * radius;

            for (int x = centerX - radius; x <= centerX + radius; x++)
                for (int y = centerY - radius; y <= centerY + radius; y++)
                {
                    if (!IsInside(x, y))
                    {
                        continue;
                    }

                    int dx = x - centerX;
                    int dy = y - centerY;

                    if ((dx * dx) + (dy * dy) <= radiusSquared)
                    {
                        _tiles[x, y] = VisibilityState.Visible;
                    }
                }
        }

        private bool IsInside(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }
    }
}
