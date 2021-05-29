using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Weapon.Magic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.SacrificialDagger
{
	public class SacrificialDaggerProjectile : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sacrificial Dagger");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			ProjectileID.Sets.MinionShot[projectile.type] = true;
			Main.projFrames[projectile.type] = 4;
		}

		private readonly int maxtimeleft = 30;
		public override void SetDefaults()
		{
			projectile.timeLeft = maxtimeleft;
			projectile.friendly = true;
			projectile.height = 12;
			projectile.width = 12;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.extraUpdates = 1;
			projectile.usesIDStaticNPCImmunity = true;
			projectile.idStaticNPCHitCooldown = 5;
		}

        float alphaCounter;
		public override void AI()
        {
            alphaCounter += 0.08f;
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;

			if(projectile.timeLeft > 3 * (maxtimeleft / 4))
				projectile.alpha = Math.Max(projectile.alpha - (255 / (maxtimeleft / 4)), 0);

			else if (projectile.timeLeft < (maxtimeleft / 4))
				projectile.alpha = Math.Min(projectile.alpha + (255 / (maxtimeleft / 4)), 255);

			projectile.frameCounter++;
			if (projectile.frameCounter >= 5) {
				projectile.frameCounter = 0;
				projectile.frame = (projectile.frame + 1) % 4;
			}
		}



		public void AdditiveCall(SpriteBatch spriteBatch)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 2f;
            {
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
					Color color = new Color(255, 179, 246) * 0.75f * projectile.Opacity * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                    float scale = projectile.scale;
					Texture2D tex = Main.projectileTexture[projectile.type];
                    Texture2D glowtex = ModContent.GetTexture("SpiritMod/Projectiles/Summon/SacrificialDagger/SacrificialDagger_Trail");
					spriteBatch.Draw(glowtex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color * sineAdd, projectile.rotation, glowtex.Size() / 2, scale, default, default);
					spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, projectile.DrawFrame(), color, projectile.rotation, projectile.DrawFrame().Size() / 2, scale, default, default);
                }
            }
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;
	}
}
