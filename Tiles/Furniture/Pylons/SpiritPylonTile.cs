using SpiritMod.Items.ByBiome.Spirit.Placeables.Furniture;
using Terraria;
using Terraria.GameContent;

namespace SpiritMod.Tiles.Furniture.Pylons
{
	internal class SpiritPylonTile : SimplePylonTile<SpiritPylonItem>
	{
		internal override string MapKeyName => "Mods.SpiritMod.MapObject.SpiritPylon";

		public override bool ValidTeleportCheck_BiomeRequirements(TeleportPylonInfo pylonInfo, SceneMetrics sceneData) => Biomes.BiomeTileCounts.InSpirit;
		public override bool IsSold(int npcType, Player player, bool npcHappyEnough) => npcHappyEnough && Biomes.BiomeTileCounts.InSpirit;
	}
}