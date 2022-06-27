using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritBiomeDrops
{
	public class NetherCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nether Crystal");
			Tooltip.SetDefault("'Brimming with bright souls'");
		}


		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.rare = ItemRarityID.Pink;

			Item.maxStack = 999;
		}
	}
}
