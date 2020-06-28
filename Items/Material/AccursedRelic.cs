using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
	public class AccursedRelic : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Accursed Relic");
			Tooltip.SetDefault("'Fragments from the past'");
		}


		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.value = 100;
			item.rare = ItemRarityID.Purple;
			item.maxStack = 999;
		}
	}
}
