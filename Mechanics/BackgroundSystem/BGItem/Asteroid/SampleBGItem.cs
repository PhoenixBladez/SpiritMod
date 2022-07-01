using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.BackgroundSystem.BGItem.Asteroid
{
    public class SampleBGItem : BaseBGItem
    {
        private int scaleTimer = 0;
        private float timer = 0;

        public const int SpawnChance = 150;

        private readonly int scaleDecay = 0;
        private readonly float scaleSpd = 0f;

        public SampleBGItem(Vector2 pos) : base(pos, 0f, new Point(0, 0))
        {
            tex = ModContent.Request<Texture2D>("SpiritMod/Mechanics/BackgroundSystem/BGItem/Asteroid/SampleBGItem");
            source = new Rectangle(0, 0, tex.Width, tex.Height);

            scaleDecay = Main.rand.Next(2500, 2800);
            scaleSpd = Main.rand.NextFloat(0.002f, 0.005f);
            parallaxScale = Main.rand.Next(550, 851) * 0.001f;

            timer = Main.rand.Next(10000);

            source = new Rectangle(63 * Main.rand.Next(3), 0, 62, 82);
        }

        internal override void Behaviour()
        {
            base.Behaviour();
            velocity.X = (float)Math.Sin((timer++ * 2) * 0.004f) * 0.4f;
            velocity.Y = (float)Math.Sin(timer * 0.004f) * 0.4f;

            scaleTimer++;
            if (scaleTimer++ < scaleDecay - 120 && scale < parallaxScale)
                scale += scaleSpd;
            else if (scaleTimer > scaleDecay - 100)
                scale *= 0.987f;

            if (scaleTimer > scaleDecay && scale < 0.0001f)
                killMe = true;

            rotation = velocity.X * 0.4f;
            BaseParallax();
        }

        internal override void Draw(Vector2 off)
        {
            drawColor = Color.Lerp(new Color(0.6f, 0.24f, 0.42f), Main.ColorOfTheSkies, (drawColor.R - 10) / 245f);
            base.Draw(GetParallax());
            Color col = Color.Lerp(new Color(0.72f, 0.230f, 0.50f), Color.White, (drawColor.R - 10) / 245f);
            Main.spriteBatch.Draw(tex, DrawPosition + GetParallax() - Main.screenPosition, new Rectangle(source.X, 90, 62, 23), col, rotation, tex.Bounds.Center.ToVector2(), scale, SpriteEffects.None, 0f);
        }
    }
}
