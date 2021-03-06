using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.WitherArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class WitherPlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wither Chestplate");
			Tooltip.SetDefault("Increases damage by 18% and movement speed by 10%\nIncreases life regeneration");
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = 120000;
			item.rare = ItemRarityID.Yellow;
			item.defense = 23;
		}

		public override void UpdateEquip(Player player)
		{
			player.magicDamage += 0.18f;
			player.meleeDamage += 0.18f;
			player.rangedDamage += 0.18f;
			player.minionDamage += 0.18f;
			player.maxRunSpeed += 0.1f;

			player.lifeRegen += 3;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<NightmareFuel>(), 16);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
