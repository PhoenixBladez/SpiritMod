using Terraria.ModLoader;
using Terraria;

namespace SpiritMod.NPCs.DungeonCube
{
	public class DungeonCubePink : BaseDungeonCube
	{
		public override void SetDefaults()
		{
			base.SetDefaults();

			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.PinkDungeonCubeBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.downedPlantBoss)
			{
				return spawnInfo.player.ZoneDungeon && NPC.CountNPCS(ModContent.NPCType<DungeonCubePink>()) < 1 ? 0.0015f : 0f;
			}
			if (spawnInfo.spawnTileType == 44) {
				return spawnInfo.player.ZoneDungeon ? 0.04f : 0f;
			}
			return 0f;
		}

		public override int TileDropType => 139;

		public override string GoreColor => "Pink";
	}
}