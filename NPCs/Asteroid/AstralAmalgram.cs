using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.NPCs.Asteroid
{
	public class AstralAmalgram : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Amalgram");
			Main.npcFrameCount[npc.type] = 1;
		}
		private bool hasSpawnedBoys = false;
		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 48;
			npc.damage = 20;
			npc.defense = 5;
			npc.lifeMax = 540;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 60f;
			npc.knockBackResist = .60f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.aiStyle = 22;
			aiType = NPCID.Wraith;
			aiType = NPCID.Wraith;
			npc.stepSpeed = .5f;
		}

		private static int[] SpawnTiles = { };
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0))
			{
				return spawnInfo.player.GetSpiritPlayer().ZoneAsteroid ? 0.2f : 0f;
			}
			return 0f;
		}
		

	}
}