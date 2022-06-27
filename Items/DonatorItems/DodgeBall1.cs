using SpiritMod.Items.Material;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	public class DodgeBall1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sluggy Throw");
			Tooltip.SetDefault("Throw a dodgeball at snail speed");
		}

		public override void SetDefaults()
		{
			Item.damage = 13;
			Item.DamageType = DamageClass.Melee;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 9;
			Item.value = 4000;
			Item.rare = ItemRarityID.Green;
			Item.shootSpeed = 6f;
			Item.shoot = ModContent.ProjectileType<Dodgeball1>();
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Silk, 11);
			recipe.AddIngredient(ModContent.ItemType<OldLeather>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ModContent.ItemType<DodgeBall>(), 1);
			recipe2.Register();
		}
	}
}