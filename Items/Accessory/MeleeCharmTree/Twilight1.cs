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
			Item.width = 14;
			Item.height = 24;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item11;
			Item.accessory = true;
			Item.value = Item.buyPrice(0, 2, 30, 0);
		}

		public override void SafeUpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Generic) += 0.05f;
			player.GetCritChance(DamageClass.Generic) += 5;
			player.endurance += 0.05f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ModContent.ItemType<YoyoCharm2>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<MCharm>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<DCharm>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<HCharm>(), 1);
			modRecipe.AddTile(TileID.DemonAltar);
			modRecipe.Register();
		}
	}
}
