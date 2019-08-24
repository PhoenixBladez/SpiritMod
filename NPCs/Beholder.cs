using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class Beholder : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beholder");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BoundGoblin];
		}

		public override void SetDefaults()
		{
			npc.width = 72;
			npc.height = 68;
			npc.damage = 22;
			npc.defense = 14;
			npc.lifeMax = 85;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 760f;
			npc.knockBackResist = .35f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			animationType = NPCID.BoundGoblin;
		}

		public override bool PreAI()
		{
			float velMax = 1f;
			float acceleration = 0.011f;
			npc.TargetClosest(true);
			Vector2 center = npc.Center;
			float deltaX = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - center.X;
			float deltaY = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - center.Y;
			float distance = (float)Math.Sqrt((double)deltaX * (double)deltaX + (double)deltaY * (double)deltaY);
			npc.ai[1] += 1f;
			if ((double)npc.ai[1] > 600.0)
			{
				acceleration *= 8f;
				velMax = 4f;
				if ((double)npc.ai[1] > 650.0)
				{
					npc.ai[1] = 0f;
				}
			}
			else if ((double)distance < 250.0)
			{
				npc.ai[0] += 0.9f;
				if (npc.ai[0] > 0f)
				{
					npc.velocity.Y = npc.velocity.Y + 0.019f;
				}
				else
				{
					npc.velocity.Y = npc.velocity.Y - 0.019f;
				}
				if (npc.ai[0] < -100f || npc.ai[0] > 100f)
				{
					npc.velocity.X = npc.velocity.X + 0.019f;
				}
				else
				{
					npc.velocity.X = npc.velocity.X - 0.019f;
				}
				if (npc.ai[0] > 200f)
				{
					npc.ai[0] = -200f;
				}
			}
			if ((double)distance > 350.0)
			{
				velMax = 5f;
				acceleration = 0.3f;
			}
			else if ((double)distance > 300.0)
			{
				velMax = 3f;
				acceleration = 0.2f;
			}
			else if ((double)distance > 250.0)
			{
				velMax = 1.5f;
				acceleration = 0.1f;
			}
			float stepRatio = velMax / distance;
			float velLimitX = deltaX * stepRatio;
			float velLimitY = deltaY * stepRatio;
			if (Main.player[npc.target].dead)
			{
				velLimitX = (float)((double)((float)npc.direction * velMax) / 2.0);
				velLimitY = (float)((double)(-(double)velMax) / 2.0);
			}
			if (npc.velocity.X < velLimitX)
			{
				npc.velocity.X = npc.velocity.X + acceleration;
			}
			else if (npc.velocity.X > velLimitX)
			{
				npc.velocity.X = npc.velocity.X - acceleration;
			}
			if (npc.velocity.Y < velLimitY)
			{
				npc.velocity.Y = npc.velocity.Y + acceleration;
			}
			else if (npc.velocity.Y > velLimitY)
			{
				npc.velocity.Y = npc.velocity.Y - acceleration;
			}
			if ((double)velLimitX > 0.0)
			{
				npc.spriteDirection = -1;
				npc.rotation = (float)Math.Atan2((double)velLimitY, (double)velLimitX);
			}
			if ((double)velLimitX < 0.0)
			{
				npc.spriteDirection = 1;
				npc.rotation = (float)Math.Atan2((double)velLimitY, (double)velLimitX) + 3.14f;
			}
			return false;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			int x = spawnInfo.spawnTileX;
			int y = spawnInfo.spawnTileY;
			int tile = (int)Main.tile[x, y].type;
			return (tile == 367) && spawnInfo.spawnTileY > Main.rockLayer && NPC.downedBoss2 ? 0.07f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Beholder1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Beholder1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Beholder2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Beholder1"), 1f);
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(4) == 1)
			{
				target.AddBuff(BuffID.Silenced, 160);
			}
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MarbleChunk"), 1);
		}
	}
}
