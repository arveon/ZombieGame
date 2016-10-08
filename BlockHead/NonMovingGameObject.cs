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
    public class NonMovingGameObject: GameObject
    {
        public NonMovingGameObject(Texture2D texture, Vector2 position, ObjectTypes type) : base(texture, position, type)
        { }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
