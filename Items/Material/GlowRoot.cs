using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
	public class GlowRoot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glowroot");
		}


		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = 300;
			Item.rare = ItemRarityID.Blue;

			Item.maxStack = 999;
		}
	}
}