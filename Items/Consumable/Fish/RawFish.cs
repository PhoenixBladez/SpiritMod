
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Fish
{
	public class RawFish : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Raw Fish");
			Tooltip.SetDefault("'Can be eaten... Maybe cook it first?'");
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
			Item.buffTime = 18000;
			Item.noMelee = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item2;
			Item.autoReuse = false;

		}
		public override bool CanUseItem(Player player)
		{
			player.AddBuff(BuffID.Poisoned, 600);
			return true;
		}
		public override void AddRecipes()
		{
			Recipe recipe1 = Recipe.Create(ItemID.CookedFish, 1);
			recipe1.AddIngredient(ModContent.ItemType<RawFish>(), 1);
			recipe1.AddTile(TileID.CookingPots);
			recipe1.Register();

			Recipe recipe2 = Recipe.Create(ItemID.Sashimi, 1);
			recipe2.AddIngredient(ModContent.ItemType<RawFish>(), 1);
			recipe2.AddTile(TileID.WorkBenches);
			recipe2.Register();
		}
	}
}
