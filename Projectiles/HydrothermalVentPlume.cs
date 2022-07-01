using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.ReefhunterSet;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class HydrothermalVentPlume : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydrothermal Vent");
			Main.projFrames[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.alpha = 255;

			Projectile.hostile = false;
			Projectile.friendly = true;

			Projectile.penetrate = 4;
		}

		public override bool PreAI()
		{
			if (Projectile.timeLeft % 7 == 0 && Main.rand.NextBool(2))
			{
				int type = Main.rand.Next(new int[] { ModContent.ItemType<Items.Sets.CascadeSet.DeepCascadeShard>(), ModContent.ItemType<SulfurDeposit>() });
				int slot = Item.NewItem(Projectile.GetSource_FromAI(), new Vector2(Projectile.Center.X + Main.rand.Next(-10, 10), Projectile.Center.Y + Main.rand.Next(-10, 10)), 0, 0, type, 1, false, 0, false);

				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.SyncItem, -1, -1, null, slot, 1f);
			}

			if (Projectile.ai[0] == 0f)
			{
				Projectile.ai[1] += 1f;

				if (Projectile.ai[1] >= 60f)
					Projectile.Kill();

				for (int num1200 = 0; num1200 < 3; num1200++)
				{
					Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f)];
					dust.scale = 0.1f + Main.rand.Next(5) * 0.1f;
					dust.fadeIn = 1.5f + Main.rand.Next(5) * 0.1f;
					dust.noGravity = true;
					dust.position = Projectile.Center + new Vector2(0f, -Projectile.height / 2f).RotatedBy(Projectile.rotation, default) * 1.1f;
				}
			}

			int side = Math.Sign(Projectile.velocity.Y);
			float speedY = -2.5f * (0f - side);
			Dust dust81 = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Utils.SelectRandom(Main.rand, 6, 259, 31), 0f, speedY, 0, default, 1f)];
			dust81.alpha = 200;
			dust81.velocity *= new Vector2(0.3f, 2f);
			dust81.velocity.Y += 2 * side;
			dust81.scale += Main.rand.NextFloat();
			dust81.position = new Vector2(Projectile.Center.X, Projectile.Center.Y + Projectile.height * 0.5f * (0f - side));
			return true;
		}
	}
}