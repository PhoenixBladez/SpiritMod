using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	public class QuacklingMinion : ModProjectile
	{
		int timer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quackling Minion");
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.netImportant = true;
			Projectile.width = 52;
			Projectile.height = 40;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			AIType = ProjectileID.Raven;
		}

		public override void AI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 6f)
			{
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
				Projectile.frameCounter = 0;
			}

			bool flag64 = Projectile.type == ModContent.ProjectileType<QuacklingMinion>();
			Player player = Main.player[Projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();

			if (flag64)
			{
				if (player.dead)
					modPlayer.QuacklingMinion = false;
				if (modPlayer.QuacklingMinion)
					Projectile.timeLeft = 2;
			}

			for (int num526 = 0; num526 < 1000; num526++)
			{
				if (num526 != Projectile.whoAmI && Main.projectile[num526].active && Main.projectile[num526].owner == Projectile.owner && Main.projectile[num526].type == Projectile.type && Math.Abs(Projectile.position.X - Main.projectile[num526].position.X) + Math.Abs(Projectile.position.Y - Main.projectile[num526].position.Y) < (float)Projectile.width)
				{
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

			if (Projectile.ai[0] == 0f)
			{
				for (int num531 = 0; num531 < 100; num531++)
				{
					if (Main.npc[num531].CanBeChasedBy(Projectile, false))
					{
						float num532 = Main.npc[num531].position.X + (float)(Main.npc[num531].width / 2);
						float num533 = Main.npc[num531].position.Y - 250 + (float)(Main.npc[num531].height / 2);
						float num534 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num532) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num533);
						if (num534 < num529 && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[num531].position, Main.npc[num531].width, Main.npc[num531].height))
						{
							num529 = num534;
							num527 = num532;
							num528 = num533;
							flag19 = true;
						}
					}
				}
			}
			else
				Projectile.tileCollide = false;

			if (!flag19)
			{
				Projectile.friendly = true;
				float num535 = 8f;
				if (Projectile.ai[0] == 1f)
					num535 = 12f;

				Vector2 vector38 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				float num536 = Main.player[Projectile.owner].Center.X - vector38.X;
				float num537 = Main.player[Projectile.owner].Center.Y - vector38.Y - 60f;
				float num538 = (float)Math.Sqrt((double)(num536 * num536 + num537 * num537));

				if (num538 < 100f && Projectile.ai[0] == 1f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
					Projectile.ai[0] = 0f;

				if (num538 > 2000f)
				{
					Projectile.position.X = Main.player[Projectile.owner].Center.X - (Projectile.width * .5f);
					Projectile.position.Y = Main.player[Projectile.owner].Center.Y - (Projectile.width * .5f);
				}

				if (num538 > 70f)
				{
					num538 = num535 / num538;
					num536 *= num538;
					num537 *= num538;
					Projectile.velocity.X = (Projectile.velocity.X * 20f + num536) * (1f / 21f);
					Projectile.velocity.Y = (Projectile.velocity.Y * 20f + num537) * (1f / 21f);
				}
				else
				{
					if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
					{
						Projectile.velocity.X = -0.15f;
						Projectile.velocity.Y = -0.05f;
					}
					Projectile.velocity *= 1.01f;
				}
				Projectile.friendly = false;
				Projectile.rotation = Projectile.velocity.X * 0.05f;

				if (Math.Abs(Projectile.velocity.X) > 0.2)
				{
					Projectile.spriteDirection = -Projectile.direction;
					return;
				}
			}
			else
			{
				timer++;

				if (timer % 70 == 1 && Main.netMode != NetmodeID.MultiplayerClient)
					Projectile.NewProjectile(Projectile.Center, new Vector2(0, 3), ModContent.ProjectileType<AquaBall>(), Projectile.damage, 0, Main.myPlayer);

				if (Projectile.ai[1] == -1f)
					Projectile.ai[1] = 17f;

				if (Projectile.ai[1] > 0f)
					Projectile.ai[1] -= 1f;

				if (Projectile.ai[1] == 0f)
				{
					Projectile.friendly = true;
					float num539 = 8f;
					float num540 = num527 - Projectile.Center.X;
					float num541 = num528 - Projectile.Center.Y;
					float num542 = (float)Math.Sqrt((double)(num540 * num540 + num541 * num541));
					if (num542 < 100f)
						num539 = 10f;

					num542 = num539 / num542;
					num540 *= num542;
					num541 *= num542;
					Projectile.velocity.X = (Projectile.velocity.X * 14f + num540) / 15f;
					Projectile.velocity.Y = (Projectile.velocity.Y * 14f + num541) / 15f;
				}
				else
				{
					Projectile.friendly = false;
					if (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) < 10f)
						Projectile.velocity *= 1.05f;
				}

				Projectile.rotation = Projectile.velocity.X * 0.05f;

				if (Math.Abs(Projectile.velocity.X) > 0.2)
				{
					Projectile.spriteDirection = -Projectile.direction;
					return;
				}
			}
		}

		public override bool MinionContactDamage() => true;
	}
}