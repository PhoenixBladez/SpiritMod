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
			item.width = 22;
			item.height = 22;
			item.rare = ItemRarityID.Pink;

			item.maxStack = 999;
		}
	}
}
