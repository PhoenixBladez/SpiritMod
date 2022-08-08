using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween.Biome
{
	public class CaramelApple : ModItem
	{
		public override void SetStaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats");

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 42;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 0, 0, 50);
			Item.noUseGraphic = false;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 20;
			Item.noMelee = true;
			Item.consumable = true;
			Item.autoReuse = false;
			Item.UseSound = SoundID.Item2;
			Item.buffTime = 4 * 60 * 60;
			Item.buffType = BuffID.WellFed;
		}
	}
}
