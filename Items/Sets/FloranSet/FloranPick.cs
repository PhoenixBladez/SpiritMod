using SpiritMod.Items.Sets.BriarDrops;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FloranSet
{
	public class FloranPick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Pickaxe");
		}


		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 42;
			Item.value = 1000;
			Item.rare = ItemRarityID.Blue;

			Item.pick = 55;

			Item.damage = 12;
			Item.knockBack = 3;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 15;
			Item.useAnimation = 25;

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
