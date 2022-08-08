using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween.Biome
{
	public class JackOGourd : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jack-o-Gourd");
			Tooltip.SetDefault("Minor improvements to all stats...kinda");
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 42;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 0, 0, 50);
			Item.noUseGraphic = false;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 20;
			Item.noMelee = true;
			Item.consumable = true;
			Item.autoReuse = false;
			Item.UseSound = SoundID.Item2;
			Item.buffTime = 4 * 60 * 60;
			Item.buffType = BuffID.WellFed;
		}

		public override bool? UseItem(Player player)
		{
			player.AddBuff(BuffID.OnFire, 3 * 60 * 60);
			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe().
				AddIngredient<TreeGourd>(1).
				AddIngredient(ItemID.Torch, 4).
				Register();

			CreateRecipe(4).
				AddIngredient<TreeGourd>(4).
				AddIngredient(ItemID.LivingFireBlock, 1).
				Register();
		}
	}
}
