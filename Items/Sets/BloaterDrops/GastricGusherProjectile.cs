using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BloaterDrops
{
	public class GastricGusherProjectile : ModProjectile
	{
		//private bool _released = false; //TO BE USED LATER?
		private int _charge = 0;
		private int _endCharge = -1;

		const int MinimumCharge = 30; //How long it takes for a minimum charge - 1/2 second by default

		private float Scaling => ((_endCharge - MinimumCharge) * 0.04f) + 1f; //Scale factor for projectile damage, spread and speed


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gastric Gusher");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;

		}
		public override void SetDefaults()
		{
			projectile.width = 42;
			projectile.height = 24;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.ranged = true;
			projectile.aiStyle = -1;
			//projectile.ownerHitCheck = true;

			drawHeldProjInFrontOfHeldItemAndArms = true;
		}

		public override void AI()
		{
			Player p = Main.player[projectile.owner];

			if (p.whoAmI != Main.myPlayer) return; //mp check (hopefully)

			projectile.Center = p.Center - (Vector2.Normalize(p.MountedCenter - Main.MouseWorld) * 30);
			projectile.rotation = Vector2.Normalize(p.MountedCenter - Main.MouseWorld).ToRotation() - MathHelper.Pi;

			if (!p.channel && _endCharge == -1)
			{
				_endCharge = _charge;
				if (_endCharge > MinimumCharge)
					Fire(p);
			}

			if (p.channel)
			{
				p.direction = Main.MouseWorld.X >= p.MountedCenter.X ? 1 : -1;
			}

			if (_charge > _endCharge + 20 && _endCharge != -1)
				projectile.active = false;

			_charge++;
			projectile.timeLeft++;

			if (_endCharge == -1)
			{
				p.itemTime = 10;
				p.itemAnimation = 10;
			}
		}

		private void Fire(Player p)
		{
			Vector2 vel = Vector2.Normalize(Main.MouseWorld - p.Center) * 9.5f * Scaling;

			for (int i = 4; i < 9; i++)
				Projectile.NewProjectile(p.Center, vel.RotatedByRandom(Scaling * 0.2f) * Main.rand.NextFloat(0.9f, 1.1f), ProjectileID.WoodenArrowFriendly, (int)(10 * Scaling), 0.2f);
		}
	}
}
