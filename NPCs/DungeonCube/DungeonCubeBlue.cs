using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.DungeonCube
{
	public class DungeonCubeBlue : BaseDungeonCube
	{
		public override void SetDefaults()
		{
			base.SetDefaults();

			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.BlueDungeonCubeBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.spawnTileType == TileID.BlueDungeonBrick) {
				return spawnInfo.player.ZoneDungeon ? 0.04f : 0f;
			}
			return 0f;
		}
	}
}