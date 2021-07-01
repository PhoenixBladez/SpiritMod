using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.QuasarGauntlet
{
    public class QuasarOrbiter : ModProjectile
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quasar Orb");
		}
		public override void SetDefaults()
		{
			projectile.width = 2;
			projectile.height = 2;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.extraUpdates = 3;
		}
        int counter = 0;
        Vector2 target = Vector2.Zero;
        Vector2 newCenter = Vector2.Zero;
        public override void AI()
        {
            counter++;
            Projectile parent = Main.projectile[(int)projectile.ai[0]];
            if (parent.active && parent.owner == projectile.owner && parent.type == mod.ProjectileType("QuasarOrb"))
            {
                target = parent.Center;
            }
            else
            {
                projectile.Kill();
            }
            if (counter == 1)
            {
                projectile.velocity = projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f));
               // ColorSelect = Color.Lerp(Color.Orange, Color.Yellow, Main.rand.NextFloat(1));
            }
            float x = 0.03f;
            float y = 0.03f;
             projectile.velocity += new Vector2((float)Math.Sign(target.X - projectile.Center.X), (float)Math.Sign(target.Y - projectile.Center.Y)) * new Vector2(x, y);
            newCenter += projectile.velocity;
            projectile.Center = target + newCenter;
            projectile.rotation += 0.1f;
            if (projectile.velocity.Length() > 12)
            {
                projectile.velocity *= (2f / projectile.velocity.Length());
            }
        }
        float FlickerFactor => Math.Abs((float)Math.Sin(counter / 120f)) * 3;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Helpers.DrawAdditive(Helpers.RadialMask, projectile.Center.ForDraw(), Color.White * (FlickerFactor * 0.1f + 0.12f), projectile.scale * Math.Abs((float)Math.Sin(counter / 120f) *0.2f));
            Texture2D texture = Main.projectileTexture[projectile.type];
            Color color = SpiritMod.StarjinxColor(Main.GlobalTime * 2 + projectile.ai[1]);
            Rectangle drawrect = new Rectangle(0, (int)projectile.ai[1] * texture.Height / 3, texture.Width, texture.Height / 3);
            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(drawrect), color, projectile.rotation, drawrect.Size()/2, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}