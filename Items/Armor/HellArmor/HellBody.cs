using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.HellArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class HellBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Malevolent Platemail");
			Tooltip.SetDefault("15% increased movement speed\n8% increased ranged critical strike chance");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 18;
			item.value = Item.buyPrice(gold: 4, silver: 60);
			item.rare = ItemRarityID.LightPurple;
			item.defense = 20;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.15f;
			player.rangedCrit += 8;
			player.maxRunSpeed += 1;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FieryEssence>(), 20);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.EssenceDistorter>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}