using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameBaseHelpers;
using Microsoft.Xna.Framework;

namespace TestMonogame
{
    class MTGExample : GameScreen 
    {
        Sprite CardExampleSprite;
        Sprite CardExampleSprite2;
        public MTGExample(Game game)
            :base(game)
        {
            //Loads norin unless another card has been placed in the cache
            Loader.Textures.loadTextureFromWeb(game.GraphicsDevice,"Emrakul","CardImgs","http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=397905&type=card");
            Loader.Textures.loadTextureFromWeb(game.GraphicsDevice, "Norin", "CardImgs", "http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=113512&type=card");
        }

        public override void LoadContent()
        {
            base.LoadContent();
            CardExampleSprite = new Sprite(Loader.Textures["Norin"]);
            CardExampleSprite.Bounds = new Rectangle(250, 250, 223, 311);
            CardExampleSprite2 = new Sprite(Loader.Textures["Emrakul"]);
            CardExampleSprite2.Bounds = new Rectangle(850, 250, 223, 311);
            CardExampleSprite2.RotationD = 90f;
        }

        public override void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            CardExampleSprite.Draw(gameTime,spriteBatch);
            CardExampleSprite2.Draw(gameTime, spriteBatch);
        }
    }
}
