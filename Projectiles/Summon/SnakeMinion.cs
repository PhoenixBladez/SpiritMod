using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Projectiles.Summon
{
	public class SnakeMinion : Minion
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flying Snake");
			Main.projFrames[Projectile.type] = 4;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
		}

		int timer = 0;
		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 32;
			Main.projPet[Projectile.type] = true;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 18000;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.netImportant = true;
		}

		public override void CheckActive()
		{
			MyPlayer mp = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
			if (mp.Player.dead)
				mp.SnakeMinion = false;

			if (mp.SnakeMinion)
				Projectile.timeLeft = 2;

		}

		public override void Behavior()
		{
			Player player = Main.player[Projectile.owner];
			for (int num526 = 0; num526 < 1000; num526++) {
				if (num526 != Projectile.whoAmI && Main.projectile[num526].active && Main.projectile[num526].owner == Projectile.owner && Main.projectile[num526].type == Projectile.type && Math.Abs(Projectile.position.X - Main.projectile[num526].position.X) + Math.Abs(Projectile.position.Y - Main.projectile[num526].position.Y) < (float)Projectile.width) {
					if (Projectile.position.X < Main.projectile[num526].position.X)
						Projectile.velocity.X = Projectile.velocity.X - 0.05f;
					else
						Projectile.velocity.X = Projectile.velocity.X + 0.05f;

					if (Projectile.position.Y < Main.projectile[num526].position.Y)
						Projectile.velocity.Y = Projectile.velocity.Y - 0.05f;
					else
						Projectile.velocity.Y = Projectile.velocity.Y + 0.05f;

				}
			}

			float num527 = Projectile.position.X;
			float num528 = Projectile.position.Y;
			float num529 = 900f;
			bool flag19 = false;

			if (Projectile.ai[0] == 0f) {
				for (int num531 = 0; num531 < 200; num531++) {
					if (Main.npc[num531].CanBeChasedBy(Projectile, false)) {
						float num532 = Main.npc[num531].position.X + (float)(Main.npc[num531].width / 2);
						float num533 = Main.npc[num531].position.Y - 250 + (float)(Main.npc[num531].height / 2);
						float num534 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num532) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num533);
						if (num534 < num529 && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[num531].position, Main.npc[num531].width, Main.npc[num531].height)) {
							num529 = num534;
							num527 = num532;
							num528 = num533;
							flag19 = true;
						}
					}
				}
			}
			else {
				Projectile.tileCollide = false;
			}

			if (!flag19) {
				Projectile.friendly = true;
				float num535 = 8f;
				if (Projectile.ai[0] == 1f)
					num535 = 12f;

				Vector2 vector38 = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
				float num536 = Main.player[Projectile.owner].Center.X - vector38.X;
				float num537 = Main.player[Projectile.owner].Center.Y - vector38.Y - 60f;
				float num538 = (float)Math.Sqrt((num536 * num536 + num537 * num537));
				if (num538 < 100f && Projectile.ai[0] == 1f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height)) {
					Projectile.ai[0] = 0f;
				}
				if (num538 > 2000f) {
					Projectile.position.X = Main.player[Projectile.owner].Center.X - (Projectile.width / 2f);
					Projectile.position.Y = Main.player[Projectile.owner].Center.Y - (Projectile.width / 2f);
				}
				if (num538 > 70f) {
					num538 = num535 / num538;
					num536 *= num538;
					num537 *= num538;
					Projectile.velocity.X = (Projectile.velocity.X * 20f + num536) / 21f;
					Projectile.velocity.Y = (Projectile.velocity.Y * 20f + num537) / 21f;
				}
				else {
					if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f) {
						Projectile.velocity.X = -0.15f;
						Projectile.velocity.Y = -0.05f;
					}
					Projectile.velocity *= 1.01f;
				}
				Projectile.friendly = false;
				Projectile.rotation = Projectile.velocity.X * 0.05f;

				if ((double)Math.Abs(Projectile.velocity.X) > 0.2) {
					Projectile.spriteDirection = -Projectile.direction;
					return;
				}
			}
			else {
				timer++;
				if (timer % 20 == 1) {
					float num = 8000f;
					int num2 = -1;
					for (int i = 0; i < 200; i++) {
						float num3 = Vector2.Distance(Projectile.Center, Main.npc[i].Center);
						if (num3 < num && num3 < 640f && Main.npc[i].CanBeChasedBy(Projectile, false)) {
							num2 = i;
							num = num3;
						}
					}
					if (num2 != -1) {
						bool flag = Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[num2].position, Main.npc[num2].width, Main.npc[num2].height);
						if (flag) {
							Vector2 value = Main.npc[num2].Center - Projectile.Center;
							float num4 = 12f;
							float num5 = (float)Math.Sqrt((double)(value.X * value.X + value.Y * value.Y));
							if (num5 > num4) {
								num5 = num4 / num5;
							}
							value *= num5;
							int p = Terraria.Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, value.X, value.Y, ProjectileID.VenomFang, Projectile.damage, Projectile.knockBack / 2f, Projectile.owner, 0f, 0f);
							Main.projectile[p].friendly = true;
							Main.projectile[p].hostile = false;
						}
					}

				}
				if (Projectile.ai[1] == -1f)
					Projectile.ai[1] = 17f;

				if (Projectile.ai[1] > 0f)
					Projectile.ai[1] -= 1f;

				if (Projectile.ai[1] == 0f) {
					Projectile.friendly = true;
					float num539 = 8f;
					Vector2 vector39 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
					float num540 = num527 - vector39.X;
					float num541 = num528 - vector39.Y;
					float num542 = (float)Math.Sqrt((double)(num540 * num540 + num541 * num541));
					if (num542 < 100f) {
						num539 = 10f;
					}
					num542 = num539 / num542;
					num540 *= num542;
					num541 *= num542;
					Projectile.velocity.X = (Projectile.velocity.X * 14f + num540) / 15f;
					Projectile.velocity.Y = (Projectile.velocity.Y * 14f + num541) / 15f;
				}
				else {
					Projectile.friendly = false;
					if (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) < 10f) {
						Projectile.velocity *= 1.05f;
					}
				}
				Projectile.rotation = Projectile.velocity.X * 0.05f;

				if (Math.Abs(Projectile.velocity.X) > 0.2) {
					Projectile.spriteDirection = -Projectile.direction;
					return;
				}
			}
		}

		public override void SelectFrame()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 10) {
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
			}
		}

	}
}
