using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework;

namespace GameBaseHelpers
{
    public class Transition
    {
        protected GameScreen Screen;
        public Boolean Active = false;
        public Transition(GameScreen screen)
        {
            Screen = screen;
        }

        public virtual void Update(GameTime gameTime)
        {

        }
    }

    public class FadeInTransition : Transition
    {
        public float DelayTime = 15f;
        public float FadeInTime = 012f;

        float InternalTimer = 0f;
        float c = 0f;
        int CurrentPosition = 0;
        public FadeInTransition(GameScreen screen)
            : base(screen)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                InternalTimer += gameTime.ElapsedGameTime.Milliseconds * 0.001f;
                if (CurrentPosition == 0)
                {
                    if (InternalTimer >= DelayTime)
                    {
                        CurrentPosition++;
                        InternalTimer = 0f;
                    }
                }
                else if (CurrentPosition == 1)
                {
                    c = InternalTimer / FadeInTime;
                    if (InternalTimer >= FadeInTime)
                    {
                        CurrentPosition = 0;
                        InternalTimer = 0f;
                        Active = false;
                    }
                }
                Screen.Tint = new Color(c, c, c, c);
            }
            base.Update(gameTime);
        }
    }

    public class FadeInOutTransition : Transition
    {
        public float DelayTime = 0.5f;
        public float FadeInTime = 0.5f;
        public float FadeOutTime = 0.5f;
        public float HoldTime = 2f;
        float InternalTimer = 0f;
        float c = 0f;
        int CurrentPosition = 0;
        public FadeInOutTransition(GameScreen screen)
            :base(screen)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                InternalTimer += gameTime.ElapsedGameTime.Milliseconds*0.001f;
                if (CurrentPosition == 0)
                {
                    if (InternalTimer >= DelayTime)
                    {
                        CurrentPosition++;
                        InternalTimer = 0f;
                    }
                }
                else if (CurrentPosition == 1)
                {
                    c = InternalTimer / FadeInTime;
                    if (InternalTimer >= FadeInTime)
                    {
                        CurrentPosition++;
                        InternalTimer = 0f;
                    }
                }
                else if (CurrentPosition == 2)
                {
                    if (InternalTimer >= HoldTime)
                    {
                        CurrentPosition++;
                        InternalTimer = 0f;
                    }
                }
                else
                {
                    c = 1 - InternalTimer / FadeOutTime;
                    if (InternalTimer >= FadeOutTime)
                    {
                        CurrentPosition = 0;
                        InternalTimer = 0f;
                        Active = false;
                    }
                }
                Screen.Tint = new Color(c, c, c, c);
            }
            base.Update(gameTime);
        }
    }


}
