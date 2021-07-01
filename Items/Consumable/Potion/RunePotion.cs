using SpiritMod.Buffs.Potion;
using SpiritMod.Items.Material;
using SpiritMod.Items.Sets.RunicSet;
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
			item.width = 20;
			item.height = 30;
			item.rare = ItemRarityID.Pink;
			item.maxStack = 30;

			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useTime = item.useAnimation = 20;

			item.consumable = true;
			item.autoReuse = false;

			item.buffType = ModContent.BuffType<RunePotionBuff>();
			item.buffTime = 10800;

			item.UseSound = SoundID.Item3;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<SoulBloom>(), 1);
			recipe.AddIngredient(ItemID.Fireblossom, 1);
			recipe.AddIngredient(ModContent.ItemType<Rune>(), 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
