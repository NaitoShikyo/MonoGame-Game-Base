using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameBaseHelpers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace sotice
{

    class SoticeMenu : Menu
    {
        public SoticeMenu(Game game)
            : base(game, "Textures/ALiftInPink")
        {
            ButtonLoaderStruct bls = new ButtonLoaderStruct
            {
                ButtonDefault = new LoadObject
                {
                    FileName = "Textures/HortofGeld",
                    FileType = fileType.Texture2D,
                    InternalName = "Start_dft"
                },
                ButtonDefaultTint = Color.White,
                ButtonDown = new LoadObject
                {
                    FileType = fileType.Texture2D,
                    FileName = "Textures/BorkenHeartofGold",
                    InternalName = "Start_dwn",
                },
                ButtonDownTint = Color.Black,
                ButtonOver = new LoadObject
                {
                    FileName = "Textures/BorkenHeartofGold",
                    FileType = fileType.Texture2D,
                    InternalName = "Start_ovr"
                },
                ButtonOverTint = Color.White
            };
            MenuButton mb = new MenuButton(new Rectangle(700, 500, 444, 532),"Start","Start",bls);
            mb.Clicked += mb_Clicked;
            AddButton("StartBtn", mb);
        }

        void mb_Clicked(object sender, EventArgs e)
        {
            _MenuResult = "startClicked";
        }
    }

    class SoticeGameScreen1 :GameScreen
    {
        public SoticeGameScreen1(Game game)
            : base(game)
        {
            Loader.Add("background", "Textures/ImBringLiftingBack", fileType.Texture2D);
            Loader.Add("MalePlayer", "Textures/MrMan", fileType.Texture2D);
            Loader.Add("FemalePlayer", "Textures/MrsWoman", fileType.Texture2D);
            Loader.Add("TextBox", "Textures/TextBoxDateSim", fileType.Texture2D);
            Loader.Add("text", "Font/font", fileType.SpriteFont);
         
        }

        public override void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(Loader.Textures["background"], screenRect, Tint);
            spriteBatch.Draw(Loader.Textures["MalePlayer"], new Rectangle(150, 200, 736, 892),Tint);
            spriteBatch.Draw(Loader.Textures["FemalePlayer"], new Rectangle(1034, 200, 736, 892),Tint);
            spriteBatch.Draw(Loader.Textures["TextBox"], new Rectangle(100, 700, 1720, 360), Tint);
            Loader.SpriteFonts["text"].LineSpacing = 44;
            spriteBatch.DrawString(Loader.SpriteFonts["text"], spriteFontHelper.WordWrap(Loader.SpriteFonts["text"], "You:\nHello Friend Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum ", 1600f), new Vector2(120, 700), Color.White);
        }
    }

    class SoticeFightScreen : GameScreen
    {
        Sprite playerSprite;
        Sprite cpuSprite;
        Vector2 playerVel,playerAcc;
        Vector2 cpuVel,cpuAcc;
        Boolean playerFlip = false;
        ninepatchSprite TestSprite;
        AnimatedSprite AS;
        List<projectile> sp;
        MouseState pms;
        public SoticeFightScreen(Game game)
            : base(game)
        {
            Loader.Add("background", "Textures/FightSceneBackground", fileType.Texture2D);
            Loader.Add("man", "Textures/playerTexture", fileType.Texture2D);
            Loader.Add("woman","Textures/EnemyTexture",fileType.Texture2D);
            Loader.Add("projRed", "Textures/ProjectileTexture", fileType.Texture2D);
            Loader.Add("projBlue","Textures/ProjectileTextureBlue",fileType.Texture2D);
            Loader.Add("floor", "Textures/firstRoundEnviroment", fileType.Texture2D);
            Loader.Add("rr", "Textures/nn", fileType.Texture2D);
            Loader.Add("walker", "Textures/walk", fileType.Texture2D);
            playerVel = Vector2.Zero;
            cpuVel = Vector2.Zero;
            playerAcc = Vector2.Zero;
            cpuAcc = Vector2.Zero;
            sp = new List<projectile>(50);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            playerSprite = new Sprite(Loader.Textures["man"],new Vector2(420,420));
            cpuSprite = new Sprite(Loader.Textures["woman"],new Vector2(40,40));
            TestSprite = new ninepatchSprite(Loader.Textures["rr"], new Rectangle(32,32,256-64,256-64));
            playerSprite.Width *= 1.5f;
            playerSprite.Height *= 1.5f;
            cpuSprite.Width *= 1.5f;
            cpuSprite.Height *= 1.5f;
            TestSprite.Height = 512;
            TestSprite.Width = 512;
            TestSprite.Position = new Vector2(500, 500);
            AS = new AnimatedSprite(Loader.Textures["walker"], new Rectangle(0, 0, 612 / 6, 148), 6,10);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();
            AS.Update(gameTime);
            //Options.Instance.ScaleMouseToScreenCoordinates();
            base.Update(gameTime);
            if (ms.LeftButton == ButtonState.Pressed && pms.LeftButton == ButtonState.Released)
            {
                sp.Add(new projectile(Loader.Textures["projBlue"],playerSprite.Position,-(playerSprite.Position-Options.Instance.ScaleMouseToScreenCoordinates(ms.Position.ToVector2()))*0.04f,Vector2.Zero));
                pms = ms;
                AS.Active = !AS.Active;
            }
            else
            {
                pms = ms;
            }
            if (playerSprite.Position.Y >= 1030 - playerSprite.Height)
            {
                if(ks.IsKeyDown(Keys.Space)){
                    playerVel.Y = -15f;
                    playerAcc.Y = 0.004f * gameTime.ElapsedGameTime.Milliseconds;
                
                }
                else
                {
                    playerAcc.Y = 0;
                    playerVel.Y = 0;
                }
                playerSprite.Position.Y = 1030 - playerSprite.Height;

            }
            else
            {
                playerAcc.Y += 0.004f * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (ks.IsKeyDown(Keys.N))
            {
                Options.Instance.SetMaxResolution();
            }
            if (ks.IsKeyDown(Keys.A))
            {
                if (playerSprite.Position.X > 0 )
                {
                    playerVel.X = -0.5f * gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    playerAcc.X = 0;
                    playerSprite.Position.X = 0;
                }
                if (playerSprite is AnimatedSprite)
                {
                    ((AnimatedSprite)playerSprite).Active = true;
                }
                playerFlip = true;
            }
            else if (ks.IsKeyDown(Keys.D))
            {
                if(playerSprite.Position.X < 1920-playerSprite.Width){
                    playerVel.X = 0.5f * gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    playerAcc.X = 0;
                    playerSprite.Position.X = 1920 - playerSprite.Width;
                }
                if (playerSprite is AnimatedSprite)
                {
                    ((AnimatedSprite)playerSprite).Active = true;
                }
                playerFlip = false;
            }
            else if (ks.IsKeyDown(Keys.H))
            {
                playerSprite = new AnimatedSprite(Loader.Textures["walker"],new Rectangle(200,200,102,148),6,7);
            }
            else
            {
                if (playerSprite is AnimatedSprite)
                {
                    ((AnimatedSprite)playerSprite).Active = false;
                }
                playerVel.X = 0;
            }
            playerVel += playerAcc;
            playerSprite.Position += playerVel;
            playerSprite.Position.Y = playerSprite.Position.Y > 1030 - playerSprite.Height ? 1030 - playerSprite.Height : playerSprite.Position.Y;
            playerSprite.Update(gameTime);
            for (int i = 0; i<sp.Count;i++)
            {
                sp[i].Update(gameTime);
                if (!screenRect.Contains(sp[i].Position) || sp[i].Position.Y > 1030 || CollisionHelper.PixelCollisionDetection(sp[i].Bounds,sp[i].Texture,cpuSprite.Bounds,cpuSprite.Texture))
                {
                    sp.RemoveAt(i);
                    i--;
                }
            }
            Console.WriteLine(sp.Count);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(Loader.Textures["background"], screenRect, Tint);
            spriteBatch.Draw(Loader.Textures["floor"], new Rectangle(0, 1030, 1920, 50), Tint);
            //TestSprite.Draw(gameTime,spriteBatch);
            playerSprite.Draw(gameTime, spriteBatch,playerFlip ? SpriteEffects.FlipHorizontally:SpriteEffects.None);
            cpuSprite.Draw(gameTime, spriteBatch);
            foreach (projectile poop in sp)
            {
                poop.Draw(spriteBatch,gameTime);
            }
            AS.Draw(gameTime, spriteBatch);
        }
    }

    class projectile
    {
        public Vector2 Velocity;
        public Vector2 Accelaration;
        public Vector2 Position { get { return sprite.Position; } }
        public Texture2D Texture { get { return sprite.Texture; } }
        public Rectangle Bounds { get { return sprite.Bounds; } }
        float Gravity;

        Sprite sprite;

        public projectile(Texture2D texture,Vector2 p,Vector2 v,Vector2 a,float gravity = 0.004f)
        {
            sprite = new Sprite(texture, p);
            Velocity = v;
            Accelaration = a;
            Gravity = gravity;
            sprite.Bounds = new Rectangle((int)sprite.Position.X, (int)sprite.Position.Y, 32, 32);
        }

        public void Update(GameTime gameTime)
        {
            Accelaration.Y += Gravity*gameTime.ElapsedGameTime.Milliseconds;
            Velocity += Accelaration;
            sprite.Position += Velocity;
        }

        public void Draw(SpriteBatch sb, GameTime gt)
        {
            sprite.Draw(gt,sb);
        }
    }
}
