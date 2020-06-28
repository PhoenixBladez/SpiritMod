using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
	public class TatteredShotbow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tattered Shotbow");
			Tooltip.SetDefault("Shoots 2 arrows with high armor penetration");
		}



		public override void SetDefaults()
		{
			item.damage = 39;
			item.noMelee = true;
			item.ranged = true;
			item.width = 20;
			item.height = 38;
			item.useTime = 15;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Shuriken;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 5;
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item5;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.autoReuse = false;
			item.shootSpeed = 10f;
			item.crit = 8;

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<MarbleArrow>(), damage, knockBack, player.whoAmI, 0f, 0f);
			return false;
		}
	}
}