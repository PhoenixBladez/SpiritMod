using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class LightningNode : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lightning Node");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = 300;
			projectile.damage = 13;
			//projectile.extraUpdates = 1;
			projectile.width = projectile.height = 32;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;

		}
        float alphaCounter;
        public override void AI()
        {
            alphaCounter += 0.04f;
            int rightValue = (int)projectile.ai[0] - 1;
			if (rightValue < (double)Main.npc.Length && rightValue != -1) {
				NPC other = Main.npc[rightValue];
				Vector2 direction9 = other.Center - projectile.Center;
				int distance = (int)Math.Sqrt((direction9.X * direction9.X) + (direction9.Y * direction9.Y));
				direction9.Normalize();
				if (projectile.timeLeft % 4 == 0 && distance < 1000 && other.active) {
					int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)direction9.X * 30, (float)direction9.Y * 30, mod.ProjectileType("MoonLightning"), 30, 0);
					Main.projectile[proj].timeLeft = (int)(distance / 30);
					DustHelper.DrawElectricity(projectile.Center + (projectile.velocity * 4), other.Center + (other.velocity * 4), 226, 0.6f, 60);
				}
			}
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_49"), (projectile.Center - Main.screenPosition), null, new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + .6f), SpriteEffects.None, 0f);
        }
    }
}