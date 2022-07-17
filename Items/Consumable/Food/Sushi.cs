
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Fish;

namespace SpiritMod.Items.Consumable.Food
{
	public class Sushi : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sushi");
			Tooltip.SetDefault("Minor improvements to all stats\nProvides underwater breathing\n'Cold and fresh!'");
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
			player.AddBuff(BuffID.Gills, 1800);
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe1 = CreateRecipe(1);
			recipe1.AddIngredient(ModContent.ItemType<Items.Sets.FloatingItems.Kelp>(), 5);
			recipe1.AddIngredient(ModContent.ItemType<RawFish>(), 1);
			recipe1.AddTile(TileID.CookingPots);
			recipe1.Register();
		}
	}
}
