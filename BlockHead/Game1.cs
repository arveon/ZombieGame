using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

using System;

using UsefulClasses;

namespace BlockHead
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        const int WindowWidth = 800;
        const int WindowHeight = 600;
        const string healthPrefix = "Health: ";
        const string scorePrefix = "Score: ";
        const string rifleString = "Rifle";
        const string pistolString = "Pistol";

        public static Player player;
        static List<GameObject> listOfObjects = new List<GameObject>();
        Spawner spawner;
        GUI gui;

        Random rand = new Random();

        SpriteFont font;
        bool gameLost = false;

        CollisionHandler collisionHandler;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load a font
            font = Content.Load<SpriteFont>("Arial20");

            //create player and add him to the list of game objects
            Texture2D character = Content.Load<Texture2D>("survivor_handgun");
            Texture2D characterRifle = Content.Load<Texture2D>("survivor_rifle");
            Texture2D projectile = Content.Load<Texture2D>("bulletTrace");
            player = new Player(character, characterRifle, new Vector2(WindowWidth / 2, WindowHeight / 2), rand, projectile, ObjectTypes.Player);
            AddGameObject(player);

            //create a spawner and add it to the list of objects
            Texture2D pistolAmmoBox = Content.Load<Texture2D>("pistolammo");
            Texture2D rifleAmmoBox = Content.Load<Texture2D>("rifleammo");
            Texture2D propTexture = Content.Load<Texture2D>("prop");
            Texture2D regularTexture = Content.Load<Texture2D>("regularZombie");
            Texture2D scoutTexture = Content.Load<Texture2D>("scoutZombie");
            Texture2D tankTexture = Content.Load<Texture2D>("zombieTankc");
            spawner = new Spawner(pistolAmmoBox, rifleAmmoBox, propTexture, regularTexture, scoutTexture, tankTexture, rand);

            Texture2D healthTexture = Content.Load<Texture2D>("healthBar");
            Texture2D crosshairTexture = Content.Load<Texture2D>("crosshair");
            gui = new GUI(player, font, healthTexture, crosshairTexture);

            //create a collision handler
            collisionHandler = new CollisionHandler();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!gameLost)
            {
                //update all objects
                spawner.Update(gameTime);
                for (int i = 0; i < listOfObjects.Count; i++)
                {
                    listOfObjects[i].Update(gameTime, Mouse.GetState(), Keyboard.GetState());
                }

                //find and resolve all the collisions
                collisionHandler.Update(listOfObjects);

                //remove inactive objects
                for (int i = listOfObjects.Count - 1; i >= 0; i--)
                {
                    if (!listOfObjects[i].Active)
                    {
                        listOfObjects.RemoveAt(i);
                    }
                }

                gui.Update(gameTime, Mouse.GetState());
            }

            if(!player.Active)
            {
                gameLost = true;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            if (!gameLost)
            {
                foreach (GameObject obj in listOfObjects)
                {
                    obj.Draw(spriteBatch);
                }
            }
            else
                GraphicsDevice.Clear(Color.DarkRed);

            gui.Draw(spriteBatch);

            spriteBatch.End();
    
            base.Draw(gameTime);
        }

        public static void AddGameObject(GameObject gameObject)
        {
            listOfObjects.Add(gameObject);
        }

        public static List<GameObject> GetListOfObjects()
        {
            return listOfObjects;
        }

        public static Player GetPlayer()
        {
            return player;
        }

        public Random GetRandom()
        {
            return rand;
        }
    }
}
