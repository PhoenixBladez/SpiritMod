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
			Item.width = 44;
			Item.height = 40;
			Item.value = Item.buyPrice(0, 0, 16, 0);
			Item.rare = ItemRarityID.Blue;

			Item.axe = 12;
			Item.hammer = 50;

			Item.damage = 11;
			Item.knockBack = 5;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 24;
			Item.useAnimation = 24;

			Item.DamageType = DamageClass.Melee;
			Item.useTurn = true;
			Item.autoReuse = true;

			Item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe();
			modRecipe.AddIngredient(ModContent.ItemType<FloranBar>(), 15);
			modRecipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 5);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.Register();
		}

	}
}
