using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Prim;
using Terraria.Audio;

namespace SpiritMod.Items.Ammo.Bullet
{
	public class RipperSlug : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ripper Slug");
			Tooltip.SetDefault("Obeys gravity, but does extra damage");
		}

		public override void SetDefaults()
		{
			Item.width = 8;
			Item.height = 16;
			Item.value = 1000;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 0, 0, 40);
			Item.maxStack = 999;
			Item.damage = 8;
			Item.knockBack = 1.5f;
			Item.ammo = AmmoID.Bullet;
			Item.DamageType = DamageClass.Ranged;
			Item.consumable = true;
			Item.shoot = ModContent.ProjectileType<RipperSlugProj>();
			Item.shootSpeed = 3f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(50);
			recipe.AddIngredient(ItemID.MusketBall, 50);
			recipe.AddIngredient(ItemID.TissueSample, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}

	public class RipperSlugProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ripper Slug");

			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.hide = true;
			Projectile.extraUpdates = 1;
		}

		bool primsCreated = false;

		public override void AI()
		{
			if (!primsCreated)
			{
				primsCreated = true;
				SpiritMod.primitives.CreateTrail(new RipperPrimTrail(Projectile));
			}
			Projectile.velocity.X *= 0.995f;
			Projectile.velocity.Y += 0.05f;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCHit8 with { PitchVariance = 0.2f, Volume = 0.3f }, Projectile.Center);

			for (int i = 0; i < 20; i++)
				Dust.NewDustPerfect(Projectile.Center, 5, Main.rand.NextFloat(0.25f, 0.5f) * Projectile.velocity.RotatedBy(3.14f + Main.rand.NextFloat(-0.4f, 0.4f)));
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => damage = (int)(damage * 1.15f);

		public override void ModifyHitPvp(Player target, ref int damage, ref bool crit) => damage = (int)(damage * 1.15f);
	}
}