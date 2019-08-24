using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tide.NPCs
{
	public class Kakamora3 : ModNPC
	{
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 100f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kakamoran Windglider");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 54;
			npc.height = 36;
			npc.damage = 44;
			npc.defense = 19;
			npc.lifeMax = 400;
			npc.noGravity = true;
			npc.knockBackResist = .30f;
			npc.value = 200f;
			npc.noTileCollide = false;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath2;
		}

		public override void NPCLoot()
		{
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DepthShard"), 1);
			}
			if (Main.rand.Next(33) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PalmSword"), 1);
			}

		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Player player = Main.player[npc.target];
			if (npc.Center.X >= player.Center.X && moveSpeed >= -23) // flies to players x position
			{
				moveSpeed--;
			}

			if (npc.Center.X <= player.Center.X && moveSpeed <= 23)
			{
				moveSpeed++;
			}

			npc.velocity.X = moveSpeed * 0.05f;

			if (npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -20) //Flies to players Y position
			{
				moveSpeedY--;
				HomeY = 40f;
			}

			if (npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 30)
			{
				moveSpeedY++;
			}

			npc.velocity.Y = moveSpeedY * 0.05f;
			if (Main.rand.Next(220) == 6)
			{
				HomeY = -15f;
			}
			if (Main.rand.Next(150) == 6) //Fires desert feathers like a shotgun
			{
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction.X *= 8f;
				direction.Y *= 8f;

				int amountOfProjectiles = Main.rand.Next(1, 1);
				for (int i = 0; i < amountOfProjectiles; ++i)
				{
					float A = (float)Main.rand.Next(-10, 10) * 0.01f;
					float B = (float)Main.rand.Next(-10, 10) * 0.01f;
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ProjectileID.PoisonSeedPlantera, 10, 1, Main.myPlayer, 0, 0);
				}
			}

		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (TideWorld.TheTide && TideWorld.InBeach  && NPC.downedMechBossAny)
				return 2f;

			return 0;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/KakamoraHead"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Leaf"), 1f);
				if (TideWorld.TheTide)
				{
					TideWorld.TidePoints2 += 1;
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
	}
}
