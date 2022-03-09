using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FloatingItems.Driftwood
{
	public class DriftwoodSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Driftwood Sword");
		}


		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 0, 20);
			item.rare = ItemRarityID.White;

			item.damage = 9;
			item.knockBack = 5f;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 22;
			item.useAnimation = 22;

			item.melee = true;
			item.autoReuse = false;

			item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<DriftwoodTileItem>(), 16);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this);
			modRecipe.AddRecipe();
		}
	}
}
