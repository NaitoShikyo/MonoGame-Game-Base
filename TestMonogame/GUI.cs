using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameBaseHelpers
{

    public class spriteFontHelper
    {
        public static String WordWrap(SpriteFont spriteFont, String text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }

            return sb.ToString();
        }
    }

    public interface IGuiItem
    {
        void Display();
        void Update();
    }

    public class Panel : DrawableGameComponent, IGuiItem
    {
        public Rectangle Bounds;
        public Texture2D Texture;
        public Point Location;
        public Color TintColor;


        public Panel(Game game,Texture2D text,Rectangle bounds, Point location)
            :base(game)
        {
            Texture = text;
            Bounds = bounds;
            Location = location;
            TintColor = Color.White;
        }

        public Panel(Game game, Color Tint, Rectangle bounds, Point location)
            : base(game)
        {
            Texture = new Texture2D(GraphicsDevice, 1, 1);
            Texture.SetData(new Color[] { Color.White });
            Bounds = bounds;
            Location = location;
            TintColor = Tint;
        }

        public Panel(Game game, Texture2D text, Color Tint, Rectangle bounds, Point location)
            : base(game)
        {
            Texture = text;
            Bounds = bounds;
            Location = location;
            TintColor = Tint;
        }

        public void Display()
        {
            
        }

        public void Update()
        {

        }
        
    }

    class GUI
    {
        List<IGuiItem> GuiItems;

        public void Display()
        {
            foreach (IGuiItem item in GuiItems)
            {
                item.Display();
            }
        }

        public void Update()
        {
            foreach (IGuiItem item in GuiItems)
            {
                item.Update();
            }
        }
    }
}