using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Returning;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BismiteSet
{
	public class BismiteChakra : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Cutter");
			Tooltip.SetDefault("Occasionally causes foes to receive 'Festering Wounds,' which deal more damage to enemies under half health");
		}

		public override void SetDefaults()
		{
			Item.damage = 9;
			Item.DamageType = DamageClass.Melee;
			Item.width = 30;
			Item.height = 28;
			Item.useTime = 28;
			Item.useAnimation = 25;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 2;
			Item.value = Terraria.Item.sellPrice(0, 0, 12, 0);
			Item.rare = ItemRarityID.Blue;
			Item.shootSpeed = 11f;
			Item.shoot = ModContent.ProjectileType<BismiteCutter>();
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
		}

		public override bool CanUseItem(Player player)       //this make that you can shoot only 1 boomerang at once
		{
			for (int i = 0; i < Main.maxProjectiles; ++i) {
				if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Item.shoot)
					return false;
			}
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}