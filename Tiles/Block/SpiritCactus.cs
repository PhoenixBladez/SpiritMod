using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class SpiritCactus : ModCactus
	{
		public override void SetStaticDefaults() => GrowsOnTileId = new int[1] { ModContent.TileType<Spiritsand>() };

		public override Asset<Texture2D> GetTexture() => ModContent.Request<Texture2D>("SpiritMod/Tiles/Block/SpiritCactus");
		public override Asset<Texture2D> GetFruitTexture() => null;
	}
}