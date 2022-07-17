
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Food
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
			Item.width = Item.height = 22;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 30;
			Item.buffType = BuffID.WellFed;
			Item.buffTime = 108000;
			Item.noMelee = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item3;
			Item.autoReuse = false;
		}

		public override bool CanUseItem(Player player)
		{
			player.AddBuff(BuffID.Warmth, 72000);
			return true;
		}
	}
}
