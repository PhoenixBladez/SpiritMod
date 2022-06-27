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
			Item.width = 14;
			Item.height = 20;
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 99;
			Item.autoReuse = false;

		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

	}
}
