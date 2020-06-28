using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
	public class Brightbulb : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brightbulb");
			Tooltip.SetDefault("'Intricate and versatile'");
		}
		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 20;
			item.rare = ItemRarityID.Green;
			item.maxStack = 99;
			item.autoReuse = false;

		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

	}
}
