using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TestMonogame;

namespace GameBaseHelpers
{
    public class GameScreen
    {
        protected MasterLoader Loader;
        protected Rectangle screenRect;
        protected String _type;
        public String Type {get { return _type;}}
        public Transition transition;
        public Color Tint;

        public GameScreen(Game game)
        {
            Loader = new MasterLoader(game);
            screenRect = new Rectangle(0,0,1920,1080);
            Tint = Color.White;
            transition = new Transition(this); //blank transition nothing happens
            _type = "base";
        }



        public virtual void LoadContent()
        {
            Loader.load();
        }

        public virtual void UnloadContent()
        {
            Loader.unload();
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {

        }

    }
}
