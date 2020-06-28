using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
	public class EternityEssence : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Essence of Eternity");
			Tooltip.SetDefault("'The breath of Spirit, forever.'");
		}


		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.value = 8000;
			item.rare = ItemRarityID.Purple;

			item.maxStack = 999;
		}
	}
}