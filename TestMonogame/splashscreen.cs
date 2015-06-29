using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameBaseHelpers;

namespace TestMonogame
{
    public class splashscreen : GameScreen
    {
        public Boolean Done = false;
        public splashscreen(Game game,String splashImg)
            :base(game)
        {
            Loader.Add("splashysplash", splashImg, fileType.Texture2D);
            Tint = Color.TransparentBlack;
            transition = new FadeInOutTransition(this);
            _type = "Splash";
        }

        public override void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Loader.Textures["splashysplash"], screenRect, Tint);
            base.Draw(gameTime,spriteBatch);
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
            transition.Update(gameTime);
            if (transition.Active == false)
            {
                Done = true;
            }
            base.Update(gameTime);
        }


    }
}
