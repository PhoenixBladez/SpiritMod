using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.HellArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class HellLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Malevolent Greaves");
			Tooltip.SetDefault("25% chance to not consume ammo\n10% increased movement speed");
		}

		int timer = 0;
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 18;
			item.value = Item.buyPrice(gold: 4, silver: 60);
			item.rare = ItemRarityID.LightPurple;
			item.defense = 15;
		}

		public override void UpdateEquip(Player player)
		{
			player.ammoCost75 = true;
			player.moveSpeed += 0.1f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FieryEssence>(), 18);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.EssenceDistorter>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}