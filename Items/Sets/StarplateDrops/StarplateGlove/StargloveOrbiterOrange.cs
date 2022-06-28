using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarplateDrops.StarplateGlove
{
	public class StargloveOrbiterOrange: ModProjectile
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starfall");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.scale = 2f;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 60;
            Projectile.ignoreWater = true;
		}
        Color ColorSelect = Color.Orange;
        int counter = 0;
        public override void AI()
        {
            counter++;
            Projectile parent = Main.projectile[(int)Projectile.ai[0]];
            if (!parent.active)
            {
                Projectile.active = false;
            }
            if (counter == 1)
            {
                Projectile.velocity.Y = Main.rand.NextFloat(-16, 16);
                Projectile.velocity.X = Main.rand.NextFloat(-16, 16);
               // ColorSelect = Color.Lerp(Color.Orange, Color.Yellow, Main.rand.NextFloat(1));
            }
            float x = 1f;
            float y = 1f;
             Projectile.velocity += new Vector2((float)Math.Sign(parent.Center.X - Projectile.Center.X), (float)Math.Sign(parent.Center.Y - Projectile.Center.Y)) * new Vector2(x, y);
            if (Projectile.velocity.Length() > 11)
            {
                Projectile.velocity *= (2f / Projectile.velocity.Length());
            }
        }
        float FlickerFactor => Math.Abs((float)Math.Sin(counter / 120f)) * 3;
        float FlickerFactor2 => Math.Abs((float)Math.Sin(counter / 20f)) * 3;
        public override bool PreDraw(ref Color lightColor)
        {
            Helpers.DrawAdditive(Helpers.RadialMask, Projectile.Center.ForDraw(), ColorSelect * (FlickerFactor * 0.1f + 0.12f), Projectile.scale * Math.Abs((float)Math.Sin(counter / 120f) *0.2f));
            Vector2 drawOrigin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                    float fade = (1 - (k / (float)Projectile.oldPos.Length));
                    Vector2 drawPos = Projectile.oldPos[k].ForDraw() + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, drawPos, new Rectangle(0, 0, 2, 2), ColorSelect * fade * FlickerFactor, Projectile.rotation, drawOrigin, Projectile.scale * fade, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}