using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BismiteSet.BismiteArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class BismiteChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Chestplate");
			Tooltip.SetDefault("3% increased critical strike chance");

		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.value = Item.buyPrice(silver: 8);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 3;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetCritChance(DamageClass.Generic) += 3;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
