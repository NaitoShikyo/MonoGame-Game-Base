using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameBaseHelpers
{
    class ScreenManager
    {
        public Vector2 Dimensions { private set; get; }
        public ContentManager Content { private set; get; }
        public GameScreen CurrentScreen;
        //public MasterLoader ContentLoader;

        #region singleton creation
        private static volatile ScreenManager instance;
        private static object syncRoot = new Object();

        private ScreenManager() { }

        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ScreenManager();
                    }
                }
                return instance;
            }
        }
        #endregion

        public void LoadContent()
        {
            CurrentScreen.LoadContent();
        }

        public void UnloadContent()
        {
            CurrentScreen.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            CurrentScreen.Update(gameTime);
        }

        public void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            CurrentScreen.Draw(gameTime,spriteBatch);
        }

        public GameScreen LoadGameScreen(GameScreen Gamescreen)
        {
            CurrentScreen.UnloadContent();
            CurrentScreen = Gamescreen;
            CurrentScreen.LoadContent();
            return CurrentScreen;
        }
        public GameScreen ChangeGameScreen(GameScreen Gamescreen)
        {
            LoadGameScreen(Gamescreen);
            CurrentScreen.transition.Active = true;
            return CurrentScreen;
        }

    }
        
}
