
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Food
{
	public class TofuSatay : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tofu Satay");
			Tooltip.SetDefault("Minor improvements to all stats\n'Fresh and fried!'");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 22;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 99;
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useTime = item.useAnimation = 30;

			item.buffType = BuffID.WellFed;
			item.buffTime = 180000;
			item.noMelee = true;
			item.consumable = true;
			item.UseSound = SoundID.Item3;
			item.autoReuse = false;

		}
	}
}
