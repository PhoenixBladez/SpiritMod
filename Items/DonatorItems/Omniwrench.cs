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
			item.width = 50;
			item.height = 48;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.UseSound = SoundID.Item1;

			item.value = Item.sellPrice(0, 11, 50, 0);
			item.rare = ItemRarityID.Purple;

			item.damage = 180;
			item.knockBack = 7f;
			item.melee = true;
			item.autoReuse = true;
			item.shootSpeed = 12f;

			item.pick = 225;
			item.tileBoost = 5;

			item.useTime = 9;
			item.useAnimation = 10;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2) {
				item.shoot = ModContent.ProjectileType<Projectiles.DonatorItems.Omniwrench>();
				item.noUseGraphic = true;
				item.noMelee = true;
			}
			else {
				item.shoot = ProjectileID.None;
				item.noUseGraphic = false;
				item.noMelee = false;
			}
			return player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.DonatorItems.Omniwrench>()] == 0;
		}

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MulticolorWrench);
			recipe.AddIngredient(ItemID.ExtendoGrip);
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddIngredient(ItemID.FragmentStardust, 20);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
