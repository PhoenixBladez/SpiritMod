using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Food
{
	public class RockCandy : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rock Candy");
			Tooltip.SetDefault("'Sweet and incredibly tough!'");
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.rare = ItemRarityID.Pink;
			Item.maxStack = 99;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 30;
			Item.buffType = BuffID.Endurance;
			Item.buffTime = 9600;
			Item.noMelee = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item2;
			Item.autoReuse = false;
		}
	}
}
