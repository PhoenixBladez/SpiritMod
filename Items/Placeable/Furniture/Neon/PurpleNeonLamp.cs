using SpiritMod.Tiles.Furniture.NeonLights;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Material;

namespace SpiritMod.Items.Placeable.Furniture.Neon
{
	public class PurpleNeonLamp : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purple Fluorescent Lantern");
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 28;
			Item.value = Item.value = Terraria.Item.buyPrice(0, 0, 1, 0);
			Item.rare = ItemRarityID.Blue;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<PurpleLightsLarge>();
		}
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(3);
            recipe.AddIngredient(ModContent.ItemType<Items.Material.SynthMaterial>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}