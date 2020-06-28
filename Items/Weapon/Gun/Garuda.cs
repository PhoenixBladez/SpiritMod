using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
	public class Garuda : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Garuda");
			Tooltip.SetDefault("Bullets shot may summon celestial energy from the sky upon hitting foes");
		}

		public override void SetDefaults()
		{
			item.damage = 47;
			item.ranged = true;
			item.width = 50;
			item.height = 28;
			item.useTime = 8;
			item.useAnimation = 8;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 4f;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item36;
			item.autoReuse = true;
			item.shoot = ProjectileID.CrystalBullet;
			item.shootSpeed = 11f;
			item.useAmmo = AmmoID.Bullet;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int projectileFired = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, item.damage, item.knockBack, player.whoAmI);
			Main.projectile[projectileFired].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromGaruda = true;
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<WorshipCrystal>(), 13);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
}
