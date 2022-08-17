using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Spirit
{
	public class RuneHostile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Flames");
			Main.projFrames[base.Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;       //projectile width
			Projectile.height = 16;  //projectile height
			Projectile.friendly = false;      //make that the projectile will not damage you
			Projectile.hostile = true;        // 
			Projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
			Projectile.penetrate = 1;      //how many npc will penetrate
			Projectile.timeLeft = 300;   //how many time projectile projectile has before disepire
			Projectile.ignoreWater = true;
			Projectile.extraUpdates = 1;
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			float num = 1f - (float)Projectile.alpha / 255f;
			num *= Projectile.scale;
			Lighting.AddLight(Projectile.Center, 0.5f * num, 0.5f * num, 0.9f * num);
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 8) {
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
				Projectile.frameCounter = 0;
			}

			float num1 = 10f;
			float num2 = 5f;
			float num3 = 40f;
			num1 = 10f;
			num2 = 7.5f;
			if (Projectile.timeLeft > 30 && Projectile.alpha > 0)
				Projectile.alpha -= 25;
			if (Projectile.timeLeft > 30 && Projectile.alpha < 128 && Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				Projectile.alpha = 128;
			if (Projectile.alpha < 0)
				Projectile.alpha = 0;

			if (++Projectile.frameCounter > 4) {
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 4)
					Projectile.frame = 0;
			}
			float num4 = 0.5f;
			if (Projectile.timeLeft < 120)
				num4 = 1.1f;
			if (Projectile.timeLeft < 60)
				num4 = 1.6f;

			++Projectile.ai[1];
			double num5 = (double)Projectile.ai[1] / 180.0;
			for (float num6 = 0.0f; (double)num6 < 3.0; ++num6) {
				if (!Main.rand.NextBool(3))
					return;
				Dust dust = Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustID.Flare_Blue, 0.0f, -2f)];
				dust.position = Projectile.Center + Vector2.UnitY.RotatedBy((double)num6 * (Math.PI / 1.5) + (double)Projectile.ai[1]) * 10f;
				dust.noGravity = true;
				dust.velocity = Projectile.DirectionFrom(dust.position);
				dust.scale = num4;
				dust.fadeIn = 0.5f;
				dust.alpha = 200;
			}

			int index1 = (int)Projectile.ai[0];
			if (index1 >= 0 && Main.player[index1].active && !Main.player[index1].dead) {
				if (Projectile.Distance(Main.player[index1].Center) <= num3)
					return;
				Vector2 unitY = Projectile.DirectionTo(Main.player[index1].Center);
				if (unitY.HasNaNs())
					unitY = Vector2.UnitY;
				Projectile.velocity = (Projectile.velocity * (num1 - 1f) + unitY * num2) / num1;
			}
			else {
				if (Projectile.timeLeft > 30)
					Projectile.timeLeft = 30;
				if (Projectile.ai[0] == -1f)
					return;
				Projectile.ai[0] = -1f;
				Projectile.netUpdate = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 40; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueCrystalShard, 0f, -2f, 0, default, 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				if (Main.dust[num].position != Projectile.Center) {
					Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
		}

	}
}
