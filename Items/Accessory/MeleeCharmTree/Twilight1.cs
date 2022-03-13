using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.MeleeCharmTree
{
	public class Twilight1 : AccessoryItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Talisman");
			Tooltip.SetDefault("Increases melee speed by 5%\nIncreases damage reduction and damage dealt by 5%\nIncreases critical strike chance by 4%\nAttacks have a small chance of inflicting Shadowflame");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item11;
			item.accessory = true;
			item.value = Item.buyPrice(0, 2, 30, 0);
		}

		public override void SafeUpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeDamage += 0.05f;
			player.magicDamage += 0.05f;
			player.rangedDamage += 0.05f;
			player.minionDamage += 0.05f;
			player.thrownDamage += 0.05f;
			player.meleeCrit += 5;
			player.rangedCrit += 5;
			player.magicCrit += 5;
			player.thrownCrit += 5;
			player.endurance += 0.05f;
			player.meleeSpeed += 0.05f;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<YoyoCharm2>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<MCharm>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<DCharm>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<HCharm>(), 1);
			modRecipe.AddTile(TileID.DemonAltar);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
