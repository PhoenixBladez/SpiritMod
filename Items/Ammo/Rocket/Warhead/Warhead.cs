using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Utilities;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Items.Ammo.Rocket.Warhead
{
	public class Warhead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Warhead");
			Tooltip.SetDefault("Can be used as ammo for rocket launchers");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.RocketI);
			item.width = 26;
			item.height = 14;
			item.value = Item.buyPrice(0, 0, 0, 30);
			item.rare = ItemRarityID.White;
			item.damage = 10;
			item.shoot = ModContent.ProjectileType<WarheadProj>();
			item.shootSpeed = 6;
		}

		public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref int damage, ref float knockback)
		{
			switch (weapon.type) {
				case ItemID.ProximityMineLauncher:
					type = ModContent.ProjectileType<Warhead_proximity>();
					break;
				case ItemID.GrenadeLauncher:
					type = ModContent.ProjectileType<Warhead_grenade>();
					break;
				case ItemID.SnowmanCannon:
					type = ModContent.ProjectileType<Warhead_snowman>();
					break;
				default:
					type = item.shoot;
					break;
			}
		}
	}

	public abstract class BaseWarheadProj : Projectiles.BaseProj.BaseRocketProj //uses a base abstract class to cut down on boilerplate, since the vanilla rocket alts for this ammo dont require any unique ai code
	{
		public int CopyProj;
		public bool usesaitype;

		public BaseWarheadProj(int CopyProj, bool usesaitype = false)
		{
			this.CopyProj = CopyProj;
			this.usesaitype = usesaitype;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Warhead");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(CopyProj);
			projectile.penetrate = 1;
			if (usesaitype)
				aiType = CopyProj;
			else
				projectile.aiStyle = -1;
		}

		public override void ExplodeEffect()
		{
			Main.PlaySound(new LegacySoundStyle(soundId: SoundID.Item, style: 14).WithPitchVariance(0.1f), projectile.Center);
			for (int i = 0; i < 10; i++)
			{
				Vector2 offset = Main.rand.NextVector2Circular(50, 50);
				ParticleHandler.SpawnParticle(new WarheadBoom(projectile.Center + offset, Main.rand.NextFloat(1f, 1.4f), offset.ToRotation() + MathHelper.PiOver2));
			}

			for (int i = 0; i < 20; i++)
			{
				float maxDist = 70;
				float Dist = Main.rand.NextFloat(maxDist);
				Vector2 offset = Main.rand.NextVector2Unit();
				ParticleHandler.SpawnParticle(new SmokeParticle(projectile.Center + (offset * Dist), Main.rand.NextFloat(5f) * offset * (1 - (Dist / maxDist)), new Color(60, 60, 60) * 0.5f, Main.rand.NextFloat(0.4f, 0.6f), 40));
			}

			for (int i = 0; i < 6; i++)
				ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, (projectile.velocity / 2) - (Vector2.UnitY.RotatedByRandom(MathHelper.PiOver2 * 3) * Main.rand.NextFloat(6)),
					new Color(246, 255, 0), new Color(232, 37, 2), Main.rand.NextFloat(0.5f, 0.7f), 40, delegate (Particle particle)
					{
						if (particle.Velocity.Y < 16)
							particle.Velocity.Y += 0.12f;
					}));
		}
	}

	public class WarheadProj : BaseWarheadProj, ITrailProjectile
	{
		public WarheadProj() : base(ProjectileID.RocketI) { }

		private const int maxTimeLeft = 240;
		private int AiTimer => maxTimeLeft - projectile.timeLeft;
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.timeLeft = maxTimeLeft;
		}

		public override string Texture => mod.Name + "/Items/Ammo/Rocket/Warhead/Warhead";

		public void DoTrailCreation(TrailManager tM) => tM.CreateTrail(projectile, new GradientTrail(Color.Lerp(Color.Orange, Color.White, 0.7f) * 0.2f, Color.Transparent), new RoundCap(), new DefaultTrailPosition(), 30, 100);

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (AiTimer < 30)
				projectile.velocity *= 1.023f;
			if (!Main.dedServ)
			{
				ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, -projectile.velocity.RotatedByRandom(MathHelper.Pi / 32) * Main.rand.NextFloat(),
							new Color(246, 255, 0), new Color(232, 37, 2), Main.rand.NextFloat(0.35f, 0.5f), 7, delegate (Particle particle)
							{
								particle.Velocity *= 0.94f;
							}));

				for(int i = 0; i < 2; i++)
					ParticleHandler.SpawnParticle(new SmokeParticle(projectile.Center, -projectile.velocity.RotatedByRandom(MathHelper.Pi / 16) * Main.rand.NextFloat(0.25f), new Color(60, 60, 60) * 0.5f, Main.rand.NextFloat(0.2f, 0.3f), 20));
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			return base.OnTileCollide(oldVelocity);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for(int i = 0; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i++) {
				Vector2 drawpos = projectile.oldPos[i] + projectile.Size / 2 - Main.screenPosition;
				float opacity = (ProjectileID.Sets.TrailCacheLength[projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[projectile.type];
				opacity *= 0.4f;
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawpos, null, projectile.GetAlpha(lightColor) * opacity, projectile.rotation, projectile.Size / 2, projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}
	}

	public class Warhead_proximity : BaseWarheadProj
	{
		public Warhead_proximity() : base(ProjectileID.ProximityMineI, true) { }
	}

	public class Warhead_grenade : BaseWarheadProj
	{
		public Warhead_grenade() : base(ProjectileID.GrenadeI, true) { }
	}

	public class Warhead_snowman : BaseWarheadProj
	{
		public Warhead_snowman() : base(ProjectileID.RocketSnowmanI, true) { }
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			return base.OnTileCollide(oldVelocity);
		}
	}
}
