
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Food
{
	public class Meatballs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meatballs");
			Tooltip.SetDefault("Minor improvements to all stats\n'Can't go wrong with the classics!'");
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
			Item.buffTime = 72000;
			Item.noMelee = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item2;
			Item.autoReuse = false;

		}
		public override bool CanUseItem(Player player)
		{
			player.AddBuff(BuffID.Regeneration, 1800);
			return true;
		}
	}
}
