using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo.Rocket.Warhead
{
	class Warhead : ModItem
	{

		public override bool Autoload(ref string name) => false;
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
			item.value = Item.buyPrice(0, 0, 1, 0);
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

	abstract class BaseWarheadProj : ModProjectile //uses a base abstract class to cut down on boilerplate, since the vanilla rocket alts for this ammo dont require any unique ai code
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
			projectile.penetrate = -1;
			if (usesaitype)
				aiType = CopyProj;
			else
				projectile.aiStyle = -1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//target.immune[projectile.owner] = 30;
			projectile.Kill();
		}

		public override void OnHitPvp(Player target, int damage, bool crit) => projectile.Kill();

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 3; i++) {
				Vector2 spawnoffset = (i == 0) ? projectile.oldVelocity : projectile.oldVelocity.RotatedByRandom(MathHelper.Pi);
				Projectile.NewProjectile(projectile.Center + spawnoffset * 2, Vector2.Zero, ModContent.ProjectileType<WarheadBoom>(),
					projectile.damage, projectile.knockBack, projectile.owner, spawnoffset.ToRotation());
			}

			for (int i = 0; i < 4; i++) {
				Gore gore = Gore.NewGoreDirect(projectile.Center, Main.rand.NextVector2Circular(2, 2), 11 + Main.rand.Next(3), 0.7f);
				gore.timeLeft = 10;
			}

			for (int i = 0; i < 10; i++)
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, Main.rand.NextFloat(-8, 8), Main.rand.NextFloat(-16, -4), 0, default, 1.3f);

			Main.PlaySound(new LegacySoundStyle(soundId: SoundID.Item, style: 14).WithPitchVariance(0.1f), projectile.Center);
		}
	}
	class WarheadProj : BaseWarheadProj
	{
		public WarheadProj() : base(ProjectileID.RocketI) { }
		public override string Texture => mod.Name + "/Items/Ammo/Rocket/Warhead/Warhead";

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			return base.OnTileCollide(oldVelocity);
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			for (int i = 0; i < 3; i++) 
			{
				Dust dust = Dust.NewDustPerfect(projectile.Center - projectile.velocity, 6, projectile.velocity, 50, default, Main.rand.NextFloat(1, 1.5f));
				dust.noGravity = true;
				dust.velocity /= i;
			}

			if (projectile.timeLeft % 15 == 0) 
				DustHelper.DrawCircle(position: projectile.Center, dustType: 6, mainSize: 0.8f, RatioX: 2f, RatioY: 1f, dustSize: 1.4f, rotationAmount: projectile.rotation + MathHelper.TwoPi, nogravity: true);
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

	class Warhead_proximity : BaseWarheadProj
	{
		public Warhead_proximity() : base(ProjectileID.ProximityMineI, true) { }
	}

	class Warhead_grenade : BaseWarheadProj
	{
		public Warhead_grenade() : base(ProjectileID.GrenadeI, true) { }
	}

	class Warhead_snowman : BaseWarheadProj
	{
		public Warhead_snowman() : base(ProjectileID.RocketSnowmanI, true) { }
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			return base.OnTileCollide(oldVelocity);
		}
	}

	class WarheadBoom : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosion");
			Main.projFrames[projectile.type] = 10;
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.Size = new Vector2(40, 40);
			projectile.ranged = true;
			projectile.tileCollide = false;
			projectile.scale = 1.75f;
			projectile.penetrate = -1;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override void AI()
		{
			projectile.rotation = projectile.ai[0] + MathHelper.PiOver2;
			Lighting.AddLight(projectile.position, Color.OrangeRed.ToVector3());
			projectile.frameCounter++;
			if(projectile.frameCounter >= 3) {
				projectile.frameCounter = 0;
				projectile.frame++;
				if (projectile.frame >= Main.projFrames[projectile.type])
					projectile.Kill();
			}
		}

		public override bool? CanHitNPC(NPC target) => projectile.frame <= 2;
	}
}
