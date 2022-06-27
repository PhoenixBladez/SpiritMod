using SpiritMod.Buffs.Potion;
using SpiritMod.Items.Material;
using SpiritMod.Items.Sets.RunicSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Potion
{
	public class RunePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Runescribe Potion");
			Tooltip.SetDefault("Magic attacks may cause enemies to erupt into runes\nIncreases magic damage by 5%");
		}


		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 30;
			Item.rare = ItemRarityID.Pink;
			Item.maxStack = 30;

			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 20;

			Item.consumable = true;
			Item.autoReuse = false;

			Item.buffType = ModContent.BuffType<RunePotionBuff>();
			Item.buffTime = 10800;

			Item.UseSound = SoundID.Item3;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<SoulBloom>(), 1);
			recipe.AddIngredient(ItemID.Fireblossom, 1);
			recipe.AddIngredient(ModContent.ItemType<Rune>(), 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.Register();
		}
	}
}
