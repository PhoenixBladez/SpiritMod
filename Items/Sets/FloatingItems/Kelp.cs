using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Ambient.Kelp;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FloatingItems
{
	public class Kelp : FloatingItem
	{
		public override float SpawnWeight => 1.2f;
		public override float Weight => base.Weight * 0.9f;
		public override float Bouyancy => base.Bouyancy * 1.15f;

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
	}
}