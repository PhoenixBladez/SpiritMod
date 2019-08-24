using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class FierySpore : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Spore");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BoundGoblin];
		}

		public override void SetDefaults()
		{
			npc.width = 54;
			npc.height = 40;
			npc.damage = 60;
			npc.defense = 15;
			npc.lifeMax = 500;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 60f;
			npc.knockBackResist = .1f;
			npc.aiStyle = 0;
			aiType = NPCID.BoundGoblin;
			animationType = NPCID.BoundGoblin;
		}

		public override void AI()
		{
			Player target = Main.player[npc.target];
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
			if (distance < 320)
			{
				npc.ai[0]++;
				if (npc.ai[0] >= 120)
				{
					int type = ProjectileID.BallofFire;
					int p = Terraria.Projectile.NewProjectile(npc.position.X, npc.position.Y, -(npc.position.X - target.position.X) / distance * 4, -(npc.position.Y - target.position.Y) / distance * 4, type, (int)((npc.damage * .5)), 0);
					Main.projectile[p].friendly = false;
					Main.projectile[p].hostile = true;
					npc.ai[0] = 0;
				}
			}
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(35) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Boquet"));
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.spawnTileY > Main.rockLayer && Main.hardMode && spawnInfo.player.ZoneJungle ? 0.03f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Leaf"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Leaf"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Leaf"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Spore_4"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Spore_4"), 1f);
			}
		}
	}
}
