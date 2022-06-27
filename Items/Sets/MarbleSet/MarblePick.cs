using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MarbleSet
{
	public class MarblePick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Pickaxe");
		}


		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 38;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;

			Item.pick = 70;

			Item.damage = 16;
			Item.knockBack = 5f;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 19;
			Item.useAnimation = 19;

			Item.DamageType = DamageClass.Melee;
			Item.autoReuse = true;

			Item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe();
			modRecipe.AddIngredient(ModContent.ItemType<MarbleChunk>(), 16);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.Register();
		}
	}
}
