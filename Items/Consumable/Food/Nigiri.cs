
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
			item.width = item.height = 22;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 99;
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useTime = item.useAnimation = 30;

			item.buffType = BuffID.WellFed;
			item.buffTime = 18000;
			item.noMelee = true;
			item.consumable = true;
			item.UseSound = SoundID.Item2;
			item.autoReuse = false;

		}
		public override bool CanUseItem(Player player)
		{
			player.AddBuff(BuffID.Flipper, 3600);
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(ModContent.ItemType<Items.Sets.FloatingItems.Kelp>(), 7);
			recipe1.AddIngredient(ModContent.ItemType<RawFish>(), 1);
			recipe1.AddTile(TileID.CookingPots);
			recipe1.SetResult(this, 1);
			recipe1.AddRecipe();
		}
	}
}
