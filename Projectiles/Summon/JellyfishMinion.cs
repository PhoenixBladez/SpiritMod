using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using System.Linq;

namespace SpiritMod.Projectiles.Summon
{
	public class JellyfishMinion : ModProjectile
    {
		int timer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Little Jellyfish");

			Main.projFrames[Projectile.type] = 6;
			Main.projPet[Projectile.type] = true;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.netImportant = true;
			Projectile.width = 28;
			Projectile.height = 28;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			AIType = ProjectileID.Raven;
		}
		
        int colorType;
        Color colorVer;
        bool chosenColor;

		public override bool? CanCutTiles() => false;

		public override void AI()
		{
			if (!chosenColor)
			{
				colorType = Main.rand.Next(0, 2);
				chosenColor = true;
			}

			if (colorType == 0)
				colorVer = new Color(133 + Main.rand.Next(-10, 20), 177 + Main.rand.Next(-10, 20), 255 + Main.rand.Next(0, 10));
			else if (colorType == 1)
				colorVer = new Color(248 + Main.rand.Next(-13, 6), 148 + Main.rand.Next(-10, 20), 255 + Main.rand.Next(-20, 0));

			bool flag64 = Projectile.type == ModContent.ProjectileType<JellyfishMinion>();
			Player player = Main.player[Projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();

			if (flag64)
			{
				if (player.dead)
					modPlayer.jellyfishMinion = false;
				if (modPlayer.jellyfishMinion)
					Projectile.timeLeft = 2;
			}

			foreach (Projectile p in Main.projectile.Where(x => x.active && x != null && x.type == Projectile.type && x.owner == Projectile.owner && x != Projectile))
			{
				if (p.Hitbox.Intersects(Projectile.Hitbox))
					Projectile.velocity += Projectile.DirectionFrom(p.Center) / 5;
			}

			float num527 = Projectile.position.X;
			float num528 = Projectile.position.Y;
			float num529 = 900f;
			bool flag19 = false;

			if (Projectile.ai[0] == 0f)
			{
				for (int num531 = 0; num531 < 200; num531++)
				{
					if (Main.npc[num531].CanBeChasedBy(Projectile, false))
					{
						float num532 = Main.npc[num531].position.X + (Main.npc[num531].width / 2);
						float num533 = Main.npc[num531].position.Y - 150 + (Main.npc[num531].height / 2);
						float num534 = Math.Abs(Projectile.position.X + (Projectile.width / 2) - num532) + Math.Abs(Projectile.position.Y + (Projectile.height / 2) - num533);
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
			{
				Projectile.tileCollide = false;
			}

			if (!flag19)
			{
				Projectile.frameCounter++;
				if (Projectile.frameCounter >= 10f)
				{
					Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
					Projectile.frameCounter = 0;
					if (Projectile.frame >= 2)
						Projectile.frame = 0;
				}

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
						Projectile.velocity.X = -0.05f;
						Projectile.velocity.Y = -0.025f;
					}
					Projectile.velocity *= 1.005f;
				}
				Projectile.friendly = false;
				Projectile.rotation = Projectile.velocity.X * 0.05f;

				if (Math.Abs(Projectile.velocity.X) > 0.1)
				{
					Projectile.spriteDirection = -Projectile.direction;
					return;
				}
			}

			else
			{
				Projectile.frameCounter++;
				if (Projectile.frameCounter >= 6f)
				{
					Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
					Projectile.frameCounter = 0;
					if (Projectile.frame > 5 || Projectile.frame < 4)
						Projectile.frame = 3;
				}
				timer++;

				if (timer >= Main.rand.Next(50, 90))
				{
					int range = 100;   //How many tiles away the projectile targets NPCs
					float shootVelocity = 6.5f; //magnitude of the shoot vector (speed of arrows shot)

					//TARGET NEAREST NPC WITHIN RANGE
					float lowestDist = float.MaxValue;
					for (int i = 0; i < 200; ++i)
					{
						NPC npc = Main.npc[i];
						//if npc is a valid target (active, not friendly, and not a critter)
						if (npc.active && npc.CanBeChasedBy(Projectile) && !npc.friendly)
						{
							//if npc is within 50 blocks
							float dist = Projectile.Distance(npc.Center);
							if (dist / 16 < range)
							{
								//if npc is closer than closest found npc
								if (dist < lowestDist)
								{
									lowestDist = dist;

									//target this npc
									Projectile.ai[1] = npc.whoAmI;
									Projectile.netUpdate = true;
								}
							}
						}
					}

					NPC target = (Main.npc[(int)Projectile.ai[1]] ?? new NPC());
					SoundEngine.PlaySound(SoundID.Item12, Projectile.Center);
					timer = 0;
					Vector2 ShootArea = new Vector2(Projectile.Center.X, Projectile.Center.Y - 13);
					Vector2 direction = Vector2.Normalize(target.Center - ShootArea) * shootVelocity;

					for (int i = 0; i < 10; i++)
					{
						int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0f, -2f, 0, default, .5f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;

						if (Main.dust[num].position != Projectile.Center)
							Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 1.1f;

						if (colorType == 0)
							Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(96, Main.LocalPlayer);
						else
							Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(93, Main.LocalPlayer);
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						if (colorType == 0)
							Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, direction.X, direction.Y, ModContent.ProjectileType<BlueJellyfishBolt>(), Projectile.damage, 0, Main.myPlayer);
						else
							Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, direction.X, direction.Y, ModContent.ProjectileType<PinkJellyfishBolt>(), Projectile.damage, 0, Main.myPlayer);
					}
				}
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
					float num542 = (float)Math.Sqrt(num540 * num540 + num541 * num541);
					if (num542 < 100f)
						num539 = 10f;

					num542 = num539 / num542;
					num540 *= num542;
					num541 *= num542;
					Projectile.velocity.X = (Projectile.velocity.X * 12.5f + num540) / 15f;
					Projectile.velocity.Y = (Projectile.velocity.Y * 12.5f + num541) / 15f;
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

		public override Color? GetAlpha(Color lightColor) => colorVer;
		public override bool MinionContactDamage() => true;
	}
}