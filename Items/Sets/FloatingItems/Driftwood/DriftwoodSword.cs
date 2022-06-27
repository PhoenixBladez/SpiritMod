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
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 0, 20);
			Item.rare = ItemRarityID.White;

			Item.damage = 9;
			Item.knockBack = 5f;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 22;
			Item.useAnimation = 22;

			Item.DamageType = DamageClass.Melee;
			Item.autoReuse = false;

			Item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe();
			modRecipe.AddIngredient(ModContent.ItemType<DriftwoodTileItem>(), 16);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.Register();
		}
	}
}
