using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
	public class SpiritKoi : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Koi");
			Tooltip.SetDefault("'Is it past its expiry date?'");
		}


		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 42;
			Item.value = 100;
			Item.rare = ItemRarityID.LightRed;

			Item.maxStack = 999;
		}
	}
}
