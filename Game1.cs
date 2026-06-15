using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectY.Core;
using ProjectY.Presentation;
using ProjectY.Presentation.UI;
using Gum.Forms;
using Gum.Forms.Controls;
using MonoGameGum;

namespace ProjectY
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private MapRenderer _mapRenderer;
        private UnitRenderer _unitRenderer;
        private SpriteBatch _spriteBatch;

        private GameManager _gameManager;
        private SpriteFont _font;

        private InputHandler _inputHandler;
        private TileInfoPanel _tileInfoPanel;
        private TileActionMenu _tileActionMenu;
        private BottomInfoBar _bottomBar;
        private Texture2D _topBarTexture;
        private KeyboardState _previousKeyboard;
        private ActionBarRenderer _actionBarRenderer;
        
        // Zoom
        private float _zoomLevel = 0.5f;
        private const float _minZoom = 0.1f;
        private const float _maxZoom = 3.0f;
        private const float _zoomSpeed = 0.2f;
        private Vector2 _cameraOffset = Vector2.Zero;
        private int _previousScrollValue = 0;
        
        // Panning (arraste do mapa)
        private MouseState _previousMouse;
        private bool _wasActionMenuOpenLastFrame = false;

        public Game1()
        {
            Console.WriteLine("Starting ProjectY Version: 0.1-foundation");
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
         }

        protected override void Initialize()
        {
            _mapRenderer = new MapRenderer();
            _unitRenderer = new UnitRenderer();
            _gameManager = GameBootstrap.CreateGameManager();
            _inputHandler = new InputHandler();

            _tileInfoPanel = new TileInfoPanel(
                new Rectangle(600, 0, 260, 200) // ajuste conforme sua resolução
            );

            _tileActionMenu = new TileActionMenu(
                new Rectangle(0, 0, 260, 220)
            );

            _bottomBar = new BottomInfoBar();
            GumService.Default.Initialize(this, "GumProject/GumProject.gumx");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Fonts/NarrowFont");
            _mapRenderer.LoadContent(GraphicsDevice, Content);
            _unitRenderer.LoadContent(GraphicsDevice, Content);
            _tileInfoPanel.LoadContent(GraphicsDevice, Content);
            _tileActionMenu.LoadContent(GraphicsDevice, Content);
            _bottomBar.LoadContent(GraphicsDevice, Content);
           //_actionBarRenderer = new ActionBarRenderer(GraphicsDevice);
            _topBarTexture = new Texture2D(GraphicsDevice, 1, 1);
            _topBarTexture.SetData(new[] { Color.DarkSlateGray });
            
            Texture2D panelTexture = Content.Load<Texture2D>("UI/panel_rect");
            //Texture2D panelTexture = Content.Load<Texture2D>("UI/panel_01");
            _actionBarRenderer = new ActionBarRenderer(GraphicsDevice,panelTexture);

            UpdateActionMenuArea();
            CenterCameraOnColony();
        }

        private void UpdateActionMenuArea()
        {
            int menuWidth = 260;
            int menuHeight = 220;
            int margin = 20;

            int x = GraphicsDevice.Viewport.Width - menuWidth - margin;
            int y = (GraphicsDevice.Viewport.Height - menuHeight) / 2;

            _tileActionMenu.SetArea(new Rectangle(x, y, menuWidth, menuHeight));
        }

        private void CenterCameraOnColony()
        {
            if (_gameManager.World.Colonies.Count == 0) return;

            var colony = _gameManager.World.Colonies[0];
            const int tileWidth = 120;
            const int tileHeight = 140;
            const float verticalSpacing = tileHeight * 0.75f;

            float colonyPixelX = colony.MapPosition.X * tileWidth;
            if (colony.MapPosition.Y % 2 == 1)
            {
                colonyPixelX += tileWidth * 0.5f;
            }

            float colonyPixelY = colony.MapPosition.Y * verticalSpacing;

            colonyPixelX += tileWidth * 0.5f;
            colonyPixelY += tileHeight * 0.5f;

            colonyPixelX *= _zoomLevel;
            colonyPixelY *= _zoomLevel;

            _cameraOffset.X = (GraphicsDevice.Viewport.Width / 2f) - colonyPixelX;
            _cameraOffset.Y = (GraphicsDevice.Viewport.Height / 2f) - colonyPixelY - 25f;
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            var mouse = Mouse.GetState();

            if (keyboard.IsKeyDown(Keys.Space) &&
                _previousKeyboard.IsKeyUp(Keys.Space))
            {
                _gameManager.AdvanceTurn();
            }

            // Zoom com mouse wheel
            HandleZoom(mouse);
            
            // Panning - arrastar mapa com botão direito
            HandlePanning(mouse);

            _inputHandler.Update(_gameManager.World, _zoomLevel, _cameraOffset);

            if (_gameManager.World.IsActionMenuOpen && !_wasActionMenuOpenLastFrame)
            {
                _tileActionMenu.NotifyMenuOpenedThisFrame();
            }

            _tileActionMenu.Update(_gameManager.World, _gameManager);

            _previousKeyboard = keyboard;
            _previousMouse = mouse;
            _wasActionMenuOpenLastFrame = _gameManager.World.IsActionMenuOpen;
            GumService.Default.Update(this, gameTime);
            base.Update(gameTime);
        }

        private void HandleZoom(MouseState mouse)
        {
            // Obter scroll do mouse
            int currentScroll = mouse.ScrollWheelValue;
            int scrollDelta = currentScroll - _previousScrollValue;
            _previousScrollValue = currentScroll;

            if (scrollDelta != 0)
            {
                float oldZoom = _zoomLevel;
                _zoomLevel += (scrollDelta > 0 ? _zoomSpeed : -_zoomSpeed);
                _zoomLevel = MathHelper.Clamp(_zoomLevel, _minZoom, _maxZoom);

                // Manter o centro da tela no mesmo ponto durante o zoom
                Vector2 screenCenter = new Vector2(
                    GraphicsDevice.Viewport.Width / 2f,
                    GraphicsDevice.Viewport.Height / 2f
                );
                _cameraOffset -= screenCenter * (_zoomLevel - oldZoom) / oldZoom;
            }
        }
        
        private void HandlePanning(MouseState mouse)
        {
            // Arrastar mapa com botão direito do mouse
            if (mouse.RightButton == ButtonState.Pressed)
            {
                Vector2 mouseDelta = new Vector2(mouse.X - _previousMouse.X, mouse.Y - _previousMouse.Y);
                _cameraOffset += mouseDelta;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Usar transformação de câmera com zoom para o mapa
            var transform = Matrix.CreateScale(_zoomLevel, _zoomLevel, 1.0f) *
                           Matrix.CreateTranslation(_cameraOffset.X, _cameraOffset.Y, 0);

            _spriteBatch.Begin(transformMatrix: transform);
                _mapRenderer.Draw(_spriteBatch, _gameManager.World);
                _unitRenderer.Draw(_spriteBatch, _gameManager.World);
            _spriteBatch.End();

            // Desenhar menu fixo na tela (sem transformação de câmera)
            _spriteBatch.Begin();
                _tileActionMenu.Draw(_spriteBatch, _gameManager.World);
                _actionBarRenderer.Draw(_spriteBatch, GraphicsDevice.Viewport);
                
    
            
            var colony = _gameManager.World.Colonies[0];
            
            // Texto da barra superior
            string infoText = $"Date: {_gameManager.World.Date} | Pop: {colony.Population.Total} | F: {colony.Resources.Food} | W: {colony.Resources.Wood} | G: {colony.Resources.Gold}";
            
            // Medir tamanho do texto
            Vector2 textSize = _font.MeasureString(infoText);
            
            // Dimensões da barra
            int barHeight = 50;
            Rectangle barRect = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, barHeight);
            
            // Desenhar fundo da barra
            _spriteBatch.Draw(_topBarTexture, barRect, Color.White);
            
            // Centralizar texto na barra horizontalmente
            float textX = (GraphicsDevice.Viewport.Width - textSize.X) / 2f;
            float textY = (barHeight - textSize.Y) / 2f;
            
            _spriteBatch.DrawString(
                _font,
                infoText,
                new Vector2(textX, textY),
                Color.White
            );

            _bottomBar.Draw(
            _spriteBatch,
            _gameManager.World,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height
            );

            _spriteBatch.End();
GumService.Default.Draw();
            base.Draw(gameTime);
        }
    }

}
