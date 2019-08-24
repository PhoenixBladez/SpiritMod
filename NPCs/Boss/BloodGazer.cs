using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss
{
	public class BloodGazer : ModNPC
	{
		int timer = 0;
		int moveSpeed = 0;
		int moveSpeedY = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Gazer");
		}

		public override void SetDefaults()
		{
			npc.width = 50;
			npc.height = 68;
			npc.damage = 50;
			npc.defense = 13;
			npc.lifeMax = 4000;
			npc.knockBackResist = 0;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.npcSlots = 5;
			Main.npcFrameCount[npc.type] = 7;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.boss = true;
			music = MusicID.Eerie;
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Player player = Main.player[npc.target];
			if (npc.Center.X >= player.Center.X && moveSpeed >= -30) // flies to players x position
				moveSpeed--;
			else if (npc.Center.X <= player.Center.X && moveSpeed <= 30)
				moveSpeed++;

			npc.velocity.X = moveSpeed * 0.1f;

			if (npc.Center.Y >= player.Center.Y - 50f && moveSpeedY >= -30) //Flies to players Y position
				moveSpeedY--;
			else if (npc.Center.Y <= player.Center.Y - 50f && moveSpeedY <= 30)
				moveSpeedY++;

			npc.velocity.Y = moveSpeedY * 0.1f;

			timer++;
			if (timer == 200)
			{
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction.X *= 5f;
				direction.Y *= 5f;
				timer = 0;

				int amountOfProjectiles = Main.rand.Next(5, 6);
				for (int i = 0; i < amountOfProjectiles; ++i)
				{
					float A = (float)Main.rand.Next(-150, 150) * 0.03f;
					float B = (float)Main.rand.Next(-150, 150) * 0.03f;
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, mod.ProjectileType("BloodyThing"), 30, 1, Main.myPlayer, 0, 0);
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}


		public override bool CheckDead()
		{
			NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("MoonGazer"));

			return true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.spawnTileY < Main.rockLayer && (Main.bloodMoon) && Main.hardMode && !NPC.AnyNPCs(mod.NPCType("BloodGazer")) ? 0.0074f : 0f;
		}
	}
}
