using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace GameBaseHelpers
{
    class CollisionHelper
    {
        public static Boolean SquareSquare(Rectangle R1,Rectangle R2){
            return R1.Intersects(R2);
        }

        public static Boolean CircleCircle(Vector2 Center1, float Radius1, Vector2 Center2, float Radius2)
        {
            return (Vector2.Distance(Center1, Center2) <= Radius1 + Radius2);
        }

        public static Boolean PixelCollisionDetection(Rectangle Bounds1, Texture2D Texture1, Rectangle Bounds2, Texture2D Texture2)
        {
            // first Check that the bounds collide
            if(Bounds1.Intersects(Bounds2)){
                if (Bounds1.Contains(Bounds2) || Bounds2.Contains(Bounds1))
                    return true;
                

            }
            return false;
        }

        private static Boolean PixelPerfectCollision(Rectangle Bounds1, Texture2D Texture1, Rectangle Bounds2, Texture2D Texture2)
        {
            Color[] bitsA = new Color[Bounds1.Width * Bounds1.Height];
            Color[] bitsB = new Color[Bounds2.Width * Bounds1.Height];
            Texture1.GetData(bitsA);
            Texture2.GetData(bitsB);


            int x1 = Math.Max(Bounds1.X, Bounds2.X);
            int x2 = Math.Min(Bounds1.X + Bounds1.Width, Bounds2.X + Bounds2.Width);

            int y1 = Math.Max(Bounds1.Y, Bounds2.Y);
            int y2 = Math.Min(Bounds1.Y + Bounds1.Height, Bounds2.Y + Bounds2.Height);

            // For each single pixel in the intersecting rectangle
            for (int y = y1; y < y2; ++y)
            {
                for (int x = x1; x < x2; ++x)
                {
                    // Get the color from each texture
                    Color a = bitsA[(x - Bounds1.X) + (y - Bounds1.Y) * Texture1.Width];
                    Color b = bitsB[(x - Bounds2.X) + (y - Bounds2.Y) * Texture2.Width];

                    if (a.A != 0 && b.A != 0) // If both colors are not transparent (the alpha channel is not 0), then there is a collision
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
