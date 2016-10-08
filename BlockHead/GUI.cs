using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace BlockHead
{
    
    class GUI
    {
        const string healthPrefix = "Health: ";
        const string scorePrefix = "Score: ";
        const string rifleString = "Rifle";
        const string pistolString = "Pistol";
        const string gameLostString = "You are dead!";

        Player player;

        Texture2D crosshairTexture;
        Rectangle crosshairRectangle;

        SpriteFont font;
        Texture2D healthTexture;
        Rectangle healthRectangle;
        Rectangle healthBGRectangle;
        int maxWidth;

        string ammo;
        string healthString;
        string scoreString;
        string curGunString;

        public GUI(Player player, SpriteFont font, Texture2D healthBarTexture, Texture2D crosshair)
        {
            this.healthTexture = healthBarTexture;
            this.font = font;
            this.player = player;

            this.crosshairTexture = crosshair;
            crosshairRectangle = new Rectangle(-15, -15, crosshairTexture.Width, crosshairTexture.Height);

            healthBGRectangle = new Rectangle(5,5,healthTexture.Width, healthTexture.Height);
            healthRectangle = new Rectangle(healthBGRectangle.X + 2, healthBGRectangle.Y + 2, healthBGRectangle.Width - 4, healthBGRectangle.Height - 4);
            maxWidth = healthRectangle.Width;
        }

        public void Update(GameTime gameTime, MouseState mouse)
        {
            #region Crosshair
            crosshairRectangle.X = mouse.X - crosshairRectangle.Width/2;
            crosshairRectangle.Y = mouse.Y - crosshairRectangle.Height / 2;
            
            //clamp crosshair to the window
            //horizontally
            if(crosshairRectangle.X + crosshairRectangle.Width > GameConstants.WindowWidth)
            {
                crosshairRectangle.X = GameConstants.WindowWidth - crosshairRectangle.Width;
            }
            else if(crosshairRectangle.X < 0)
            {
                crosshairRectangle.X = 0;
            }
            //vertically
            if (crosshairRectangle.Y + crosshairRectangle.Height > GameConstants.WindowHeight)
            {
                crosshairRectangle.Y = GameConstants.WindowHeight - crosshairRectangle.Height;
            }
            else if (crosshairRectangle.Y < 0)
            {
                crosshairRectangle.Y = 0;
            }
            #endregion

            healthRectangle.Width =(int)(maxWidth * ((float)player.Health / Entity.BaseHealth));
            ammo = "Current ammo: " + player.CurrentAmmo;
            healthString = healthPrefix + player.Health;
            scoreString = scorePrefix + player.Score;
            switch(player.CurrentWeapon)
            {
                case Weapons.Pistol:
                    curGunString = pistolString;
                    break;
                case Weapons.Rifle:
                    curGunString = rifleString;
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (player.Active)
            {
                //draw crosshair
                spriteBatch.Draw(crosshairTexture, crosshairRectangle, Color.White);

                //draw a health bar
                spriteBatch.Draw(healthTexture, healthBGRectangle, Color.Gray);
                spriteBatch.Draw(healthTexture, healthRectangle, Color.Green);

                //draw score, currently selected weapon and ammo
                spriteBatch.DrawString(font, scoreString, new Vector2(670, 0), Color.White);
                spriteBatch.DrawString(font, curGunString, new Vector2(GameConstants.WindowWidth - 250, GameConstants.WindowHeight - 60), Color.White);
                spriteBatch.DrawString(font, ammo, new Vector2(GameConstants.WindowWidth - 250, GameConstants.WindowHeight - 30), Color.White);
            }
            else
            {
                //draw lost string and final score
                spriteBatch.DrawString(font, gameLostString, new Vector2(GameConstants.WindowWidth / 2, GameConstants.WindowHeight / 2), Color.White);
                spriteBatch.DrawString(font, scoreString, new Vector2(GameConstants.WindowWidth / 2, GameConstants.WindowHeight / 2 + 30), Color.White);
            }
        }
    }
}
