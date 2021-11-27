using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Ambient.Kelp;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FloatingItems
{
	public class Kelp : FloatingItem
	{
		public override float Weight => base.Weight * 0.9f;
		public override float Bouyancy => base.Bouyancy * 1.05f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kelp");
			Tooltip.SetDefault("Must be planted in water");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 24;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
			item.createTile = ModContent.TileType<OceanKelp>();
			item.maxStack = 999;
			item.autoReuse = true;
			item.consumable = true;
			item.useAnimation = 15;
			item.useTime = 10;
		}

		public override bool UseItem(Player player)
		{
			Point tPos = Main.MouseWorld.ToTileCoordinates();
			Tile bel = Framing.GetTileSafely(tPos.X, tPos.Y + 1);
			Tile cur = Framing.GetTileSafely(tPos.X, tPos.Y);

			if (bel.active() && bel.type == ModContent.TileType<OceanKelp>() && !cur.active() && cur.liquid > 100)
			{
				WorldGen.PlaceTile(tPos.X, tPos.Y, ModContent.TileType<OceanKelp>(), false, true);
				return true;
			}
			return false;
		}
	}
}