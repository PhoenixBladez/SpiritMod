using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarplateDrops.StarplateGlove
{
	public class StargloveOrbiterOrange: ModProjectile
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starfall");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 14; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.width = 2;
			projectile.height = 2;
			projectile.scale = 2f;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.magic = true;
            projectile.extraUpdates = 3;
            projectile.timeLeft = 60;
            projectile.ignoreWater = true;
		}
        Color ColorSelect = Color.Orange;
        int counter = 0;
        public override void AI()
        {
            counter++;
            Projectile parent = Main.projectile[(int)projectile.ai[0]];
            if (!parent.active)
            {
                projectile.active = false;
            }
            if (counter == 1)
            {
                projectile.velocity.Y = Main.rand.NextFloat(-16, 16);
                projectile.velocity.X = Main.rand.NextFloat(-16, 16);
               // ColorSelect = Color.Lerp(Color.Orange, Color.Yellow, Main.rand.NextFloat(1));
            }
            float x = 1f;
            float y = 1f;
             projectile.velocity += new Vector2((float)Math.Sign(parent.Center.X - projectile.Center.X), (float)Math.Sign(parent.Center.Y - projectile.Center.Y)) * new Vector2(x, y);
            if (projectile.velocity.Length() > 11)
            {
                projectile.velocity *= (2f / projectile.velocity.Length());
            }
        }
        float FlickerFactor => Math.Abs((float)Math.Sin(counter / 120f)) * 3;
        float FlickerFactor2 => Math.Abs((float)Math.Sin(counter / 20f)) * 3;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Helpers.DrawAdditive(Helpers.RadialMask, projectile.Center.ForDraw(), ColorSelect * (FlickerFactor * 0.1f + 0.12f), projectile.scale * Math.Abs((float)Math.Sin(counter / 120f) *0.2f));
            Vector2 drawOrigin = new Vector2(projectile.width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                    float fade = (1 - (k / (float)projectile.oldPos.Length));
                    Vector2 drawPos = projectile.oldPos[k].ForDraw() + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                    spriteBatch.Draw(Main.magicPixel, drawPos, new Rectangle(0, 0, 2, 2), ColorSelect * fade * FlickerFactor, projectile.rotation, drawOrigin, projectile.scale * fade, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}