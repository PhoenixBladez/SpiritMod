using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlueMoon
{
	public class MadHatter : ModNPC
	{
		int timer = 0;
		bool hat = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mad Hatter");
			Main.npcFrameCount[npc.type] = 15;
		}

		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 48;
			npc.damage = 44;
			npc.defense = 20;
			npc.lifeMax = 670;
			npc.HitSound = SoundID.NPCHit6;
			npc.DeathSound = SoundID.NPCDeath8;
			npc.value = 1000f;
			npc.knockBackResist = 0f;
			npc.aiStyle = 3;
			aiType = 104;
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
				npc.width = 40;
				npc.height = 48;
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
			return MyWorld.BlueMoon ? 1f : 0f;
		}

		public override void FindFrame(int frameHeight)
		{
			if (timer % 300 >= 80)
			{
				npc.frameCounter += 0.40f;
				npc.frameCounter %= 13;
				int frame = (int)npc.frameCounter + 2;
				npc.frame.Y = frame * 80;
			}
			else if (timer % 300 < 40)
			{
				npc.frame.Y = 80;
			}
			else
			{
				npc.frame.Y = 0;
			}
		}

		public override void AI()
		{
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			timer++;
			if (timer % 300 == 40 && hat == false)
			{
				Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 43);

				Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 15, 0, -7, mod.ProjectileType("MadHat"), 40, 1, Main.myPlayer, 0, 0);
				hat = true;
			}

			if (timer % 300 < 80)
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
				if (hat == true)
					hat = false;
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(mod.BuffType("StarFlame"), 200);
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(12) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 239);
			if (Main.rand.Next(20) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MadHat"));
		}

	}
}

