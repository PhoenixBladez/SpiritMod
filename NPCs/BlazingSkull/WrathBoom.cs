using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlazingSkull
{
	public class WrathBoom : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wrath explosion");
			Main.projFrames[Projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 110;
			Projectile.penetrate = -1;
			Projectile.alpha = 0;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.scale = 1.6f;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, Color.OrangeRed.ToVector3());
			Projectile.frameCounter++;
			if (Projectile.frameCounter > 4) {
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame > Main.projFrames[Projectile.type])
					Projectile.Kill();
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			int frameheight = texture.Height / Main.projFrames[Projectile.type];
			Rectangle drawrect = new Rectangle(0, frameheight * Projectile.frame, texture.Width, frameheight);
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, drawrect, Color.White, 0, drawrect.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}

		public override bool CanHitPlayer(Player target) => Projectile.frame <= (Main.projFrames[Projectile.type] / 2);

		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit) => target.AddBuff(BuffID.OnFire, 180);
	}
}
