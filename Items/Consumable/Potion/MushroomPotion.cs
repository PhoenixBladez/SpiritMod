using SpiritMod.Buffs.Potion;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Potion
{
	public class MushroomPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sporecoid Potion");
			Tooltip.SetDefault("Causes the player to leave behind a damaging trail of mushrooms");
		}


		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 30;

			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 20;

			Item.consumable = true;
			Item.autoReuse = false;

			Item.buffType = ModContent.BuffType<MushroomPotionBuff>();
			Item.buffTime = 7300;

			Item.UseSound = SoundID.Item3;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<GlowRoot>(), 1);
			recipe.AddIngredient(ItemID.GlowingMushroom, 1);
			recipe.AddIngredient(ItemID.Moonglow, 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.Register();
		}
	}
}
