
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Fish
{
	public class Ramen : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ramen");
			Tooltip.SetDefault("Minor improvements to all stats\n'It'll warm you right up!'");
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
			item.UseSound = SoundID.Item2;
			item.autoReuse = false;

		}
		public override bool CanUseItem(Player player)
		{
			player.AddBuff(124, 72000);
			return true;
		}
	}
}
