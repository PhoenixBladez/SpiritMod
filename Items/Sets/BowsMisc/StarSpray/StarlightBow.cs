using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

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
			Item.damage = 17;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 20;
			Item.height = 40;
			Item.useTime = 27;
			Item.useAnimation = 27;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.Starfury;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 3;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shootSpeed = 11f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (type == ProjectileID.WoodenArrowFriendly)
				type = ProjectileID.StarWrath;// ProjectileID.Starfury;

			int proj = Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
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