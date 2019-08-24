using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Collections.Generic;

namespace SpiritMod.NPCs.Spirit
{
	public class SpiritMimic : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Mimic");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[475];
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 44;
			npc.damage = 50;
			npc.defense = 8;
			npc.lifeMax = 3500;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 240000f;
			npc.knockBackResist = .30f;
			npc.aiStyle = 87;
			aiType = NPCID.Zombie;
			animationType = 475;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0))
			{
				int[] TileArray2 = { mod.TileType("SpiritDirt"), mod.TileType("SpiritStone"), mod.TileType("Spiritsand"), mod.TileType("SpiritIce"), };
				return TileArray2.Contains(Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].type) && NPC.downedMechBossAny && spawnInfo.spawnTileY > (Main.rockLayer) ? 0.1f : 0f;
			}
			return 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, Vector2.Zero, 13);
				Gore.NewGore(npc.position, Vector2.Zero, 12);
				Gore.NewGore(npc.position, Vector2.Zero, 11);
			}
		}

		public override void NPCLoot()
		{
			string[] lootTable = { "SpiritFlameStaff", "Gravehunter", "SpiritSword", "StoneOfSpiritsPast", };
			int loot = Main.rand.Next(lootTable.Length);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType(lootTable[loot]));
		}

		private int Counter;
		public override void AI()
		{
			Counter++;
			if (Counter > 600)
			{
				int newNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("ShadowMimic"), npc.whoAmI);
				Counter = 0;
			}
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.05f, 0.05f, 0.4f);
		}
	}
}