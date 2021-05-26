using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Prim;

namespace SpiritMod.Items.Weapon.Bow.ArtemisHunt
{
	public class ArtemisHunt : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Artemis's Hunt");
			Tooltip.SetDefault("Converts wooden arrows into Celestial Arrows\nCelestial Arrows create damaging crescents when hitting enemies");
		}

		public override void SetDefaults()
		{
			item.damage = 42;
			item.noMelee = true;
			item.ranged = true;
			item.width = 20;
			item.height = 38;
			item.useTime = 17;
			item.useAnimation = 17;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Shuriken;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 3;
			item.UseSound = SoundID.Item5;
			item.rare = 4;
			item.value = Item.sellPrice(gold: 2);
			item.autoReuse = true;
			item.shootSpeed = 12f;


		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int p = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<ArtemisHuntArrow>(), damage, knockBack, player.whoAmI);
			return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 0);
		}
	}
	public class ArtemisHuntArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestial Arrow");
		}

		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 10;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.extraUpdates = 1;
			projectile.timeLeft = 600;
			projectile.light = 0.5f;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			aiType = ProjectileID.WoodenArrowFriendly;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int proj = Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<ArtemisCrescent>(), (int)(projectile.damage * 0.66f), 0, projectile.owner);
			float offset = Main.rand.NextFloat(0.5f) * Main.rand.NextBool().ToDirectionInt();
			if (Main.projectile[proj].modProjectile is ArtemisCrescent modProj)
			{
				switch (Main.rand.Next(2))
				{
					case 0:
						modProj.start = projectile.Center - (new Vector2( 60, 60).RotatedBy(projectile.velocity.ToRotation() + offset));
						modProj.c2 = projectile.Center - (new Vector2(-38, -60).RotatedBy(projectile.velocity.ToRotation() + offset));
						modProj.c1 = projectile.Center - (new Vector2(-38, 60).RotatedBy(projectile.velocity.ToRotation() + offset));
						modProj.end = projectile.Center - (new Vector2( 60,-60).RotatedBy(projectile.velocity.ToRotation() + offset));
						modProj.reverse = false;
						break;
					case 1:
						modProj.end = projectile.Center - (new Vector2( 60, 60).RotatedBy(projectile.velocity.ToRotation() + offset));
						modProj.c2 = projectile.Center - (new Vector2(-38, 60).RotatedBy(projectile.velocity.ToRotation() + offset));
						modProj.c1 = projectile.Center - (new Vector2(-38, -60).RotatedBy(projectile.velocity.ToRotation() + offset));
						modProj.start = projectile.Center - (new Vector2( 60,-60).RotatedBy(projectile.velocity.ToRotation() + offset));
						modProj.reverse = true;
						break;
				}
			}
		}
	}
	public class ArtemisCrescent : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Artemis Crescent");
		}
		public Vector2 start = Vector2.Zero;
		public Vector2 c1 = Vector2.Zero;
		public Vector2 c2 = Vector2.Zero;
		public Vector2 end = Vector2.Zero;
		public bool reverse;
		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.friendly = false;
			projectile.ranged = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 170;
			projectile.light = 0;
			projectile.extraUpdates = 7;
			projectile.tileCollide = false;
			projectile.alpha = 255;
		}
		private bool primsCreated;
		public override void AI()
		{
			if (projectile.timeLeft <= 60)
			{
				Lighting.AddLight(projectile.Center, 0.08f, .28f, .38f);
				if (!primsCreated)
				{
					primsCreated = true;
					SpiritMod.primitives.CreateTrail(new ArtemisPrimTrail(projectile, start, c1, c2, end, reverse));
				}
				projectile.Center = Helpers.TraverseBezier(end, start, c1, c2, projectile.timeLeft / 60f);
				projectile.friendly = true;
			}
		}
	}
}