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
    public class Player : Entity
    {
        const float PlayerMaxSpeed = 0.2f;
        const int pistolTotalReloadTime = 500;
        const int rifleTotalReloadTime = 100;
        public const int InitialPistolAmmo = 20;
        public const int InitialRifleAmmo = 0;
        
        Vector2 centerOnPistol = Vector2.Zero;

        #region Fields
        int rifleAmmo;
        int pistolAmmo;

        int currentAmmo;
        Weapons currentWeapon;

        bool justSwitched = false;

        bool readyToShoot = true;
        int currentTotalReloadTime;
        int elapsedReloadTime;

        Texture2D texturePistol;
        Texture2D textureRifle;

        int score;

        Random rand;

        Texture2D projectileTexture;

        #endregion
        #region Properties
        /// <summary>
        /// Gets the ammo for the currently selected gun
        /// </summary>
        public int CurrentAmmo
        {
            get { return currentAmmo; }
        }

        /// <summary>
        /// Gets the currently selected weapon
        /// </summary>
        public Weapons CurrentWeapon
        {
            get { return currentWeapon; }
        }

        /// <summary>
        /// Gets and sets player score
        /// </summary>
        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        /// <summary>
        /// Gets and sets the amount of rifle ammo player has
        /// </summary>
        public int RifleAmmo
        {
            get { return rifleAmmo; }
            set
            {
                rifleAmmo = value;
                if (rifleAmmo < 0) rifleAmmo = 0;
            }
        }

        /// <summary>
        /// Gets and sets the amount of pistol ammo player has
        /// </summary>
        public int PistolAmmo
        {
            get { return pistolAmmo; }
            set
            {
                pistolAmmo = value;
                if (pistolAmmo < 0) pistolAmmo = 0;
            }
        }

        /// <summary>
        /// Returns players current health
        /// </summary>
        public int Health
        {
            get { return health; }
        }

        /// <summary>
        /// Returns the current rotation of the player sprite in radians
        /// </summary>
        public float Rotation
        {
            get { return rotation; }
        }

        #endregion
        #region Constructors
        public Player(Texture2D texturePistol, Texture2D textureRifle, Vector2 position, Random rand, Texture2D projectileTexture, ObjectTypes type) : base(texturePistol, position, PlayerMaxSpeed, type)
        {
            this.rand = rand;
            this.projectileTexture = projectileTexture;
            this.texturePistol = texturePistol;
            this.textureRifle = textureRifle;

            currentWeapon = Weapons.Pistol;
            pistolAmmo = InitialPistolAmmo;
            rifleAmmo = InitialRifleAmmo;
            score = 0;
        }
        #endregion
        #region PublicMethods
        #region Overrides
        public override void Update(GameTime gameTime, MouseState mouse, KeyboardState keyboard)
        {
            #region SwitchGuns
            //if Q is pressed and it didn't switch to that press, switch the gun
            //else if Q isn't pressed set justSwitched to false so that next press registers
            if(keyboard.IsKeyDown(Keys.Q) && !justSwitched)
            {
                //switch weapon
                if(currentWeapon == Weapons.Pistol)
                {
                    texture = textureRifle;
                    currentWeapon = Weapons.Rifle;
                }
                else if(currentWeapon == Weapons.Rifle)
                {
                    texture = texturePistol;
                    currentWeapon = Weapons.Pistol;
                }
                justSwitched = true;
            }
            else if(keyboard.IsKeyUp(Keys.Q))
            {
                justSwitched = false;
            }
            #endregion
            #region Shooting
            //set current ammo and required reload time depending on currently selected weapon
            if(currentWeapon == Weapons.Pistol)
            {
                currentAmmo = pistolAmmo;
                currentTotalReloadTime = pistolTotalReloadTime;
            }
            else if(currentWeapon == Weapons.Rifle)
            {
                currentAmmo = rifleAmmo;
                currentTotalReloadTime = rifleTotalReloadTime;
            }

            if (currentAmmo > 0)
            { 
                //shoot if mouse pressed and ready
                if (mouse.LeftButton == ButtonState.Pressed && readyToShoot)
                {
                    float cos = (float)Math.Cos(rotation);
                    float sin = (float)Math.Sin(rotation);
                    //create the projectile
                    Vector2 projShift;
                    projShift.X = Projectile.ProjectileSpawnDistance * cos;
                    projShift.Y = Projectile.ProjectileSpawnDistance * sin;

                    Vector2 projPosition = position + projShift + centerOnPistol;

                    //add projectile to the game
                    Game1.AddGameObject(new Projectile(projectileTexture, projPosition,
                        rand, currentWeapon, rotation, ObjectTypes.Projectile));

                    //reduce ammo
                    if (currentWeapon == Weapons.Pistol)
                    {
                        pistolAmmo--;
                    }
                    else if (currentWeapon == Weapons.Rifle)
                    {
                        rifleAmmo--;
                    }

                    //put character on reload
                    readyToShoot = false;
                }

                //reload if not ready to shoot
                if (!readyToShoot)
                {
                    //if mouse released - ready to shoot again
                    //else check reload timer
                    if (mouse.LeftButton == ButtonState.Released)
                    {
                        readyToShoot = true;
                    }
                    else
                    {
                        elapsedReloadTime += gameTime.ElapsedGameTime.Milliseconds;
                        //if reload timer has finished - ready to shoot again
                        if (elapsedReloadTime >= currentTotalReloadTime)
                        {
                            readyToShoot = true;
                            elapsedReloadTime = 0;
                        }
                    }
                }
            }
            #endregion
            #region Movement
            //reset speed
            velocity = Vector2.Zero;
            //velocity.Y = -0.1f;

            //set speed depending on keys pressed
            if (keyboard.IsKeyDown(Keys.W))
            {
                velocity.Y = -PlayerMaxSpeed;
            }
            else if (keyboard.IsKeyDown(Keys.S))
            {
                velocity.Y = PlayerMaxSpeed;
            }

            if (keyboard.IsKeyDown(Keys.A))
            {
                velocity.X = -PlayerMaxSpeed;
            }
            else if (keyboard.IsKeyDown(Keys.D))
            {
                velocity.X = PlayerMaxSpeed;
            }

            //decrease speed if moving diagonally
            if((velocity.X == PlayerMaxSpeed || velocity.X == -PlayerMaxSpeed) &&
                (velocity.Y == PlayerMaxSpeed || velocity.Y == -PlayerMaxSpeed))
            {
                velocity.X = PlayerMaxSpeed * (float)Math.Cos(Math.Atan2(velocity.Y, velocity.X));
                velocity.Y = PlayerMaxSpeed * (float)Math.Sin(Math.Atan2(velocity.Y, velocity.X));
            }

            //set rotation depending on the mouse position
            float dx = Mouse.GetState().X - position.X;
            float dy = Mouse.GetState().Y - position.Y;
            rotation = (float)Math.Atan2(dy, dx);

            #region ClampPlayerToScreen
            if (type == ObjectTypes.Player)
            {
                if (position.X - texture.Width / 2 < 0)
                {
                    clampedLeft = true;
                }
                else if (position.X + texture.Width / 2 > GameConstants.WindowWidth)
                {
                    clampedRight = true;
                }

                if (position.Y - texture.Height / 2 < 0)
                {
                    clampedTop = true;
                }
                else if (position.Y + texture.Height / 2 > GameConstants.WindowHeight)
                {
                    clampedBottom = true;
                }
            }
            #endregion

            //move
            base.Update(gameTime, mouse, keyboard);
            #endregion
            
            if(health <= 0)
            {
                health = 0;
                active = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, position, null, null, spriteCenter, rotation, null, Color.White, SpriteEffects.None, 0);
        }
        #endregion
        
        public void TakeDamage(int damage)
        {
            health -= damage;
        }

        #endregion
    }
}
