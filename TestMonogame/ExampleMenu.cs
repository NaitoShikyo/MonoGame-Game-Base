using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameBaseHelpers;
namespace TestMonogame
{
    class ExampleMenu : Menu
    {
        public ExampleMenu(Game game, String BackImg)
            :base(game,BackImg)
        {
            ButtonLoaderStruct tmp = new ButtonLoaderStruct
            {
                ButtonDefault = new LoadObject
                {
                    FileName  = "2",
                    FileType = fileType.Texture2D,
                    InternalName = "btn_dft"
                },
                ButtonDown = new LoadObject
                {
                    FileName = "3",
                    FileType = fileType.Texture2D,
                    InternalName = "btn_dwn"
                },
                ButtonOver = new LoadObject
                {
                    FileName = "1",
                    FileType = fileType.Texture2D,
                    InternalName = "btn_ovr"
                },
            };
            MenuButton StartButton = new MenuButton(new Rectangle(60, 600, 500, 64),"btn", "Example", tmp);
            StartButton.Clicked += StartButton_Clicked;
            AddButton("start", StartButton);
        }

        private void StartButton_Clicked(object sender, EventArgs e)
        {
            _MenuResult = "Start";
        }



        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
