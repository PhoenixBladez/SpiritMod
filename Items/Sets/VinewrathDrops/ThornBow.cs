using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.VinewrathDrops
{
	public class ThornBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thornshot");
			Tooltip.SetDefault("Wooden arrows split into poisonous thorns");
		}

		public override void SetDefaults()
		{
			item.damage = 19;
			item.noMelee = true;
			item.ranged = true;
			item.width = 22;
			item.height = 56;
			item.useTime = 28;
			item.useAnimation = 28;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Shuriken;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 1.5f;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item5;
			item.value = Item.sellPrice(gold: 2, silver: 30);
			item.autoReuse = true;
			item.shootSpeed = 14f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if(type == ProjectileID.WoodenArrowFriendly) type = ModContent.ProjectileType<ThornArrow>();
			return true;
		}

		public class ThornArrow : ModProjectile
		{
			public override string Texture => "Terraria/Projectile_" + ProjectileID.WoodenArrowFriendly;

			public override void SetStaticDefaults() => DisplayName.SetDefault("Thorn Arrow");

			public override void SetDefaults()
			{
				projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
				aiType = ProjectileID.WoodenArrowFriendly;
			}

			public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
			{
				if(Main.rand.NextBool(4)) {
					int n = Main.rand.Next(5, 6);
					int deviation = Main.rand.Next(0, 300);
					for(int i = 0; i < n; i++) {
						float rotation = MathHelper.ToRadians(270 / n * i + deviation);
						Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(rotation);
						perturbedSpeed.Normalize();
						perturbedSpeed.X *= 3.5f;
						perturbedSpeed.Y *= 3.5f;
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<ThornBowThorn>(), projectile.damage / 5 * 3, projectile.knockBack, projectile.owner);
					}
				}
			}
		}
	}
}