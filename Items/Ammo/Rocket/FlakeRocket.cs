using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo.Rocket
{
	class FlakeRocket : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flake Rocket");
			Tooltip.SetDefault("'Put them on ice'");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.RocketI);
			item.width = 26;
			item.height = 14;
			item.value = Item.buyPrice(0, 0, 0, 40);
			item.rare = ItemRarityID.Orange;
			item.damage = 24;
			item.shoot = ModContent.ProjectileType<FlakeRocketProj>();
		}

		public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref int damage, ref float knockback) => type = item.shoot;

		public override void AddRecipes()
		{
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<CryoliteBar>(), 1);
			recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
	}
}
