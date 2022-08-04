using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace SpiritMod.Tiles.Furniture.Pylons
{
	internal class SpiritPylonTile : SimplePylonTile
	{
		internal override int ItemType => ItemID.DirtBlock;
		internal override string MapKeyName => "Mods.SpiritMod.MapObject.SpiritPylon";

		public override bool ValidTeleportCheck_BiomeRequirements(TeleportPylonInfo pylonInfo, SceneMetrics sceneData) => Biomes.BiomeTileCounts.InSpirit;
		public override bool IsSold(int npcType, Player player, bool npcHappyEnough) => npcHappyEnough && Biomes.BiomeTileCounts.InSpirit;
	}
}