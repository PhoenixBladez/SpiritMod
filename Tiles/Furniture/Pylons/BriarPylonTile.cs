using SpiritMod.Items.ByBiome.Briar.Placeable.Furniture;
using Terraria;
using Terraria.GameContent;

namespace SpiritMod.Tiles.Furniture.Pylons
{
	internal class BriarPylonTile : SimplePylonTile<BriarPylonItem>
	{
		internal override string MapKeyName => "Mods.SpiritMod.MapObject.BriarPylon";

		public override bool ValidTeleportCheck_BiomeRequirements(TeleportPylonInfo pylonInfo, SceneMetrics sceneData) => Biomes.BiomeTileCounts.InBriar;
		public override bool IsSold(int npcType, Player player, bool npcHappyEnough) => npcHappyEnough && Biomes.BiomeTileCounts.InBriar;
	}
}