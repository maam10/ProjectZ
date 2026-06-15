using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectZ.Presentation.UI;

public class ActionBarRenderer
{
    private readonly Texture2D _pixel;
    private readonly Texture2D _panelTexture;
    private readonly List<ActionButton> _buttons = new();
    

    public ActionBarRenderer(GraphicsDevice graphicsDevice,Texture2D panelTexture)
    {
        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        _panelTexture = panelTexture;
    }

    public void Draw(SpriteBatch spriteBatch, Viewport viewport)
    {
        int barWidth = 420;
        int barHeight = 72;

        Rectangle backgroundRect = new Rectangle(
            viewport.Width / 2 - barWidth / 2,
            viewport.Height - 95,
            barWidth,
            barHeight);

        DrawPanel(spriteBatch, backgroundRect);
        _buttons.Clear();
        //DrawSlots(spriteBatch, backgroundRect);
        DrawButtons(spriteBatch, backgroundRect);
    }

    private void DrawPanel(SpriteBatch spriteBatch, Rectangle rect)
    {
        spriteBatch.Draw(_panelTexture,rect,Color.White);
    }

    private void DrawSlots(SpriteBatch spriteBatch, Rectangle rect)
    {
        int slotSize = 56;
        int spacing = 8;

        int totalWidth = (slotSize * 5) + (spacing * 4);

        int startX = rect.Center.X - totalWidth / 2;
        int y = rect.Center.Y - slotSize / 2;

        for (int i = 0; i < 5; i++)
        {
            Rectangle slotRect = new Rectangle(
                startX + i * (slotSize + spacing),
                y,
                slotSize,
                slotSize);

            spriteBatch.Draw(
                _pixel,
                slotRect,
                new Color(70, 70, 90));
        }
    }
    private void DrawButtons(SpriteBatch spriteBatch,Rectangle rect)
    {
        MouseState mouse = Mouse.GetState();

        int slotSize = 56;
        int spacing = 8;

        int totalWidth = (slotSize * 5) + (spacing * 4);

        int startX = rect.Center.X - totalWidth / 2;
        int y = rect.Center.Y - slotSize / 2;

        for (int i = 0; i < 5; i++)
        {
            Rectangle slotRect = new Rectangle(
                startX + i * (slotSize + spacing),
                y,
                slotSize,
                slotSize);

            ActionButton button = new()
            {
                Bounds = slotRect,
                IsHovered = slotRect.Contains(mouse.Position)
            };

            _buttons.Add(button);

            DrawButton(spriteBatch, button);
        }
    }
    private void DrawButton(SpriteBatch spriteBatch,ActionButton button)
    {
        Color color = button.IsHovered
            ? new Color(180, 180, 220)
            : new Color(110, 110, 140);

        Rectangle drawRect = button.Bounds;

        if (button.IsHovered)
        {
            drawRect.Inflate(2, 2);
        }

        spriteBatch.Draw(
            _panelTexture,
            drawRect,
            color);
    }
}