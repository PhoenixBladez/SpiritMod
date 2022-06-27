
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Food
{
	public class Baguette : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Baguette");
			Tooltip.SetDefault("Minor improvements to all stats\n'You feel fancier already'");
		}


		public override void SetDefaults()
		{
			Item.width = Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 30;
			Item.value = Terraria.Item.sellPrice(0, 0, 0, 10);
			Item.buffType = BuffID.WellFed;
			Item.buffTime = 54000;
			Item.noMelee = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item2;
			Item.autoReuse = false;

		}
	}
}
