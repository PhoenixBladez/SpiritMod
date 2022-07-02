using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.MeleeCharmTree
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
			base.Item.width = 14;
			base.Item.height = 24;
			base.Item.rare = ItemRarityID.Orange;
			base.Item.UseSound = SoundID.Item11;
			base.Item.accessory = true;
			base.Item.value = Item.buyPrice(0, 2, 30, 0);
			base.Item.value = Item.sellPrice(0, 1, 6, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Generic) += 0.05f;
			player.GetCritChance(DamageClass.Generic) += 5;
			player.endurance += 0.05f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
			player.GetSpiritPlayer().bloodyBauble = true;
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = Recipe.Create(this.Type, 1);
			modRecipe.AddIngredient(ModContent.ItemType<YoyoCharm2>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<MCharm>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<CCharm>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<HCharm>(), 1);
			modRecipe.AddTile(TileID.DemonAltar);
			modRecipe.Register();
		}
	}
}
