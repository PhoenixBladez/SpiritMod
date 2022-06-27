
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Fish
{
	public class HoneySalmon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Honey-Glazed Salmon");
			Tooltip.SetDefault("Minor improvements to all stats\nBoosts life regeneration");
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
			Item.buffTime = 36000;
			Item.noMelee = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item2;
			Item.autoReuse = false;

		}
		public override bool CanUseItem(Player player)
		{
			player.AddBuff(48, 1800);
			return true;
		}
		public override void AddRecipes()
		{
			Recipe recipe1 = CreateRecipe(1);
			recipe1.AddIngredient(ModContent.ItemType<RawFish>(), 2);
			recipe1.AddIngredient(ItemID.BottledHoney, 1);
			recipe1.AddTile(TileID.CookingPots);
			recipe1.Register();
		}
	}
}
