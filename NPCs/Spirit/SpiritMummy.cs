using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpiritMod.NPCs.Spirit
{
	public class SpiritMummy : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dusk Mummy");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Mummy];
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 48;
			npc.damage = 50;
			npc.defense = 20;
			npc.lifeMax = 220;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 60f;
			npc.knockBackResist = .20f;
			npc.aiStyle = 3;
			aiType = NPCID.Mummy;
			aiType = NPCID.Mummy;
			animationType = NPCID.Mummy;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0))
			{
				int[] TileArray2 = { mod.TileType("Spiritsand"), };
				return TileArray2.Contains(Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].type) && NPC.downedMechBossAny && spawnInfo.spawnTileY < Main.rockLayer ? 5f : 0f;
			}
			return 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, 13);
				Gore.NewGore(npc.position, npc.velocity, 12);
				Gore.NewGore(npc.position, npc.velocity, 11);
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(5) == 1)
			{
				target.AddBuff(BuffID.Cursed, 150);
			}
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(3) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Rune"));
		}

	}
}