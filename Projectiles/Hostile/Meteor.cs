using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
	public class Meteor : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor");
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 6000;
			Projectile.height = 40;
			Projectile.hostile = true;
			Projectile.friendly = false;
		}
		int dust1 = 0;
		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			if (dust1 == 0)
				dust1 = Main.rand.Next(new int[] { 172, 68 });
			Vector2 position = Projectile.Center + Vector2.Normalize(Projectile.velocity) * 10;

			Dust newDust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dust1, 0f, 0f, 0, default, 1f)];
			newDust.position = position;
			newDust.velocity = Projectile.velocity.RotatedBy(Math.PI / 2, default) * 0.33F + Projectile.velocity / 4;
			newDust.position += Projectile.velocity.RotatedBy(Math.PI / 2, default);
			newDust.fadeIn = 0.5f;
			newDust.noGravity = true;
			newDust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dust1, 0f, 0f, 0, default, 1)];
			newDust.position = position;
			newDust.velocity = Projectile.velocity.RotatedBy(-Math.PI / 2, default) * 0.33F + Projectile.velocity / 4;
			newDust.position += Projectile.velocity.RotatedBy(-Math.PI / 2, default);
			newDust.fadeIn = 0.5F;
			newDust.noGravity = true;

			for (int i = 0; i < 1; i++) {
				newDust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dust1, 0f, 0f, 0, default, 1f)];
				newDust.velocity *= 0.5F;
				newDust.scale *= .5F;
				newDust.fadeIn = 1F;
				newDust.noGravity = true;
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

			if (Main.rand.Next(0, 4) == 0)
				Item.NewItem(Projectile.GetSource_Death(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, 116, 1, false, 0, false, false);

			for (int num625 = 0; num625 < 2; num625++) {
				float scaleFactor10 = 0.33f;
				if (num625 == 1) {
					scaleFactor10 = 0.66f;
				}
				if (num625 == 2) {
					scaleFactor10 = 1f;
				}

				for (int i = 0; i < 4; ++i)
				{
					int num626 = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
					Main.gore[num626].velocity *= scaleFactor10;
					Gore expr_13AB6_cp_0 = Main.gore[num626];
					expr_13AB6_cp_0.velocity.X = expr_13AB6_cp_0.velocity.X + 1f;
					Gore expr_13AD6_cp_0 = Main.gore[num626];
					expr_13AD6_cp_0.velocity.Y = expr_13AD6_cp_0.velocity.Y + 1f;
				}
			}

			Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
		}
	}
}