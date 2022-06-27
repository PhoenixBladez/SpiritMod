using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace SpiritMod.NPCs.DungeonCube
{
	public class DungeonCubeBlue : BaseDungeonCube
	{
		public override void SetDefaults()
		{
			base.SetDefaults();

			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.BlueDungeonCubeBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.downedPlantBoss)
			{
				return spawnInfo.Player.ZoneDungeon && NPC.CountNPCS(ModContent.NPCType<DungeonCubeBlue>()) < 1 ? 0.0015f : 0f;
			}
			if (spawnInfo.SpawnTileType == TileID.BlueDungeonBrick) {
				return spawnInfo.Player.ZoneDungeon ? 0.04f : 0f;
			}
			return 0f;
		}
	}
}