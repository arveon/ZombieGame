using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using UsefulClasses;

namespace BlockHead
{
    public class MovingGameObject: GameObject
    {
        protected Vector2 spriteCenter;
        protected float rotation;
        protected Vector2 velocity;
        protected Vector2 shift;
        #region Properties
        /// <summary>
        /// Object position on the screen
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Object movement velocity
        /// </summary>
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        #endregion
        #region Constructors
        public MovingGameObject(Texture2D texture, Vector2 position, ObjectTypes type) : base(texture, position, type)
        {
            rotation = 0;
            velocity = Vector2.Zero;
            shift = Vector2.Zero;
            spriteCenter = new Vector2(texture.Width/2, texture.Height/2);
        }
        #endregion
        #region PublicMethods
        public override void Update(GameTime gameTime, MouseState mouse, KeyboardState keyboard)
        {
            //calculate the texture & stuff shift
            shift.X = velocity.X * gameTime.ElapsedGameTime.Milliseconds;
            shift.Y = velocity.Y * gameTime.ElapsedGameTime.Milliseconds;

            //move sprite and collision circles
            position += shift;
            bigCollisionCircle.Center += shift;
            foreach(Circle smallCircle in smallCollisionCircles)
            {
                smallCircle.Center += shift;
            }
            drawRectangle.X = (int)position.X;
            drawRectangle.Y = (int)position.Y;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation, spriteCenter,
                1f, SpriteEffects.None,0);
        }

        #endregion

    }
}
