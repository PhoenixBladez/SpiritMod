using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.AtlasDrops
{
	public class ArcaneGeyser : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Geyser");
			Tooltip.SetDefault("'The rock overflows with energy'");
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 16;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.Cyan;
			Item.value = Item.sellPrice(0, 0, 15, 0);
		}
	}
}
