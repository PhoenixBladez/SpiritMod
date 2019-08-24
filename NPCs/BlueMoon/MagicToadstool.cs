using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlueMoon
{
	public class MagicToadstool : ModNPC
	{
		int timer = 0;
		bool shrooms = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magic Toadstool");
			Main.npcFrameCount[npc.type] = 17;
		}

		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 38;
			npc.damage = 30;
			npc.defense = 10;
			npc.lifeMax = 600;
			npc.HitSound = SoundID.NPCHit45;
			npc.DeathSound = SoundID.NPCDeath47;
			npc.value = 1000f;
			npc.knockBackResist = 0f;
			npc.aiStyle = 3;
			aiType = 3;
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
				npc.width = 36;
				npc.height = 38;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 200; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 206, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return MyWorld.BlueMoon ? 3f : 0f;
		}

		public override void FindFrame(int frameHeight)
		{
			if (timer % 300 >= 79)
			{
				npc.frameCounter += 0.40f;
				npc.frameCounter %= Main.npcFrameCount[npc.type] - 4;
				int frame = (int)npc.frameCounter;
				npc.frame.Y = frame * frameHeight;
			}
			else
			{
				npc.frame.Y = (int)((Main.npcFrameCount[npc.type] - 4) + ((timer % 300) / 20))* frameHeight;
			}
		}

		public override void AI()
		{
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			timer++;
			if (timer % 300 == 79 && shrooms == false)
			{
				bool expertMode = Main.expertMode;
				int damage = expertMode ? 15 : 20;

				int speed = 4;
				Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 43);

				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -1 * speed, mod.ProjectileType("ToadStool"), damage, 1, Main.myPlayer, 0, 0);

				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 1 * speed, 0, mod.ProjectileType("ToadStool"), damage, 1, Main.myPlayer, 0, 0);

				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -1* speed, 0, mod.ProjectileType("ToadStool"), damage, 1, Main.myPlayer, 0, 0);

				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(0.70710678118 * speed), (float)(-0.70710678118 * speed), mod.ProjectileType("ToadStool"), damage, 1, Main.myPlayer, 0, 0);

				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(-0.70710678118 * speed), (float)(-0.70710678118 * speed), mod.ProjectileType("ToadStool"), damage, 1, Main.myPlayer, 0, 0);
				shrooms = true;
			}
			if (timer % 300 < 79)
			{
				if (player.position.X > npc.position.X)
				{
					npc.spriteDirection = 1;
					npc.netUpdate = true;
				}
				else
				{
					npc.spriteDirection = 0;
					npc.netUpdate = true;
				}

				npc.velocity.X = 0;

			}
			else
			{
				npc.spriteDirection = npc.direction;
				if (shrooms == true)
					shrooms = false;
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(mod.BuffType("StarFlame"), 200);
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(40) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GloomgusStaff"));
		}

	}
}

