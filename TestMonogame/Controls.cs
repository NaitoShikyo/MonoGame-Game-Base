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
    #region Eventstuff
           
        #endregion

    struct ButtonLoaderStruct{
        public LoadObject ButtonDefault;
        public Color ButtonDefaultTint;
        public LoadObject ButtonOver;
        public Color ButtonOverTint;
        public LoadObject ButtonDown;
        public Color ButtonDownTint;
    }

    enum MenuButtonState
    {
        Default, MouseOver, MouseDown, Disabled
    }


    class MenuButton
    {
        public ButtonLoaderStruct LoaderFiles;
        public Rectangle Bounds; // Should work
        public Point Position;
        public String Text;
        private String _baseName;
        public String baseName { get { return _baseName; } }
        public Boolean Active { get; set; }
        public Vector2 StringPosition;
        private Vector2 StringSize;
        public String ButtonTexture;
        private ButtonState oldState;
        public Color ButtonColor;
        public ButtonState CurrentState;
        public MenuButtonState CurrentMenuButtonState;

        public delegate void onClickedEventHandler(object sender, EventArgs e);
        public event onClickedEventHandler Clicked;


        protected virtual void onClicked(EventArgs e)
        {
            if(Clicked != null)
                Clicked(this,e);
        }
        
        public MenuButton(Rectangle bounds,String basename, String text, ButtonLoaderStruct Btnl)
        {
            _baseName = basename;
            Bounds = bounds;
            Position = new Point(Bounds.Left, Bounds.Right);
            LoaderFiles = Btnl;
            Active = true;
        }

        public Boolean Update() // Returns if clicked
        {
            MouseState mouseState = Mouse.GetState();
            if (Active)
                if (Bounds.Contains(ScaledMouse.ScaledMousePosition()))
                {
                    if (mouseState.LeftButton == ButtonState.Pressed) // MouseDown
                    {
                        CurrentMenuButtonState = MenuButtonState.MouseDown;
                    }
                    else if (mouseState.LeftButton == ButtonState.Released && oldState == ButtonState.Pressed) //Mouse Up
                    {
                        CurrentMenuButtonState = MenuButtonState.Default;
                        oldState = mouseState.LeftButton;
                        onClicked(EventArgs.Empty);
                        return true;
                    }
                    else // Mouse Over
                    {
                        CurrentMenuButtonState = MenuButtonState.MouseOver;

                    }

                }
                else
                {
                    CurrentMenuButtonState = MenuButtonState.Default;

                }
            else
                CurrentMenuButtonState = MenuButtonState.Disabled;
            oldState = mouseState.LeftButton;
            return false;
        }


        public void Draw(SpriteBatch spriteBatch,Texture2D texture,Color Tint)
        {
            spriteBatch.Draw(texture,Bounds,Tint);
        }


    }

  

    public interface IControl
    {
        void Update();
        void Draw(SpriteBatch SB);
    }
}

