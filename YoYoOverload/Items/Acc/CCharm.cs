using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items.Acc
{
	public class CCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grisly Totem");
			Tooltip.SetDefault("Increases critical strike chance by 6%");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 26;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item11;
			item.accessory = true;
			item.value = Item.sellPrice(0, 0, 30, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeCrit += 6;
			player.rangedCrit += 6;
			player.magicCrit += 6;
			player.thrownCrit += 6;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ItemID.Vertebrae, 8);
			modRecipe.AddIngredient(ItemID.TissueSample, 5);
			modRecipe.AddIngredient(ItemID.Chain, 3);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
