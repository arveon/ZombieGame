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
    class Zombie:Entity
    {
        #region ZombieConstants
        const float ScoutSpeedBonus = BaseSpeed * 0.5f;
        const int ScoutHealth = 40;
        const int ScoutDamage = 5;
        const float TankSpeedBonus = -(BaseSpeed * 0.5f);
        const int TankHealth = 200;
        const int TankDamage = 20;
        const int RegularDamage = 10;

        const int TotalAttackCooldown = 500;
        #endregion

        #region Fields
        bool attacking = false;
        int elapsedCooldownTime = TotalAttackCooldown;
        ZombieType zombieType;
        int damage;
        Player player;
        #endregion

        #region Constructors
        public Zombie(Texture2D texture, Vector2 position, ZombieType zombieType, ObjectTypes type) : base(texture, position, BaseSpeed, type)
        {
            player = Game1.GetPlayer();
            this.zombieType = zombieType;
            AddTypeBonuses();
            
        }
        #endregion

        #region PublicMethods
        public override void Update(GameTime gameTime, MouseState mouse, KeyboardState keyboard)
        {
            
            float dx = player.Position.X - position.X;
            float dy = player.Position.Y - position.Y;

            rotation = (float)Math.Atan2(dy, dx);

            velocity.X = (float)Math.Cos(rotation) * (MaxSpeed);
            velocity.Y = (float)Math.Sin(rotation) * (MaxSpeed);

            //if still on cooldown, add time to cooldown
            if (elapsedCooldownTime < TotalAttackCooldown)
            {
                elapsedCooldownTime += gameTime.ElapsedGameTime.Milliseconds;
            }

            //if currently can attack player
            if (attacking)
            {
                //if not on cooldown, attack and make unable to attack
                if (elapsedCooldownTime >= TotalAttackCooldown)
                {
                    player.TakeDamage(damage);
                    elapsedCooldownTime = 0;
                }
                attacking = false;
            }

            if(health <= 0)
            {
                player.Score++;
            }

            base.Update(gameTime, mouse, keyboard);
        }

        /// <summary>
        /// Method gets called when zombie collides with player
        /// </summary>
        /// <param name="player">object of a player zombie collided</param>
        public void AttackPlayer(Player player)
        {
            attacking = true;
        }

        
        #endregion
        #region PrivateMethods
        /// <summary>
        /// Method sets zombie speed, health and damage according to its type
        /// </summary>
        private void AddTypeBonuses()
        {
            switch(zombieType)
            {
                case ZombieType.Regular:
                    MaxSpeed = BaseSpeed;
                    health = BaseHealth;
                    damage = RegularDamage;
                    break;
                case ZombieType.Scout:
                    MaxSpeed += ScoutSpeedBonus;
                    health = ScoutHealth;
                    damage = ScoutDamage;
                    break;
                case ZombieType.Tank:
                    MaxSpeed += TankSpeedBonus;
                    health = TankHealth;
                    damage = TankDamage;
                    break;
            }
        }
        #endregion
    }
}
