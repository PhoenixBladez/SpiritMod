using SpiritMod.Items.Material;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	public class DodgeBall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lightning Throw");
			Tooltip.SetDefault("Throw mach speed dodgeballs!");
		}


		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 0;
			Item.value = 4000;
			Item.rare = ItemRarityID.Green;
			Item.shootSpeed = 8f;
			Item.shoot = ModContent.ProjectileType<Dodgeball>();
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Silk, 11);
			recipe.AddIngredient(ModContent.ItemType<OldLeather>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ModContent.ItemType<DodgeBall1>());
			recipe2.Register();
		}
	}
}