using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	class Omniwrench : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omniwrench");
			Tooltip.SetDefault("Right click to throw ");
		}

		public override void SetDefaults()
		{
			Item.width = 50;
			Item.height = 48;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Item1;

			Item.value = Item.sellPrice(0, 11, 50, 0);
			Item.rare = ItemRarityID.Purple;

			Item.damage = 180;
			Item.knockBack = 7f;
			Item.DamageType = DamageClass.Melee;
			Item.autoReuse = true;
			Item.shootSpeed = 12f;

			Item.pick = 225;
			Item.tileBoost = 5;

			Item.useTime = 9;
			Item.useAnimation = 10;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2) {
				Item.shoot = ModContent.ProjectileType<Projectiles.DonatorItems.Omniwrench>();
				Item.noUseGraphic = true;
				Item.noMelee = true;
			}
			else {
				Item.shoot = ProjectileID.None;
				Item.noUseGraphic = false;
				Item.noMelee = false;
			}
			return player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.DonatorItems.Omniwrench>()] == 0;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MulticolorWrench);
			recipe.AddIngredient(ItemID.ExtendoGrip);
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddIngredient(ItemID.FragmentStardust, 20);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}
