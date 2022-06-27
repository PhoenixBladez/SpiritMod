using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;
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
			Item.CloneDefaults(ItemID.RocketI);
			Item.width = 26;
			Item.height = 14;
			Item.value = Item.buyPrice(0, 0, 0, 30);
			Item.rare = ItemRarityID.White;
			Item.damage = 10;
			Item.shoot = ModContent.ProjectileType<WarheadProj>();
			Item.shootSpeed = 6;
		}

		public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
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
					type = Item.shoot;
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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(CopyProj);
			Projectile.penetrate = 1;
			if (usesaitype)
				AIType = CopyProj;
			else
				Projectile.aiStyle = -1;
		}

		public override void ExplodeEffect()
		{
			SoundEngine.PlaySound(SoundID.Item14 with { PitchVariance = 0.1f }, Projectile.Center);
			for (int i = 0; i < 10; i++)
			{
				Vector2 offset = Main.rand.NextVector2Circular(50, 50);
				ParticleHandler.SpawnParticle(new WarheadBoom(Projectile.Center + offset, Main.rand.NextFloat(1f, 1.4f), offset.ToRotation() + MathHelper.PiOver2));
			}

			for (int i = 0; i < 20; i++)
			{
				float maxDist = 70;
				float Dist = Main.rand.NextFloat(maxDist);
				Vector2 offset = Main.rand.NextVector2Unit();
				ParticleHandler.SpawnParticle(new SmokeParticle(Projectile.Center + (offset * Dist), Main.rand.NextFloat(5f) * offset * (1 - (Dist / maxDist)), new Color(60, 60, 60) * 0.5f, Main.rand.NextFloat(0.4f, 0.6f), 40));
			}

			for (int i = 0; i < 6; i++)
				ParticleHandler.SpawnParticle(new FireParticle(Projectile.Center, (Projectile.velocity / 2) - (Vector2.UnitY.RotatedByRandom(MathHelper.PiOver2 * 3) * Main.rand.NextFloat(6)),
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
		private int AiTimer => maxTimeLeft - Projectile.timeLeft;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.timeLeft = maxTimeLeft;
		}

		public override string Texture => Mod.Name + "/Items/Ammo/Rocket/Warhead/Warhead";

		public void DoTrailCreation(TrailManager tM) => tM.CreateTrail(Projectile, new GradientTrail(Color.Lerp(Color.Orange, Color.White, 0.7f) * 0.2f, Color.Transparent), new RoundCap(), new DefaultTrailPosition(), 30, 100);

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (AiTimer < 30)
				Projectile.velocity *= 1.023f;
			if (!Main.dedServ)
			{
				ParticleHandler.SpawnParticle(new FireParticle(Projectile.Center, -Projectile.velocity.RotatedByRandom(MathHelper.Pi / 32) * Main.rand.NextFloat(),
							new Color(246, 255, 0), new Color(232, 37, 2), Main.rand.NextFloat(0.35f, 0.5f), 7, delegate (Particle particle)
							{
								particle.Velocity *= 0.94f;
							}));

				for(int i = 0; i < 2; i++)
					ParticleHandler.SpawnParticle(new SmokeParticle(Projectile.Center, -Projectile.velocity.RotatedByRandom(MathHelper.Pi / 16) * Main.rand.NextFloat(0.25f), new Color(60, 60, 60) * 0.5f, Main.rand.NextFloat(0.2f, 0.3f), 20));
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			return base.OnTileCollide(oldVelocity);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			for(int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++) {
				Vector2 drawpos = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
				float opacity = (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
				opacity *= 0.4f;
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawpos, null, Projectile.GetAlpha(lightColor) * opacity, Projectile.rotation, Projectile.Size / 2, Projectile.scale, SpriteEffects.None, 0);
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
			Projectile.Kill();
			return base.OnTileCollide(oldVelocity);
		}
	}
}
