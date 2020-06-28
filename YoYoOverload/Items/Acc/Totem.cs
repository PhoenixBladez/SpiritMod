using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items.Acc
{
	public class Totem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloody Bauble");
			Tooltip.SetDefault("Increases melee speed by 5%\nIncreases damage reduction and damage dealt by 5%\nIncreases critical strike chance by 5%\nAttacks have a small chance to steal life");
		}


		public override void SetDefaults()
		{
			base.item.width = 14;
			base.item.height = 24;
			base.item.rare = ItemRarityID.Orange;
			base.item.UseSound = SoundID.Item11;
			base.item.accessory = true;
			base.item.value = Item.buyPrice(0, 2, 30, 0);
			base.item.value = Item.sellPrice(0, 1, 6, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
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
			player.GetSpiritPlayer().bloodyBauble = true;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
			modRecipe.AddIngredient(ModContent.ItemType<YoyoCharm2>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<MCharm>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<CCharm>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<HCharm>(), 1);
			modRecipe.AddTile(TileID.DemonAltar);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
