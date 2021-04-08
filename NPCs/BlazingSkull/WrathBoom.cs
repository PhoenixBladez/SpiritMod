using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlazingSkull
{
	public class WrathBoom : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wrath explosion");
			Main.projFrames[projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 110;
			projectile.penetrate = -1;
			projectile.alpha = 0;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.scale = 1.6f;
		}

		public override void AI()
		{
			Lighting.AddLight(projectile.Center, Color.OrangeRed.ToVector3());
			projectile.frameCounter++;
			if (projectile.frameCounter > 4) {
				projectile.frameCounter = 0;
				projectile.frame++;
				if (projectile.frame > Main.projFrames[projectile.type])
					projectile.Kill();
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			int frameheight = texture.Height / Main.projFrames[projectile.type];
			Rectangle drawrect = new Rectangle(0, frameheight * projectile.frame, texture.Width, frameheight);
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, drawrect, Color.White, 0, drawrect.Size() / 2, projectile.scale, SpriteEffects.None, 0);
			return false;
		}

		public override bool CanHitPlayer(Player target) => projectile.frame <= (Main.projFrames[projectile.type] / 2);

		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit) => target.AddBuff(BuffID.OnFire, 180);
	}
}
