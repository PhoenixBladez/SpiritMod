
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Food
{
	public class Cake : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Carrot Cake");
			Tooltip.SetDefault("Minor improvements to all stats\n'Just the perfect amount of icing!'");
		}


		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 30;

			Item.buffType = BuffID.WellFed;
			Item.buffTime = 54000;
			Item.noMelee = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item2;
			Item.autoReuse = false;

		}
	}
}
