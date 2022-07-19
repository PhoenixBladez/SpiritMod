using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
	public class SoulBloom : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Soulbloom");

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
			Item.value = 100;
			Item.rare = ItemRarityID.Pink;
			Item.maxStack = 999;
		}
	}
}
