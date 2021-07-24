using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
		private float _finalRotation = 0f;

		const int MinimumCharge = 30; //How long it takes for a minimum charge - 1/2 second by default

		private float Scaling => ((_charge - MinimumCharge) * 0.015f) + 1f; //Scale factor for projectile damage, spread and speed
		private float ScalingCapped => Scaling >= 4f ? 4f : Scaling; //Cap for scaling so there's not super OP charging lol

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gastric Gusher");
		}

		public override void SetDefaults()
		{
			projectile.width = 42;
			projectile.height = 24;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.ranged = true;
			projectile.aiStyle = -1;

			drawHeldProjInFrontOfHeldItemAndArms = true;
		}

		public override bool CanDamage() => false;

		public override void AI()
		{
			Player p = Main.player[projectile.owner];

			if (p.whoAmI != Main.myPlayer) return; //mp check (hopefully)

			if (!p.channel && _endCharge == -1) //Fire (if possible)
			{
				_endCharge = _charge;
				_finalRotation = ((Vector2.Normalize(p.MountedCenter - Main.MouseWorld) * 30)).ToRotation();
				if (_endCharge > MinimumCharge)
					Fire(p);
			}

			if (p.channel) //Use turn functionality
				p.direction = Main.MouseWorld.X >= p.MountedCenter.X ? 1 : -1;

			if (_charge > _endCharge + 10 && _endCharge != -1) //Kill projectile when done shooting
				projectile.active = false;

			_charge++; //Increase charge timer...
			projectile.timeLeft++; //...and dont die

			if (_endCharge == -1) //Wait until the player has fired to let go & set position
			{
				p.itemTime = 10;
				p.itemAnimation = 10;
				projectile.Center = p.Center - (Vector2.Normalize(p.MountedCenter - Main.MouseWorld) * 30) + new Vector2(21, 12);
			}
			else
				projectile.Center = p.Center - (new Vector2(1, 0).RotatedBy(_finalRotation) * 30) + new Vector2(21, 12);

			projectile.rotation = Vector2.Normalize(p.MountedCenter - Main.MouseWorld).ToRotation() - MathHelper.Pi; //So it looks like the player is holding it properly
		}

		private void Fire(Player p)
		{
			Vector2 vel = Vector2.Normalize(Main.MouseWorld - p.Center) * 9f * (ScalingCapped * 0.8f);
			int inc = 5 + (int)ScalingCapped;

			for (int i = 0; i < inc; i++) //Projectiles
			{
				Vector2 velocity = vel.RotatedByRandom((i - (inc / 2f)) * 0.2f) * Main.rand.NextFloat(0.95f, 1.05f);
				Projectile.NewProjectile(p.Center, velocity, ModContent.ProjectileType<GastricAcid>(), (int)(10 * ScalingCapped), 1f, projectile.owner);
			}

			for (int i = 0; i < p.inventory.Length; ++i) //Consume ammo here so it's used when shot rather than when clicked
			{
				if (p.inventory[i].ammo == AmmoID.Gel)
				{
					p.inventory[i].stack--;
					if (p.inventory[i].stack <= 0)
						p.inventory[i].TurnToAir();
					break;
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player p = Main.player[projectile.owner];
			Texture2D t = Main.projectileTexture[projectile.type];
			SpriteEffects e = Main.MouseWorld.X >= p.MountedCenter.X ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			float realRot = projectile.rotation;
			if (_endCharge != -1) realRot = _finalRotation + MathHelper.Pi;
			if (e == SpriteEffects.FlipHorizontally)
				realRot -= MathHelper.Pi;

			Vector2 drawPos = projectile.position - Main.screenPosition;
			if (_charge > MinimumCharge && _endCharge == -1)
				drawPos += new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)) * ScalingCapped;

			spriteBatch.Draw(t, drawPos, new Rectangle(0, 0, 42, 24), Color.White, realRot, new Vector2(21, 12), 1f, e, 1f);
			return false;
		}
	}
}
