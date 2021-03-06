
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
	public class GorePickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gore Pickaxe");
		}


		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 38;
			item.value = Item.sellPrice(0, 0, 60, 0);
			item.rare = ItemRarityID.LightRed;

			item.pick = 150;

			item.damage = 20;
			item.knockBack = 4f;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 11;
			item.useAnimation = 21;

			item.melee = true;
			item.autoReuse = true;

			item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<FleshClump>(), 6);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.SetResult(this);
			modRecipe.AddRecipe();
		}
	}
}
