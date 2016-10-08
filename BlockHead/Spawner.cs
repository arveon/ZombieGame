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
    class Spawner
    {
        //spawner constants
        const int MaxNumberOfProps = 10;
        const int MinNumberOfProps = 5;

        const int MaxAmmoSpawnTime = 10000;
        const int MinAmmoSpawnTime = 5000;

        const int MinPistolAmmo = 20;
        const int MaxPistolAmmo = 100;

        const int MinRifleAmmo = 10;
        const int MaxRifleAmmo = 30;

        const int MaxNumberOfZombies = 5;
        const int MinNumberOfZombies = 1;

        const int MaxZombieSpawnTime = 5000;
        const int MinZombieSpawnTime = 2000;

        #region Fields
        int totalAmmoSpawnTime;
        int elapsedAmmoSpawnTime;

        int totalZombieSpawnTime;
        int elapsedZombieSpawnTime;

        Random rand;

        Texture2D pistolAmmoTexture;
        Texture2D rifleAmmoTexture;

        Texture2D propTexture;

        Texture2D regZombieTexture;
        Texture2D scoutZombieTexture;
        Texture2D tankZombieTexture;

        #endregion
        #region Constructors
        public Spawner(Texture2D pistolAmmoTexture, Texture2D rifleAmmoTexture, Texture2D propTexture, Texture2D regZombieTexture, Texture2D scoutZombieTexture, Texture2D tankZombieTexture, Random rand)
        {
            //initialise general variables
            this.rand = rand;

            //spawn random number of props on the map
            this.propTexture = propTexture;
            SpawnProps();

            this.regZombieTexture = regZombieTexture;
            this.scoutZombieTexture = scoutZombieTexture;
            this.tankZombieTexture = tankZombieTexture;
            totalZombieSpawnTime = rand.Next(MinZombieSpawnTime, MaxZombieSpawnTime + 1);

            //initialise ammo box variables
            this.pistolAmmoTexture = pistolAmmoTexture;
            this.rifleAmmoTexture = rifleAmmoTexture;
            //generate first ammo spawm time
            totalAmmoSpawnTime = rand.Next(MinAmmoSpawnTime, MaxAmmoSpawnTime + 1);
        }
        #endregion
        #region PublicMethods
        public void Update(GameTime gameTime)
        {
            //if time has come, spawn an ammobox
            elapsedAmmoSpawnTime += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedAmmoSpawnTime >= totalAmmoSpawnTime)
            {
                SpawnAmmoBox();
                elapsedAmmoSpawnTime = 0;
            }

            elapsedZombieSpawnTime += gameTime.ElapsedGameTime.Milliseconds;
            if(elapsedZombieSpawnTime >= totalZombieSpawnTime)
            {
                int numberOfZombies = rand.Next(MinNumberOfZombies, MaxNumberOfZombies + 1);
                int spawnPoint = rand.Next(8);

                SpawnZombies(numberOfZombies, spawnPoint);
                elapsedZombieSpawnTime = 0;
            }
        }
        #endregion
        #region PrivateMethods
        /// <summary>
        /// Method spawns an ammo box randomly on the map
        /// </summary>
        private void SpawnAmmoBox()
        {
            //generate random type of ammo
            int type = rand.Next(2);

            //required variables
            int ammo;
            Texture2D texture;
            Weapons atype;

            //depending on a type 'fill the box' with ammo and set the texture
            if (type == 0)
            {
                ammo = rand.Next(MinPistolAmmo, MaxPistolAmmo + 1);
                texture = pistolAmmoTexture;
                atype = Weapons.Pistol;
            }
            else
            {
                ammo = rand.Next(MinRifleAmmo, MaxRifleAmmo + 1);
                texture = rifleAmmoTexture;
                atype = Weapons.Rifle;
            }

            bool stop = false;
            Ammo ammoBox = null;

            //make sure that box doesn't spawn into a collision
            while (!stop)
            {
                //loop will stop unless variable changed
                stop = true;
                //generate new position for the box
                Vector2 spawnPosition = new Vector2();
                spawnPosition.X = rand.Next(texture.Width/2, GameConstants.WindowWidth - texture.Width);
                spawnPosition.Y = rand.Next(texture.Height/2, GameConstants.WindowHeight - texture.Height);

                //create a box object
                ammoBox = new Ammo(texture, spawnPosition, atype, ammo, UsefulClasses.ObjectTypes.Ammobox);

                //loop through game objects to check if box would collide with any of them
                foreach (GameObject obj in Game1.GetListOfObjects())
                {
                    //if box collides with at least one object, keep looping
                    if (obj.Collides(ammoBox))
                    {
                        stop = false;
                    }
                }
            }
            //add box to game world
            Game1.AddGameObject(ammoBox);
        }
        
        /// <summary>
        /// Method spawns random number of props on random points of the map
        /// </summary>
        private void SpawnProps()
        {
            
            NonMovingGameObject tempProp = null;
            int numOfProps = rand.Next(MinNumberOfProps, MaxNumberOfProps);

            for (int i = 0; i < numOfProps; i++)
            {
                bool stop = false;
                while (!stop)
                {
                    stop = true;
                    //generate position for the prop
                    Vector2 spawnPosition = new Vector2();
                    spawnPosition.X = rand.Next(propTexture.Width/2, GameConstants.WindowWidth - propTexture.Width/2);
                    spawnPosition.Y = rand.Next(propTexture.Height/2, GameConstants.WindowHeight - propTexture.Height/2);

                    tempProp = new NonMovingGameObject(propTexture, spawnPosition, UsefulClasses.ObjectTypes.Prop);

                    foreach (GameObject obj in Game1.GetListOfObjects())
                    {
                        if (tempProp.Collides(obj))
                        {
                            stop = false;
                            break;
                        }
                    }
                }
                
                Game1.AddGameObject(tempProp);
            }
        }
        #endregion


        private void SpawnZombies(int number, int spawnPoint)
        {
            
            #region Spawnpoint
            int minX = 0;
            int maxX = 0;
            int minY = 0;
            int maxY = 0;

            switch (spawnPoint)
            {
                case 0:
                    minX = -200;
                    maxX = 400;
                    minY = -200;
                    maxY = -regZombieTexture.Height / 2;
                    break;
                case 1:
                    minX = 400;
                    maxX = 600;
                    minY = -200;
                    maxY = -regZombieTexture.Height / 2;
                    break;
                case 2:
                    minX = 600;
                    maxX = 1000;
                    minY = -200;
                    maxY = -regZombieTexture.Height / 2;
                    break;
                case 3:
                    minX = 800 + regZombieTexture.Width/2;
                    maxX = 1000;
                    minY = 0;
                    maxY = 800;
                    break;
                case 4:
                    minX = 600;
                    maxX = 1000;
                    minY = 600 + regZombieTexture.Height / 2;
                    maxY = 800;
                    break;
                case 5:
                    minX = 400;
                    maxX = 600;
                    minY = 600 + regZombieTexture.Height/2;
                    maxY = 800;
                    break;
                case 6:
                    minX = -200;
                    maxX = 400;
                    minY = 600 + regZombieTexture.Height / 2;
                    maxY = 800;
                    break;
                case 7:
                    minX = -200;
                    maxX = -regZombieTexture.Width/2;
                    minY = 0;
                    maxY = 800;
                    break;

            }
            #endregion

            for (int i = 0; i < number; i++)
            {
                #region ZombieTypeGeneration
                Texture2D zombieTexture = regZombieTexture;
                ZombieType type = ZombieType.Regular;
                switch (rand.Next(3))
                {
                    case 0:
                        type = ZombieType.Regular;
                        zombieTexture = regZombieTexture;
                        break;
                    case 1:
                        type = ZombieType.Scout;
                        zombieTexture = scoutZombieTexture;
                        break;
                    case 2:
                        type = ZombieType.Tank;
                        zombieTexture = tankZombieTexture;
                        break;
                }
                #endregion
                int x = rand.Next(minX, maxX);
                int y = rand.Next(minY, maxY);
                Vector2 position = new Vector2(x, y);
                Zombie zombie = new Zombie(zombieTexture, position, type, ObjectTypes.Zombie);
                Game1.AddGameObject(zombie);
            }
        }

    }
}
