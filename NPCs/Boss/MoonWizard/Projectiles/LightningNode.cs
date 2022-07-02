using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class LightningNode : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lightning Node");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 32;
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 300;
			Projectile.damage = 13;
		}

        float alphaCounter;

        public override void AI()
        {
            alphaCounter += 0.04f;
            int rightValue = (int)Projectile.ai[0] - 1;
			if (rightValue < (double)Main.npc.Length && rightValue != -1) {
				NPC other = Main.npc[rightValue];
				Vector2 direction9 = other.Center - Projectile.Center;
				int distance = (int)Math.Sqrt((direction9.X * direction9.X) + (direction9.Y * direction9.Y));
				direction9.Normalize();
				if (Projectile.timeLeft % 4 == 0 && distance < 1000 && other.active) {
					int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, direction9.X * 30, direction9.Y * 30, ModContent.ProjectileType<MoonLightning>(), 30, 0);
					Main.projectile[proj].timeLeft = distance / 30;
					DustHelper.DrawElectricity(Projectile.Center + (Projectile.velocity * 4), other.Center + (other.velocity * 4), 226, 0.6f, 60);
				}
			}
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void PostDraw(Color lightColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            Main.spriteBatch.Draw(TextureAssets.Extra[49].Value, (Projectile.Center - Main.screenPosition), null, new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + .6f), SpriteEffects.None, 0f);
        }
    }
}