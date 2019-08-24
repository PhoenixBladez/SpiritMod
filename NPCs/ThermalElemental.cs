using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;


namespace SpiritMod.NPCs
{
	public class ThermalElemental : ModNPC
	{
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 150f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thermal Elemental");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 38;
			npc.height = 38;
			npc.damage = 45;
			npc.defense = 11;
			npc.noTileCollide = true;
			npc.lifeMax = 430;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 760f;
			npc.knockBackResist = .1f;
			npc.noGravity = true;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe || !NPC.downedPlantBoss)
			{
				return 0f;
			}
			return SpawnCondition.Cavern.Chance * 0.034478f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 10; i++)
				;
			if (npc.life <= 0)
			{
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}

		public override void NPCLoot()
		{
			int Bark = Main.rand.Next(7) + 4;
			for (int J = 0; J <= Bark; J++)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ThermiteOre"));
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(5) == 1)
			{
				target.AddBuff(BuffID.OnFire, 300);
			}
		}

		public override void AI()
		{
			int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
			Main.dust[dust].noGravity = true;
			npc.spriteDirection = npc.direction;
			Player player = Main.player[npc.target];
			if (npc.Center.X >= player.Center.X && moveSpeed >= -50) // flies to players x position
			{
				moveSpeed--;
			}

			if (npc.Center.X <= player.Center.X && moveSpeed <= 50)
			{
				moveSpeed++;
			}

			npc.velocity.X = moveSpeed * 0.1f;

			if (npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -40) //Flies to players Y position
			{
				moveSpeedY--;
				HomeY = 150f;
			}

			if (npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 40)
			{
				moveSpeedY++;
			}

			npc.velocity.Y = moveSpeedY * 0.12f;
			if (Main.rand.Next(220) == 6)
			{
				HomeY = -35f;
			}
			npc.rotation += 0.2f;
		}
	}
}
