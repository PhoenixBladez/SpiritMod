
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
			Tooltip.SetDefault("'You feel fancier already'");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 30;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 99;
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useTime = item.useAnimation = 30;

			item.buffType = BuffID.WellFed;
			item.buffTime = 54000;
			item.noMelee = true;
			item.consumable = true;
			item.UseSound = SoundID.Item2;
			item.autoReuse = false;

		}
	}
}
