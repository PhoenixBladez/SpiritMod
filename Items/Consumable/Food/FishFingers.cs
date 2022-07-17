using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Food
{
	public class FishFingers : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fish Fingers");
			Tooltip.SetDefault("'Goes great with custard!'");
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 30;
			Item.buffType = BuffID.WellFed;
			Item.buffTime = 72000;
			Item.noMelee = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item2;
			Item.autoReuse = false;
		}
	}
}
