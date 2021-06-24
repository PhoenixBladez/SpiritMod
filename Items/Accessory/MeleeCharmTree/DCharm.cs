using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.MeleeCharmTree
{
	public class DCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vile Charm");
			Tooltip.SetDefault("Increases melee critical strike chance by 5%\nIncreases movement speed by 10%");
		}


		public override void SetDefaults()
		{
			base.item.width = 16;
			base.item.height = 26;
			base.item.rare = ItemRarityID.Green;
			base.item.UseSound = SoundID.Item11;
			base.item.accessory = true;
			base.item.value = Item.buyPrice(0, 0, 30, 0);
			base.item.value = Item.sellPrice(0, 0, 6, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeCrit += 5;
			player.moveSpeed += .1f;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
			modRecipe.AddIngredient(ItemID.RottenChunk, 8);
			modRecipe.AddIngredient(ItemID.ShadowScale, 4);
			modRecipe.AddIngredient(ItemID.Chain, 3);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
