using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.InfernonDrops
{
	public class InfernalAppendage : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torment Shard");
			Tooltip.SetDefault("'Filled with suffering'");
		}


		public override void SetDefaults()
		{
			Item.width = Item.height = 16;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.LightRed;
		}
	}
}
