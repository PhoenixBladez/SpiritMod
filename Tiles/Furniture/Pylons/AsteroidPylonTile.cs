using SpiritMod.Items.ByBiome.Asteroids.Placeables.Furniture;
using Terraria;
using Terraria.GameContent;

namespace SpiritMod.Tiles.Furniture.Pylons
{
	internal class AsteroidPylonTile : SimplePylonTile<AsteroidPylonItem>
	{
		internal override string MapKeyName => "Mods.SpiritMod.MapObject.AsteroidPylon";

		public override bool ValidTeleportCheck_BiomeRequirements(TeleportPylonInfo pylonInfo, SceneMetrics sceneData) => Biomes.BiomeTileCounts.InAsteroids;
		public override bool IsSold(int npcType, Player player, bool npcHappyEnough) => npcHappyEnough && Biomes.BiomeTileCounts.InAsteroids;
	}
}