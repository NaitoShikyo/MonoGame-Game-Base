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
    class Menu : GameScreen
    {
        public Dictionary<String, MenuButton> Buttons;
        protected String _MenuResult = "";
        public String MenuResult { get { return _MenuResult; } }


        public Menu(Game game,String BackImg)
            :base(game)
        {
            _type = "Menu";
            Loader.Add("backgroundimg", BackImg, fileType.Texture2D);
            Buttons = new Dictionary<string, MenuButton>();
        }

        public void AddButton(String ButtonName, MenuButton Button)
        {
            
            Buttons.Add(ButtonName, Button);
            Loader.Add(Button.LoaderFiles.ButtonDefault);
            Loader.Add(Button.LoaderFiles.ButtonDown);
            Loader.Add(Button.LoaderFiles.ButtonOver);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (KeyValuePair<String,MenuButton> butt in Buttons)
            {
                butt.Value.Update();
                butt.Value.ButtonColor = Color.Multiply(butt.Value.ButtonColor, Tint.ToVector4().Z);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(Loader.Textures["backgroundimg"], screenRect, Tint);
            foreach (KeyValuePair<String, MenuButton> item in Buttons)
            {
                String state = item.Value.CurrentMenuButtonState == MenuButtonState.Default ? "_dft" :
                    item.Value.CurrentMenuButtonState == MenuButtonState.MouseOver ? "_ovr" : "_dwn"; 
                Color tState = item.Value.CurrentMenuButtonState == MenuButtonState.Default ? item.Value.LoaderFiles.ButtonDefaultTint :
                    item.Value.CurrentMenuButtonState == MenuButtonState.MouseOver ? item.Value.LoaderFiles.ButtonOverTint : item.Value.LoaderFiles.ButtonDownTint; 
                item.Value.Draw(spriteBatch,Loader.Textures[item.Value.baseName+state],tState);
            }
        }

    }
}
