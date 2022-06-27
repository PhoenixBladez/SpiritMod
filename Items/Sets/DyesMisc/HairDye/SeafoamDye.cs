using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.DyesMisc.HairDye
{
	public class SeafoamDye : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Seafoam Hair Dye");

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 26;
			Item.maxStack = 99;
			Item.value = Item.buyPrice(gold: 4);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item3;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTurn = true;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.consumable = true;
		}
	}
}