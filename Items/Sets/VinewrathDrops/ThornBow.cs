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
			Item.damage = 21;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 22;
			Item.height = 56;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.Shuriken;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 1.5f;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item5;
			Item.value = Item.sellPrice(gold: 2, silver: 30);
			Item.autoReuse = true;
			Item.shootSpeed = 14f;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.WoodenArrowFriendly) 
				type = ModContent.ProjectileType<ThornArrow>();
		}
	}

	public class ThornArrow : ModProjectile
	{
		public override string Texture => "Terraria/Projectile_" + ProjectileID.WoodenArrowFriendly;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Thorn Arrow");

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			AIType = ProjectileID.WoodenArrowFriendly;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(4))
			{
				int n = Main.rand.Next(5, 6);
				int deviation = Main.rand.Next(0, 300);
				for (int i = 0; i < n; i++)
				{
					float rotation = MathHelper.ToRadians(270 / n * i + deviation);
					Vector2 perturbedSpeed = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(rotation)) * 3.5f;
					Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<ThornBowThorn>(), Projectile.damage / 5 * 3, Projectile.knockBack, Projectile.owner);
				}
			}
		}
	}
}