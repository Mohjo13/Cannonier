using C3.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cannoneer
{
    public class Target
    {
        private Vector2 position;
        private int size;

        private bool isExploding;
        private bool explosionFinished;
        private float explosionTimer;
        private float explosionDuration = 0.5f; // half a second

        public Target(Vector2 position, int size)
        {
            this.position = position;
            this.size = size;

            isExploding = false;
            explosionFinished = false;
            explosionTimer = 0f;
        }

        public void Update(GameTime gameTime)
        {
            if (!isExploding)
                return;

            explosionTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (explosionTimer <= 0f)
            {
                explosionTimer = 0f;
                isExploding = false;
                explosionFinished = true;
            }
        }

        public void TriggerExplosion()
        {
            if (isExploding || explosionFinished)
                return;

            isExploding = true;
            explosionTimer = explosionDuration;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isExploding)
            {
                float progress = 1f - (explosionTimer / explosionDuration);

                float outerRadius = size + (progress * 35f);
                float middleRadius = size + (progress * 25f);
                float innerRadius = size + (progress * 15f);

                spriteBatch.DrawCircle(position, outerRadius, 100, Color.Red, 8);
                spriteBatch.DrawCircle(position, middleRadius, 100, Color.Orange, 8);
                spriteBatch.DrawCircle(position, innerRadius, 100, Color.Yellow, 8);

                // small center blast
                spriteBatch.DrawCircle(position, size + 4, 100, Color.White, size);
            }
            else if (!explosionFinished)
            {
                spriteBatch.DrawCircle(position, size + 2, 100, Color.RoyalBlue, size);
                spriteBatch.DrawCircle(position, size - 1, 100, Color.RosyBrown, size);
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)position.X - size, (int)position.Y - size, size * 2, size * 2);
            }
        }

        public bool IsExploding
        {
            get { return isExploding; }
        }

        public bool ExplosionFinished
        {
            get { return explosionFinished; }
        }
    }
}