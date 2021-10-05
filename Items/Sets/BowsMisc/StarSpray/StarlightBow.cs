using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BowsMisc.StarSpray
{
	public class StarlightBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Spray");
			Tooltip.SetDefault("Converts wooden arrows into speeding stars");
		}

		public override void SetDefaults()
		{
			item.damage = 17;
			item.noMelee = true;
			item.ranged = true;
			item.width = 20;
			item.height = 40;
			item.useTime = 27;
			item.useAnimation = 27;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Starfury;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 3;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.shootSpeed = 11f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.WoodenArrowFriendly)
				type = ProjectileID.StarWrath;// ProjectileID.Starfury;

			int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0, 0);
			Projectile projectile = Main.projectile[proj];

			if (type == ProjectileID.StarWrath)
				projectile.penetrate = 1;

			for (int k = 0; k < 15; k++)
			{
				Vector2 offset = Vector2.Normalize(Main.MouseWorld - player.position) * 23;
				int dust = Dust.NewDust(projectile.Center + offset, projectile.width, projectile.height, DustID.UndergroundHallowedEnemies);
				Main.dust[dust].velocity *= -1f;
				Main.dust[dust].noGravity = true;
				Vector2 vel = Vector2.Normalize(new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101))) * (Main.rand.Next(50, 100) * 0.04f);
				Main.dust[dust].velocity = vel;
				vel.Normalize();
				Main.dust[dust].position = projectile.Center + offset - (vel * 34f);
			}
			return false;
		}
	}
}