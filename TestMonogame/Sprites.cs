using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameBaseHelpers
{
    class Sprite
    {
        public Rectangle Bounds { 
            get { return new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height); }
            set { Height = value.Height; Width = value.Width; Position = value.Location.ToVector2(); }
        }
        private Rectangle sourceRect { get { return new Rectangle(0, 0, (int)spriteTexture.Width, (int)spriteTexture.Height); } }
        public Vector2 Position;
        public float Width;
        public float Height;
        public float Rotation;
        public Color ModColor = Color.White;
        protected Texture2D spriteTexture; // Loaded by the Loader but returned to this using function
        public Texture2D Texture { get { return spriteTexture; } }
        public Sprite(Texture2D texture)
        {
            spriteTexture = texture;
            Position = new Vector2(0, 0);
            Width = spriteTexture.Width;
            Height = spriteTexture.Height;
            Rotation = 0f;
        }

        public Sprite(Texture2D texture, Vector2 position)
        {
            Position = position;
            spriteTexture = texture;
            Width = spriteTexture.Width;
            Height = spriteTexture.Height;
        }

        public virtual void Update(GameTime gameTime){
            Console.WriteLine("Update");
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch,SpriteEffects se = SpriteEffects.None)
        {
            spriteBatch.Draw(spriteTexture,Bounds,sourceRect,ModColor,0f,Vector2.Zero,se,0);
        }

    }

    class AnimatedSprite : Sprite
    {
        private Int16 _frames;
        private int _currentFrame = 0;
        private Int16 _delay = 0;
        private Int16 _currentDelay = 0;
        private int spritePerRow { get { return Texture.Width / Bounds.Width; } }
        private Rectangle sourceRect;
        public Boolean Active = true;
        
        public AnimatedSprite(Texture2D texture, Rectangle bounds, Int16 frames,Int16 Delay = 0)
            :base(texture)
        {
            Width = bounds.Width;
            Height = bounds.Height;
            _frames = frames;
            _delay = Delay;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(Active)
            if (++_currentDelay > _delay)
            {
                _currentDelay = 0;
                _currentFrame = (int)++_currentFrame % (int)_frames;
                sourceRect = new Rectangle(_currentFrame % spritePerRow * (int)Width, _currentFrame / spritePerRow * (int)Height, (int)Width, (int)Height);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteEffects se = SpriteEffects.None)
        {
            spriteBatch.Draw(spriteTexture, Bounds, sourceRect, ModColor, 0f, Vector2.Zero, se, 0);
        }

        

        
    }

    class ninepatchSprite : Sprite
    {
        private Rectangle CenterRect;

        private int npsXCut1 {get{return CenterRect.X;}}
        private int npsXCut2 {get{return CenterRect.X+CenterRect.Width;}}
        private int npsYCut1 {get{return CenterRect.Y;}}
        private int npsYCut2 {get{return CenterRect.Y+CenterRect.Height;}}
        private int npsXCut2FromRight{get {return spriteTexture.Width-npsXCut2;}}
        private int npsYCut2FromBottom{get{return spriteTexture.Height-npsYCut2;}}


        private Rectangle topBounds
        {
            get
            {
                return new Rectangle(Bounds.X + npsXCut1,
                    Bounds.Y,
                    Bounds.Width - npsXCut1 - npsXCut2FromRight,
                    npsYCut1);
            }
        }
        private Rectangle leftBounds
        {
            get
            {
                return new Rectangle(Bounds.X,
                    Bounds.Y + npsYCut1,
                    npsXCut1,
                    Bounds.Height-(npsYCut2FromBottom+npsYCut1));
            }
        }
        private Rectangle rightBounds
        {
            get
            {
                return new Rectangle(Bounds.X + Bounds.Width - npsXCut2FromRight,
                    Bounds.Y + npsYCut1,
                    npsXCut2FromRight,
                    Bounds.Height-npsYCut1-npsYCut2FromBottom);
            }
        }
        private Rectangle bottomBounds
        {
            get
            {
                return new Rectangle(Bounds.X + npsXCut1,
                    Bounds.Y + Bounds.Height - npsYCut2FromBottom,
                    Bounds.Width - npsXCut1 - npsXCut2FromRight,
                    npsYCut2FromBottom);
            }
        }

        private Rectangle CenterBounds { get { return new Rectangle(Bounds.X + npsXCut1, Bounds.Y + npsYCut1, (int)Width - npsXCut2FromRight, (int)Height - npsYCut2FromBottom); } }

        private Rectangle topLeftBounds
        {
            get
            {
                return new Rectangle(Bounds.X,
                    Bounds.Y,
                    npsXCut1,
                    npsYCut1);
            }
        }
        private Rectangle topRightBounds
        {
            get
            {
                return new Rectangle(Bounds.X + Bounds.Width - npsXCut2FromRight,
                    Bounds.Y,
                    npsXCut2FromRight,
                    npsYCut1);
            }
        }
        private Rectangle bottomLeftBounds {get{return new Rectangle(Bounds.X,Bounds.Y+Bounds.Height-npsYCut2FromBottom,npsXCut1,npsYCut2FromBottom);}}
        private Rectangle bottomRightBounds { get{return new Rectangle(Bounds.X+Bounds.Width-npsXCut2FromRight,Bounds.Y+Bounds.Height-npsYCut2FromBottom,npsXCut2FromRight,npsYCut2FromBottom);}}
       

        public float Scale = 1f;
        public ninepatchSprite(Texture2D texture,Rectangle centerRect)
            : base(texture)
        {
            CenterRect = centerRect;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteEffects se = SpriteEffects.None)
        {
            spriteBatch.Draw(spriteTexture, Bounds, Color.Red);
            ////Draw the corners
            spriteBatch.Draw(spriteTexture, topLeftBounds, new Rectangle(0, 0, npsXCut1, npsYCut1), ModColor);
            spriteBatch.Draw(spriteTexture, topRightBounds, new Rectangle(npsXCut2, 0, npsXCut2FromRight, npsYCut1), ModColor);
            spriteBatch.Draw(spriteTexture, bottomLeftBounds, new Rectangle(0, npsYCut2, npsXCut1, npsYCut2FromBottom), ModColor);
            spriteBatch.Draw(spriteTexture, bottomRightBounds, new Rectangle(npsXCut2, npsYCut2, npsXCut2FromRight, npsYCut2FromBottom), ModColor);

            //Draw the sides
            spriteBatch.Draw(spriteTexture, topBounds, new Rectangle(npsXCut1, 0, npsXCut2 - npsXCut1, npsXCut1), ModColor);
            spriteBatch.Draw(spriteTexture, leftBounds, new Rectangle(0, npsYCut1, npsXCut1, npsYCut2 - npsXCut1), ModColor);
            spriteBatch.Draw(spriteTexture, rightBounds, new Rectangle(npsXCut2, npsYCut1, npsXCut2FromRight, npsYCut2 - npsYCut1), ModColor);
            spriteBatch.Draw(spriteTexture, bottomBounds, new Rectangle(npsXCut1, npsYCut2, npsXCut2FromRight, npsYCut2 - npsYCut1), ModColor);

           // spriteBatch.Draw(spriteTexture, CenterBounds, new Rectangle(npsXCut1, npsYCut1, npsXCut2 - npsXCut1, npsYCut2 - npsYCut1), ModColor);

            
        }
    }
}
