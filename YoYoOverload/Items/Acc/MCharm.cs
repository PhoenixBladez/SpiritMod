using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items.Acc
{
	public class MCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Marrow Pendant");
			Tooltip.SetDefault("Reduces damage taken by 5%");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 24;
			item.rare = ItemRarityID.Green;
			item.defense = 2;
			item.UseSound = SoundID.Item11;
			item.accessory = true;
			item.value = Item.buyPrice(0, 0, 30, 0);
			item.value = Item.sellPrice(0, 0, 6, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.endurance += 0.05f;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ItemID.Bone, 25);
			modRecipe.AddIngredient(ItemID.Chain, 3);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
