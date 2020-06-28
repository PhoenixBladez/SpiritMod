using SpiritMod.Items.Accessory;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	public class MoonGauntlet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gauntlet of the Moon");
			Tooltip.SetDefault("Increases melee damage by 15% and melee speed by 10%\nAttacks occasionally inflict Ichor, Cursed Inferno, and Daybroken\nMelee Attacks grant you Onyx Whirlwind, which increases movement speed\n'Infused with the Spirit of Meemourne'");
		}


		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.rare = 11;
			item.value = 550000;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeSpeed += 0.10f;
			player.meleeDamage += 0.15f;
			player.GetSpiritPlayer().moonGauntlet = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 5);
			recipe.AddRecipeGroup("SpiritMod:EvilMaterial", 50);
			recipe.AddIngredient(ModContent.ItemType<EternityEssence>(), 5);
			recipe.AddIngredient(ModContent.ItemType<ShadowGauntlet>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
