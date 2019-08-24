using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Infernon
{
	[AutoloadBossHead]
	public class InfernoSkull : ModNPC
	{
		public static int _type;

		bool txt = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernus Skull");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 100;
			npc.height = 80;
			npc.knockBackResist = 0f;
			npc.defense = 20;
			npc.damage = 50;
			npc.lifeMax = 2500;
			bossBag = mod.ItemType("InfernonBag");
			npc.aiStyle = -1;
			npc.npcSlots = 10;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Infernon");
			npc.boss = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.10f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}


		public override bool PreNPCLoot()
		{
			MyWorld.downedInfernon = true;
			return true;
		}

		public override void NPCLoot()
		{
			if (Main.expertMode)
				npc.DropBossBags();
		}

		int timer1 = 0;
		int timer = 0;
		public override void AI()
		{
			if (npc.localAI[0] == 0f)
			{
				npc.localAI[0] = npc.Center.Y;
				npc.netUpdate = true; //localAI probably isnt affected by this... buuuut might as well play it safe
			}
			if (npc.Center.Y >= npc.localAI[0])
			{
				npc.localAI[1] = -1f;
				npc.netUpdate = true;
			}
			if (npc.Center.Y <= npc.localAI[0] - 10f)
			{
				npc.localAI[1] = 1f;
				npc.netUpdate = true;
			}
			npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y + 0.05f * npc.localAI[1], -2f, 2f);
			npc.ai[0] += 1f;
			npc.netUpdate = true;

			Player player = Main.player[npc.target];
			bool expertMode = Main.expertMode;
			int damage = expertMode ? 16 : 30;

			timer++;
			if (timer == 0 || timer == 200)
			{
				float spread = 45f * 0.0174f;
				double startAngle = Math.Atan2(1, 0) - spread / 2;
				double deltaAngle = spread / 8f;
				double offsetAngle;
				int i;
				for (i = 0; i < 4; i++)
				{
					offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i;
					Terraria.Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), mod.ProjectileType("InfernalWave"), 28, 0, Main.myPlayer);
					Terraria.Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), mod.ProjectileType("InfernalWave"), 28, 0, Main.myPlayer);
					npc.netUpdate = true;
				}
			}
			if (timer == 210 || timer == 220 || timer == 230 || timer == 240 || timer == 250 || timer == 260 || timer == 270 || timer == 280 || timer == 290 || timer == 300 || timer == 310 || timer == 320 || timer == 340 || timer == 350)
			{
				if (npc.life >= (npc.lifeMax / 3))
				{
					Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 33);
					Vector2 direction = Main.player[npc.target].Center - npc.Center;
					direction.Normalize();
					direction.X *= 1f;
					direction.Y *= 1f;

					int amountOfProjectiles = 1;
					for (int z = 0; z < amountOfProjectiles; ++z)
					{
						float A = (float)Main.rand.Next(-200, 200) * 0.03f;
						float B = (float)Main.rand.Next(-200, 200) * 0.03f;
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, mod.ProjectileType("InfernalBlastHostile"), damage, 1, Main.myPlayer, 0, 0);

					}
				}
			}


			if (timer == 400)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 200, 0f, 0f, mod.ProjectileType("Fireball"), damage, 1, Main.myPlayer, 0, 0);

				Projectile.NewProjectile(npc.Center.X + 200, npc.Center.Y, 0f, 0f, mod.ProjectileType("Fireball"), damage, 1, Main.myPlayer, 0, 0);

				Projectile.NewProjectile(npc.Center.X - 200, npc.Center.Y, 0f, 0f, mod.ProjectileType("Fireball"), damage, 1, Main.myPlayer, 0, 0);

				Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 200, 0f, 0f, mod.ProjectileType("Fireball"), damage, 1, Main.myPlayer, 0, 0);

				timer = 0;
			}
			else if (Main.rand.Next(90) == 1 && npc.life <= (npc.lifeMax / 3))
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 500, 0f, 0f, mod.ProjectileType("Fireball"), damage, 1, Main.myPlayer, 0, 0);

				Projectile.NewProjectile(npc.Center.X + 500, npc.Center.Y, 0f, 0f, mod.ProjectileType("Fireball"), damage, 1, Main.myPlayer, 0, 0);

				Projectile.NewProjectile(npc.Center.X - 500, npc.Center.Y, 0f, 0f, mod.ProjectileType("Fireball"), damage, 1, Main.myPlayer, 0, 0);

				Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 500, 0f, 0f, mod.ProjectileType("Fireball"), damage, 1, Main.myPlayer, 0, 0);
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 6, hitDirection, -1f, 0, default(Color), 1f);
			}
			if (npc.life <= 0)
			{
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 80;
				npc.height = 80;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 200; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 400; num623++)
				{
					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}

	}
}