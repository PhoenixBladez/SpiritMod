using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SepulchreLoot.GraveyardTome
{
	public class GraveyardBoom : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Accursed explosion");
			Main.projFrames[Projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 110;
			Projectile.penetrate = -1;
			Projectile.alpha = 0;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.scale = Main.rand.NextFloat(0.6f, 0.8f);
			Projectile.rotation = Main.rand.NextFloat(-0.1f, 0.1f);
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, Color.Red.ToVector3() / 2);
			Projectile.frameCounter++;
			if (Projectile.frameCounter > 3) {
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
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, drawrect, Color.White, Projectile.rotation, drawrect.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => Projectile.frame <= (Main.projFrames[Projectile.type] / 3);
	}
}
