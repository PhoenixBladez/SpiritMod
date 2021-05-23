

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
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
			item.width = 8;
			item.height = 16;
			item.value = 1000;
			item.rare = ItemRarityID.Blue;
			item.value = Item.buyPrice(0, 0, 0, 40);

			item.maxStack = 999;

			item.damage = 7;
			item.knockBack = 1.5f;
			item.ammo = AmmoID.Bullet;

			item.ranged = true;
			item.consumable = true;

			item.shoot = ModContent.ProjectileType<RipperSlugProj>();
			item.shootSpeed = 3f;

		}
		public override void AddRecipes()
		{
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MusketBall, 50);
			recipe.AddIngredient(ItemID.TissueSample, 1);
			recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }
	}
	public class RipperSlugProj : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Ripper Slug");

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			projectile.width = 2;
			projectile.height = 2;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 600;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			projectile.hide = true;
			projectile.extraUpdates = 1;
		}
		bool primsCreated = false;
		public override void AI()
		{
			if (!primsCreated)
			{
				primsCreated = true;
				SpiritMod.primitives.CreateTrail(new RipperPrimTrail(projectile));
			}
			projectile.velocity.X *= 0.995f;
			projectile.velocity.Y += 0.05f;
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(new LegacySoundStyle(SoundID.NPCHit, 8).WithPitchVariance(0.2f).WithVolume(0.3f), projectile.Center);
			for (int i = 0; i < 20; i++)
			{
				Dust.NewDustPerfect(projectile.Center, 5, Main.rand.NextFloat(0.25f,0.5f) * projectile.velocity.RotatedBy(3.14f + Main.rand.NextFloat(-0.4f,0.4f)));
			}
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => damage = (int)(damage * 1.15f);

		public override void ModifyHitPvp(Player target, ref int damage, ref bool crit) => damage = (int)(damage * 1.15f);
	}
}