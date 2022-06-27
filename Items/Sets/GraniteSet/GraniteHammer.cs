using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GraniteSet
{
	public class GraniteHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Hammer");
		}


		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 40;
			Item.value = 8000;
			Item.rare = ItemRarityID.Green;

			Item.hammer = 60;

			Item.damage = 16;
			Item.knockBack = 5;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 28;
			Item.useAnimation = 28;

			Item.DamageType = DamageClass.Melee;
			Item.useTurn = true;
			Item.autoReuse = true;

			Item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<GraniteChunk>(), 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
