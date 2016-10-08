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
    class Projectile: MovingGameObject
    {
        //static constants
        public const int ProjectileSpawnDistance = 20;
        const float ProjectileDefaultVelocity = 2f;
        const int MaxPistolTravelDistance = 200;
        const int MaxRifleTravelDistance = 400;
        const int PistolDamage = 20;
        const int RifleDamage = 40;


        #region Fields
        protected int damage;
        int maxTravelDistance;
        Vector2 spawnPosition;
        #endregion
        #region Properties
        /// <summary>
        /// Gets the damage current projectile would inflict on collision
        /// </summary>
        public int Damage
        {
            get { return damage; }
        }
        #endregion
        #region Constructors
        public Projectile(Texture2D texture, Vector2 position, Random rand, Weapons weaponType, float rotation, ObjectTypes type) :
            base(texture, position, type)
        {
            
            //set positions
            spawnPosition = position;

            //set fields according to type
            if (weaponType == Weapons.Pistol)
            {
                maxTravelDistance = MaxPistolTravelDistance;
                damage = PistolDamage;
            }
            else if(weaponType == Weapons.Rifle)
            {
                maxTravelDistance = MaxRifleTravelDistance;
                damage = RifleDamage;
            }

            //add a bit of recoil
            this.rotation = rotation;
            rotation +=(float) (rand.NextDouble() - 0.5f)/5;

            //set the velocity
            velocity.X = ProjectileDefaultVelocity * (float)Math.Cos(rotation);
            velocity.Y = ProjectileDefaultVelocity * (float)Math.Sin(rotation);
        }
        #endregion
        #region PublicMethods
        public override void Update(GameTime gameTime, MouseState mouse, KeyboardState keyboard)
        {
            //if projectile is outside screen, make it inactive
            if (position.X + texture.Height < 0 ||
                position.X > GameConstants.WindowWidth ||
                position.Y + texture.Width < 0 ||
                position.Y > GameConstants.WindowHeight)
            {
                active = false;
            }

            //if projectile has travelled more than it's travel distance, make it inactive
            if((spawnPosition - position).Length() > maxTravelDistance)
            {
                active = false;
            }

            base.Update(gameTime, mouse, keyboard);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        #endregion
    }
}
