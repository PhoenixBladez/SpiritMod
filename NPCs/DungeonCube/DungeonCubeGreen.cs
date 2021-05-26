using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.DungeonCube
{
	public class DungeonCubeGreen : BaseDungeonCube
	{
		public override void SetDefaults()
		{
			base.SetDefaults();

			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.GreenDungeonCubeBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.spawnTileType == TileID.GreenDungeonBrick) {
				return spawnInfo.player.ZoneDungeon ? 0.04f : 0f;
			}
			return 0f;
		}
		public override int TileDropType => 137;
		public override string GoreColor => "Green";
	}
}