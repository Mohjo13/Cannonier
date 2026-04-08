using C3.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Cannoneer
{
    public class Projectile
    {
        float chargeLevel;
        Vector2 position;
        Vector2 velocity;
        Vector2 characterPosition;
        float rotation;
        private float x;
        private int size;

        public bool HasCollided { get; private set; }

        public Projectile(float chargeLevel, Vector2 characterPosition, float rotation)
        {
            this.chargeLevel = chargeLevel;
            this.characterPosition = characterPosition;
            this.rotation = rotation;

            float initialVelocityX = chargeLevel * 6 * (float)Math.Cos(rotation);
            float initialVelocityY = chargeLevel * 6 * (float)Math.Sin(rotation);

            position = characterPosition;
            velocity = new Vector2(initialVelocityX, initialVelocityY);
            size = 10;
        }

        public void Update(GameTime gameTime)
        {
            if (HasCollided)
            {
                return;
            }

            float gravity = 1.8f;
            velocity.Y += gravity;
            position += velocity;

            if (Keyboard.GetState().IsKeyDown(Keys.A) && position.X > 0)
            {
                x -= 1.0f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D) && position.X < 100)
            {
                x += 1.0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color color = HasCollided ? Color.Red : Color.Black;
            spriteBatch.DrawCircle(position, size, 100, color, size);
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)position.X - size, (int)position.Y - size, size * 2, size * 2);
            }
        }

        public void CollideWithTarget(Target target)
        {
            if (HasCollided)
            {
                return;
            }

            if (Bounds.Intersects(target.Bounds))
            {
                HasCollided = true;
                target.TriggerExplosion();
            }
        }
    }
}