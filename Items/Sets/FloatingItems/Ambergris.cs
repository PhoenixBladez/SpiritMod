using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Ambient.Kelp;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FloatingItems
{
	public class Ambergris : FloatingItem
	{
		public override float Weight => base.Weight * 0.9f;
		public override float Bouyancy => base.Bouyancy * 1.08f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ambergris");
			Tooltip.SetDefault("'Highly valuable'\n'You don't want to know where this came from'");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 24;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Terraria.Item.sellPrice(0, 15, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.maxStack = 999;
		}
	}
}