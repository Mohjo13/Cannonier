using C3.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Cannoneer
{
    public class Game1 : Game//***
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Character character;
        private List<Projectile> projectiles = new List<Projectile>();// ammo
        private Target target;
        private Random random;
        private SpriteFont font;// text type
        private Vector2 textPosition = new Vector2(10, 10);
        private bool isPaused;
        private string boomText;
        private Vector2 boomTextPosition;
        private bool targetHit;
        private bool canReset;//reset game possible 
        private bool canExit;//end game possible

        private string tutorialText;
        private bool showTutorial;
        private SpriteFont spritefont;
        private Vector2 textSpritePosition = new Vector2(10, 10);
        private Rectangle tutorialBox;
        private Vector2 tutorialTextSize;
        private Vector2 tutorialTextPosition;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);//graphics
            graphics.PreferredBackBufferWidth = 900;
            graphics.PreferredBackBufferHeight = 250;
            Content.RootDirectory = "Content"; //assets, Font1
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {


            character = new Character(projectiles); // Pass the projectiles list to Character, load canon with ammo
            character.Initialize(); //load canon

            random = new Random();
            int targetX = random.Next(600, 880);
            int targetY = random.Next(30, 180);
            target = new Target(new Vector2(targetX, targetY), 15);//randomize target position

            isPaused = false;//start game unpaused
            boomText = "";
            boomTextPosition = new Vector2(GraphicsDevice.Viewport.Width / 2f, GraphicsDevice.Viewport.Height / 2f);
            targetHit = false;//no hit yet
            canReset = true;
            canExit = true;

            base.Initialize();//initialze everything that is needed inte the update method
        }

        protected override void LoadContent()//load assets needed and called for
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Font1");


        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                showTutorial = false;
            }

            if (!isPaused)//checks if game is paused, if not keep going
            {
                character.Update(gameTime);//load canon and canon mechanics implemented in class

                if (Keyboard.GetState().IsKeyDown(Keys.Space) && !character.IsCharging)//if space pressed,and also not charging
                {
                    character.StartCharging();//start charging the canon
                }
                else if (Keyboard.GetState().IsKeyUp(Keys.Space) && character.IsCharging)//if space not pressed(released), and charging
                {
                    character.FireProjectile();//fire!
                }
            }//checks the status of the space key and implmenets the charging and fire methods basedon the result

            foreach (var projectile in projectiles)//cheks the following conditions for each new projectile
            {
                projectile.Update(gameTime);//creates new projectile with its game time mechanics
                projectile.CollideWithTarget(target);//collision mechanics

                if (projectile.HasCollided)//colison?
                {
                    boomText = "BOOM!!";//hit
                    isPaused = true;
                }
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();//exit game

            }

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                ResetGame();//reset game
            }

            base.Update(gameTime);
        }

        private void ResetGame()
        {
            character = new Character(projectiles);// load new character with loaded ammo
            character.Initialize();// load the initila state of the canon

            int targetX = random.Next(600, 880);//target random position generator
            int targetY = random.Next(30, 180);
            target = new Target(new Vector2(targetX, targetY), 15);//give random to target

            isPaused = false;//game starts unpasued
            boomText = "";//no hit yet
            targetHit = false;//no hit yet
            projectiles.Clear();//removes all elemnts from list, resets the ammo to 1
        }

        protected override void Draw(GameTime gameTime)//animate the game, draw the charachters and level design 
        {

            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.AliceBlue);//sky

            foreach (var projectile in projectiles)//for every projectile that is created and loaded from the initilze phase
            {
                projectile.Draw(spriteBatch);//draw new ammo
            }

            character.Draw(spriteBatch);//draw canon
            target.Draw(spriteBatch);//draw target

            spriteBatch.FillRectangle(225, 190, 10, 20, Color.Brown);//draw the wall
            spriteBatch.FillRectangle(0, 200, 1000, 50, Color.GreenYellow);//draw the grass

            // ----------------------------
            // CHARGE INDICATOR (SQUARES)
            // ----------------------------

            Vector2 textPosition = new Vector2(20, 20);

            spriteBatch.DrawString(font, "CHARGE", textPosition, Color.Black);

            int filledSquares = (int)character.GetChargeLevel();
            int totalSquares = 6;

            float startX = textPosition.X + 80;
            float startY = textPosition.Y + 0;

            float spacing =15f;

            int smallSize = 10;
            int bigSize = 18;

            for (int i = 0; i < totalSquares; i++)
            {
                bool isFilled = i < filledSquares;

                int squareSize = isFilled ? bigSize : smallSize;

                Color squareColor = isFilled ? Color.LimeGreen : Color.DarkGreen;

                float offset = (bigSize - squareSize) / 2f;

                spriteBatch.FillRectangle(
                    startX + i * spacing + offset,
                    startY + offset,
                    squareSize,
                    squareSize,
                    squareColor
                );
            }

            if (!string.IsNullOrEmpty(boomText))//based on boomtext value, true, draw text.
            {
                spriteBatch.DrawString(font, boomText, boomTextPosition, Color.Red);
            }

            spriteBatch.End();
            base.Draw(gameTime);

            
        }
    }
}
