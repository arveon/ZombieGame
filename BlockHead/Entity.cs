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
    public class Entity: MovingGameObject
    {
        public const int BaseHealth = 100;

        protected const int PushBackDistance = 20;
        protected const int TotalPushBackTime = 300;
        protected const float BaseSpeed = 0.1f;
        #region Fields
        protected int health;
        protected float MaxSpeed;

        GameObject currentCollisionProp;
        protected bool stoppedRight = false;
        protected bool stoppedDown = false;
        protected bool stoppedLeft = false;
        protected bool stoppedUp = false;

        bool pushedBack = false;
        Vector2 pushBackVelocity;
        int elapsedPushBackTime = 0;

        protected bool clampedTop;
        protected bool clampedBottom;
        protected bool clampedLeft;
        protected bool clampedRight;

        #endregion
        public Entity(Texture2D texture, Vector2 position, float maxSpeed, ObjectTypes type) : base(texture, position, type)
        {
            this.health = BaseHealth;
            MaxSpeed = maxSpeed;
        }
        #region PublicMethods
        public override void Update(GameTime gameTime, MouseState mouse, KeyboardState keyboard)
        {
            //if entity is in a process of being pushed back, push it back
            if(pushedBack && elapsedPushBackTime < TotalPushBackTime)
            {
                elapsedPushBackTime += gameTime.ElapsedGameTime.Milliseconds;
                velocity -= pushBackVelocity * 2;
            }
            else
            {
                pushedBack = false;
                elapsedPushBackTime = 0;
            }
            ProcessPropCollision();

            if(health <= 0)
            {
                health = 0;
                active = false;
            }

            //if its a player object, check if its clamped and change the velocity accordingly
            if(type == ObjectTypes.Player)
            {
                if(clampedTop && velocity.Y < 0)
                {
                    velocity.Y = 0;
                }
                else if(clampedBottom && velocity.Y > 0)
                {
                    velocity.Y = 0;
                }

                if (clampedLeft && velocity.X < 0)
                {
                    velocity.X = 0;
                }
                else if (clampedRight && velocity.X > 0)
                {
                    velocity.X = 0;
                }
            }

            base.Update(gameTime, mouse, keyboard);

            //reset clamps
            clampedBottom = false;
            clampedLeft = false;
            clampedRight = false;
            clampedTop = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        
        /// <summary>
        /// Method detects the way the collision should be resolved and
        /// saves the prop player is currently colliding with as a current collision prop
        /// </summary>
        /// <param name="prop">prop player currently colliding with</param>
        public void CollideWithGameObject(GameObject prop)
        {
            //if player is on the right of the prop, he can't move right anymore
            //else if player is on the left of the prop, he can's move left
            if (prop.BigCollisionCircle.Center.X > bigCollisionCircle.Center.X)
            {
                stoppedRight = true;
            }
            else
            {
                stoppedLeft = true;
            }

            //if player is above the prop, he can't move down
            //else if player is below the prop, he can't move up
            if (prop.BigCollisionCircle.Center.Y > bigCollisionCircle.Center.Y)
            {
                stoppedDown = true;
            }
            else
            {
                stoppedUp = true;
            }

            //save the prop entity is currently colliding as one of the fields
            currentCollisionProp = prop;
        }

        public void PushBack(Entity creature)
        {
            pushBackVelocity = -creature.Velocity;
            pushedBack = true;
        }

        /// <summary>
        /// Decreases current entity health by damage taken
        /// </summary>
        /// <param name="damage">damage taken by the entity</param>
        public void TakeDamage(int damage)
        {
            health -= damage;
        }
        #endregion
        #region ProtectedMethods
        /// <summary>
        /// Method will resolve collision by changing players velocity
        /// according to the directions that are blocked
        /// and try to wrap player around the object
        /// </summary>
        protected void ProcessPropCollision()
        {
            float MaxSpeed = this.MaxSpeed / 2;
            //if entity can't move right don't move right
            //else if entity can't move left don't move left
            if (stoppedRight)
            {
                //if entity is trying to move right, stop it
                if (velocity.X > 0)
                {
                    velocity.X = 0;

                    //depending whether the entity is above or below the prop centerpoint
                    //make entity wrap around it by moving it down/up
                    if (currentCollisionProp.BigCollisionCircle.Center.Y > bigCollisionCircle.Center.Y)
                    {
                        velocity.Y = -MaxSpeed;
                    }
                    else
                    {
                        velocity.Y = MaxSpeed;
                    }

                }
            }
            else if (stoppedLeft)
            {
                //if entity trying to move left, don't let it
                if (velocity.X < 0)
                {
                    velocity.X = 0;

                    //depending whether the entity is above or below the prop centerpoint
                    //make entity wrap around it by moving it down/up
                    if (currentCollisionProp.BigCollisionCircle.Center.Y > bigCollisionCircle.Center.Y)
                    {
                        velocity.Y = -MaxSpeed;
                    }
                    else
                    {
                        velocity.Y = MaxSpeed;
                    }

                }
            }

            //if entity can't move down don't let it
            if (stoppedDown)
            {
                //if it's trying to move down stop it's downward movement
                if (velocity.Y > 0)
                {
                    velocity.Y = 0;

                    //wrap entity around the object depending on whether it's on the right or on the left
                    //of the object centerpoint
                    if (currentCollisionProp.BigCollisionCircle.Center.X > bigCollisionCircle.Center.X)
                    {
                        velocity.X = -MaxSpeed;
                    }
                    else
                    {
                        velocity.X = MaxSpeed;
                    }

                }
            }
            else if (stoppedUp)
            {
                //if trying to move left
                //stop it
                if (velocity.Y < 0)
                {
                    velocity.Y = 0;

                    //try to wrap entity around the object depending on where it is in relation to the prop
                    if (currentCollisionProp.BigCollisionCircle.Center.X > bigCollisionCircle.Center.X)
                    {
                        velocity.X = -MaxSpeed;
                    }
                    else
                    {
                        velocity.X = MaxSpeed;
                    }
                }
            }

            //unblock all directions
            stoppedDown = false;
            stoppedRight = false;
            stoppedUp = false;
            stoppedLeft = false;
            currentCollisionProp = null;
        }
    }

        #endregion
}

