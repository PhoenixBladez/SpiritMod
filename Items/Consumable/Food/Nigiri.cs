
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Fish;

namespace SpiritMod.Items.Consumable.Food
{
	public class Nigiri : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nigiri");
			Tooltip.SetDefault("Minor improvements to all stats\nProvides free movement in water\n'The perfect cut'");
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
			player.AddBuff(BuffID.Flipper, 3600);
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe1 = CreateRecipe(1);
			recipe1.AddIngredient(ModContent.ItemType<Sets.FloatingItems.Kelp>(), 7);
			recipe1.AddIngredient(ModContent.ItemType<RawFish>(), 1);
			recipe1.AddTile(TileID.CookingPots);
			recipe1.Register();
		}
	}
}
