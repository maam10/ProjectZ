
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectY.Core.Buildings;
using ProjectY.Core.Commands;
using ProjectY.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ProjectY.Presentation
{
    public class TileActionMenu
    {
        private Rectangle _area;
        private SpriteFont _font;
        private Texture2D _background;

        private List<GameCommand> _commands = new();
        //private GameCommand _hovered;
        private MouseState _previousMouse;
        private bool _ignoreNextClick;

        public TileActionMenu(Rectangle area)
        {
            _area = area;
        }

        public void SetArea(Rectangle area)
        {
            _area = area;
        }

        public void NotifyMenuOpenedThisFrame()
        {
            _ignoreNextClick = true;
        }

        public void LoadContent(GraphicsDevice device, ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/Font");

            _background = new Texture2D(device, 1, 1);
            _background.SetData(new[] { new Color(0, 0, 0, 180) });
        }

        public void Update(World world, GameManager gameManager)
        {
            if (!world.IsActionMenuOpen || world.SelectedTile == null)
                return;

            var mouse = Mouse.GetState();

            if (_ignoreNextClick)
            {
                _ignoreNextClick = false;
                _previousMouse = mouse;
                return;
            }

            BuildCommands(world);

            int y = _area.Y + 10;

            bool leftClick =
                mouse.LeftButton == ButtonState.Pressed &&
                _previousMouse.LeftButton == ButtonState.Released;

            foreach (var cmd in _commands)
            {
                var rect = new Rectangle(_area.X + 10, y, _area.Width - 20, 24);

                if (rect.Contains(mouse.Position) &&
                    leftClick &&
                    cmd.CanExecute(world))
                {
                    gameManager.ExecuteCommand(cmd);
                    world.IsActionMenuOpen = false; // fecha após ação
                    _previousMouse = mouse;
                    return;
                }

                y += 30;
            }
            
            // clique fora do menu fecha
            if (world.IsActionMenuOpen && leftClick && !_area.Contains(mouse.Position))
            {
                world.IsActionMenuOpen = false;
            }

            _previousMouse = mouse;

        }

        public void Draw(SpriteBatch spriteBatch, World world)
        {
            if (!world.IsActionMenuOpen || world.SelectedTile == null)
                return;

            spriteBatch.Draw(_background, _area, Color.White);

            int y = _area.Y + 10;

            foreach (var cmd in _commands)
            {
                var color = cmd.CanExecute(world)
                    ? Color.White
                    : Color.Gray;

                spriteBatch.DrawString(
                    _font,
                    "- " + cmd.Label,
                    new Vector2(_area.X + 10, y),
                    color
                );

                y += 30;
            }
        }

        private void BuildCommands(World world)
        {
            _commands.Clear();

            var tile = world.SelectedTile;

            foreach (var definition in BuildingCatalog.All)
            {
                _commands.Add(new BuildBuildingCommand(tile, definition));
            }
            // depois: explorar, estrada, posto comercial etc.
        }
    }
}
