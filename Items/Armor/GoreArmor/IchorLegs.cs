
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.GoreArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class IchorLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gore Leggings");
			Tooltip.SetDefault("10% increased movement speed\n6% increased melee critical strike chance");
		}
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 90, 0);
			item.rare = 4;

			item.defense = 9;
		}

		public override void UpdateEquip(Player player)
		{
			player.meleeCrit += 6;
			player.moveSpeed += 0.1f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FleshClump>(), 9);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
