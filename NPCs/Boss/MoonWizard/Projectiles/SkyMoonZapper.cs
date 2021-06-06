using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class SkyMoonZapper : ModProjectile
	{
		float distance = 8;
		int rotationalSpeed = 4;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Zapper");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.timeLeft = 100;
            projectile.hide = true;
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
            int proj = 0;
			if (projectile.timeLeft > 20 && projectile.timeLeft % 10 == 0) {
				proj = Projectile.NewProjectile(projectile.Center, new Vector2(0, 25), mod.ProjectileType("MoonPredictorTrail"), 0, 0);
			}
			if (projectile.timeLeft == 10)
			{
				Main.PlaySound(2, projectile.position, 122);
				int p = Projectile.NewProjectile(projectile.Center + new Vector2(0, 500), Vector2.Zero, mod.ProjectileType("MoonThunder"), projectile.damage, 0);
                Main.projectile[p].hostile = true;
                Main.projectile[p].friendly = true;
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