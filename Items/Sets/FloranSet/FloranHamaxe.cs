using SpiritMod.Items.Sets.BriarDrops;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FloranSet
{
	public class FloranHamaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Hamaxe");
		}


		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 40;
			item.value = Item.buyPrice(0, 0, 16, 0);
			item.rare = ItemRarityID.Blue;

			item.axe = 12;
			item.hammer = 50;

			item.damage = 11;
			item.knockBack = 5;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 24;
			item.useAnimation = 24;

			item.melee = true;
			item.useTurn = true;
			item.autoReuse = true;

			item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<FloranBar>(), 15);
			modRecipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 5);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this);
			modRecipe.AddRecipe();
		}

	}
}
