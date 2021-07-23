using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.DyesMisc.HairDye
{
	public class BrightbloodDye : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Brightblood Hair Dye");

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 26;
			item.maxStack = 99;
			item.value = Item.buyPrice(gold: 7);
			item.rare = 4;
			item.UseSound = SoundID.Item3;
			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useTurn = true;
			item.useAnimation = 17;
			item.useTime = 17;
			item.consumable = true;
		}
	}
}