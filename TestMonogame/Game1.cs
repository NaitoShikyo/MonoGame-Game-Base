#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

using TestMonogame;
#endregion

namespace GameBaseHelpers
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MTGExample MTGE;
        Camera2D Cam;
        int ScreenIndex = 0;


        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true;
            Content.RootDirectory = "Content";
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            Options.Initialise(this, ref graphics);
            Options.Instance.SetMaxResolution();
            //Options.Instance.isFullscreen = true;
            Options.Instance.SetGameResolution(1280, 720);
            Cam = new Camera2D(Options.Instance);
            Cam.Zoom = 1f;
            Cam.Position = new Vector2(Options.Instance.VirtualWidth / 2, Options.Instance.VirtualHeight / 2);
            Options.Instance.ReinitView();
            Cam.RecalculateTransformationMatrices();
            ScreenManager.Instance.CurrentScreen = new splashscreen(this,"2");
            ScreenManager.Instance.CurrentScreen.LoadContent();
            ScreenManager.Instance.CurrentScreen.transition.Active = true;
            base.Initialize();
            MTGE = new MTGExample(this);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (ScreenManager.Instance.CurrentScreen.Type == "Splash")
            {
                if (((splashscreen)ScreenManager.Instance.CurrentScreen).Done)
                {
                    ScreenIndex++;
                    if (ScreenIndex == 1)
                    {
                        ScreenManager.Instance.ChangeGameScreen(new sotice.SoticeMenu(this));
                        ScreenManager.Instance.CurrentScreen.transition = new FadeInOutTransition(ScreenManager.Instance.CurrentScreen);
                        ScreenManager.Instance.CurrentScreen.transition.Active = true;
                        //ScreenManager.Instance.ChangeGameScreen(new splashscreen(this, "2"));
                    }
                    else if(ScreenIndex ==2)
                    {
                        ScreenManager.Instance.ChangeGameScreen(new splashscreen(this, "3"));
                    }
                    else
                    {
                        ScreenManager.Instance.ChangeGameScreen(new ExampleMenu(this,"1"));
                    }

                }
            }
            else if (ScreenManager.Instance.CurrentScreen.Type == "Menu")
            {
                if (((Menu)ScreenManager.Instance.CurrentScreen).MenuResult == "Start")
                {
                    Exit();
                }
                else if (((Menu)ScreenManager.Instance.CurrentScreen).MenuResult == "startClicked"){
                    ScreenManager.Instance.ChangeGameScreen(MTGE);
                }
            }
            ScreenManager.Instance.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Options.Instance.BeginDraw();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, null, Cam.GetViewTransformationMatrix());
            // TODO: Add your drawing code here
            ScreenManager.Instance.Draw(gameTime,spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
