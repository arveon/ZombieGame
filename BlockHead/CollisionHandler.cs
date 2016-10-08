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
    class CollisionHandler
    {
        public CollisionHandler()
        {
        }

        public void Update(List<GameObject> gameObjectList)
        {
            //check for collisions and resolve them
            for(int i = 0; i < gameObjectList.Count - 1; i++)
            {
                for(int j = i+1; j < gameObjectList.Count; j++)
                {
                    //if collision occurs, resolve it
                    if(gameObjectList[i].Collides(gameObjectList[j]))
                    {
                        switch(gameObjectList[i].Type)
                        {
                            case ObjectTypes.Player:
                                Player player = (Player)gameObjectList[i];
                                switch (gameObjectList[j].Type)
                                { 
                                    case ObjectTypes.Ammobox:
                                        Ammo ammobox =(Ammo)gameObjectList[j];
                                        
                                        if(ammobox.AmmoType == Weapons.Pistol)
                                        {
                                            player.PistolAmmo += ammobox.Amount;
                                        }
                                        else if(ammobox.AmmoType == Weapons.Rifle)
                                        {
                                            player.RifleAmmo += ammobox.Amount;
                                        }
                                        ammobox.Active = false;
                                        break;
                                    case ObjectTypes.Prop:
                                        player.CollideWithGameObject((NonMovingGameObject)gameObjectList[j]);
                                        break;
                                    case ObjectTypes.Zombie:
                                        Zombie zombie = (Zombie)gameObjectList[j];
                                        zombie.CollideWithGameObject(player);
                                        zombie.AttackPlayer(player);
                                        player.CollideWithGameObject(zombie);
                                        player.PushBack(zombie);
                                        break;
                                }
                                break;
                            case ObjectTypes.Prop:
                                switch(gameObjectList[j].Type)
                                {
                                    case ObjectTypes.Projectile:
                                        gameObjectList[j].Active = false;
                                        break;
                                    case ObjectTypes.Zombie:
                                        Zombie zombie = (Zombie)gameObjectList[j];
                                        zombie.CollideWithGameObject((NonMovingGameObject)gameObjectList[i]);
                                        break;
                                }
                                break;
                            case ObjectTypes.Zombie:
                                Zombie zombie1 = (Zombie)gameObjectList[i];
                                switch (gameObjectList[j].Type)
                                {
                                    case ObjectTypes.Zombie:
                                        Zombie zombie2 = (Zombie)gameObjectList[j];
                                        zombie1.CollideWithGameObject(zombie2);
                                        zombie2.CollideWithGameObject(zombie1);
                                        break;
                                    case ObjectTypes.Projectile:
                                        Projectile proj = (Projectile)gameObjectList[j];
                                        zombie1.TakeDamage(proj.Damage);
                                        proj.Active = false;
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}
