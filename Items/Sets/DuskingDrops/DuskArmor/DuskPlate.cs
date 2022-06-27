using SpiritMod.Items.Sets.DuskingDrops;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.DuskingDrops.DuskArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class DuskPlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dusk Plate");
			Tooltip.SetDefault("Increases ranged damage by 10%\n25% Chance to not consume ammo");

		}
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 30;
			Item.value = 50000;
			Item.rare = ItemRarityID.Pink;
			Item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			player.rangedDamage = 1.10f;
			player.ammoCost75 = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<DuskStone>(), 16);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}