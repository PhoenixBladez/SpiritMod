using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
	public class CoconutGun : ModItem

	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coconut Gun");
			Tooltip.SetDefault("'Fires in Spurts' \n'If it shoots ya, it's gonna hurt!'");
		}

		public override void SetDefaults()
		{
			item.damage = 26;
			item.ranged = true;
			item.width = 65;
			item.height = 21;
			item.useTime = 46;
			item.useAnimation = 46;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 7.5f;
			item.useTurn = true;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<CoconutSpurt>();
			item.shootSpeed = 1f;
			item.crit = 2;
			item.UseSound = SoundID.Item61;
			// item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-7, 0);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int i = 0; i < 3; i++) {
				Projectile.NewProjectile(position.X, position.Y, (speedX * Main.rand.NextFloat(25, 30) / 4) + (Main.rand.NextFloat(-1, 2) * .66f), (speedY * Main.rand.Next(25, 30) / 4) + (Main.rand.Next(-1, 2) * .66f), ModContent.ProjectileType<CoconutSpurt>(), damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}
}
