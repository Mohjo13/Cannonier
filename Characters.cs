using C3.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Cannoneer
{
    public class Character//***
    {
        private Vector2 position;
        private bool isCharging;
        private float chargeLevel;
        private float rotation;
        private float x;
        private List<Projectile> projectiles;
        private bool isHit;

        private float chargeSpeed = 0.1f; // Adjustable charge speed
        private bool increasingCharge = true; // Track whether the charge is increasing or decreasing

        public Character(List<Projectile> projectiles)//create a projectile with these paramters,
        {
            position = new Vector2(100, 175);//initial position
            isCharging = false;
            chargeLevel = 1.0f;
            x = 0;
            this.projectiles = projectiles;
            isHit = false;
        }

        public void Initialize()
        {
            rotation = 0f;//canon starts flat
        }

        public void Update(GameTime gameTime)
        {
            if (isHit)
            {
                // Character is hit, stop updating
                return;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W) && rotation > -1.5f)//radial angle equivilent to 90degrees
            {
                rotation -= 0.01f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S) && rotation < 0f)
            {
                rotation += 0.01f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A) && x > 0)//lateral movement left
            {
                x -= 1.0f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D) && x < 100)//lateral movement right
            {
                x += 1.0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))//
            {
                if (increasingCharge)
                {
                    chargeLevel += chargeSpeed;// charge level increases by charge speed
                    if (chargeLevel >= 6.5f)//maxcharge value so that 6 is easier to raech
                    {
                        chargeLevel = 6.5f;
                        increasingCharge = false;//stop charging
                    }
                }
                else
                {
                    chargeLevel -= chargeSpeed;//charge levels decrease by charge speed
                    if (chargeLevel <= 0.0f)//min value 0
                    {
                        chargeLevel = 0.0f;
                        increasingCharge = true;//start the charging process again, stop decreasing
                    }
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Space) && isCharging)//is space notpressed
            {
                isCharging = false;
                FireProjectile();//method fire 
            }

           
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(75 + x, 175, 100, 30, Color.Silver, rotation);//angle set to rotation also set to vector mechanics
            spriteBatch.DrawCircle(75 + x, 200, 50, 100, Color.DarkSlateGray, 100);// x lateral position increase and decrease 
        }
        //start charging mathod
        public void StartCharging()
        {
            isCharging = true;
        }

        public void FireProjectile()
        {
            float barrelLength = 70.0f;// length of barrel
            float initialPositionX = position.X + x + (barrelLength * (float)Math.Cos(rotation));//calculate x with rotation + length 
            float initialPositionY = position.Y + (barrelLength * (float)Math.Sin(rotation));//calculate y with rotation + length 
            var projectile = new Projectile(chargeLevel, new Vector2(initialPositionX, initialPositionY), rotation);
            projectiles.Add(projectile);
        }//create a new projectile with the charge level and the position realtive to characte x,y and angle
        public bool IsCharging
        {
            get { return isCharging; }//current state of chargning
        }

        public float GetChargeLevel()
        {
            return chargeLevel;// current charge level
        }

        public Vector2 GetPosition()
        {
            return position;// current position
        }

        public float GetRotation()
        {
            return rotation;// curent rotation and angle
        }

        public void Hit()
        {
            isHit = true;
        }
    }
}
