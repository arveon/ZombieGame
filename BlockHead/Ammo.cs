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
    class Ammo:NonMovingGameObject
    {
        #region Fields
        Weapons weaponType;
        int amount;
        #endregion
        #region Properties
        /// <summary>
        /// Returns amount of ammo in the box
        /// </summary>
        public int Amount
        {
            get { return amount; }
        }

        /// <summary>
        /// Returns the type of ammo in the box
        /// </summary>
        public Weapons AmmoType
        {
            get { return weaponType; }
        }
        #endregion
        #region Constructors
        public Ammo(Texture2D texture, Vector2 position, Weapons weaponType, int amount, ObjectTypes type):base(texture,position, type)
        {
            this.amount = amount;
            this.weaponType = weaponType;
        }
        #endregion
        #region PublicMethods
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            //spriteBatch.Draw(this.texture, drawRectangle, Color.White);

        }
        #endregion
    }
}
