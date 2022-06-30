using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace SpiritMod.NPCs.DungeonCube
{
	public class DungeonCubePink : BaseDungeonCube
	{
		public override void SetDefaults()
		{
			base.SetDefaults();

			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.PinkDungeonCubeBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.downedPlantBoss)
			{
				return spawnInfo.Player.ZoneDungeon && NPC.CountNPCS(ModContent.NPCType<DungeonCubePink>()) < 1 ? 0.0015f : 0f;
			}
			if (spawnInfo.SpawnTileType == 44) {
				return spawnInfo.Player.ZoneDungeon ? 0.04f : 0f;
			}
			return 0f;
		}

		protected override int TileDrop => TileID.PinkDungeonBrick;
		protected override string CubeColor => "Pink";
	}
}