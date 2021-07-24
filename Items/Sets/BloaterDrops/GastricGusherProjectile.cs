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
		private int _charge = 0;
		private int _endCharge = -1;
		private float _finalRotation = 0f;

		const int MinimumCharge = 0; //How long it takes for a minimum charge - 1/2 second by default

		private float Scaling => ((_charge - MinimumCharge) * 0.03f) + 1f; //Scale factor for projectile damage, spread and speed
		private float ScalingCapped => Scaling >= 4f ? 4f : Scaling; //Cap for scaling so there's not super OP charging lol

		public override void SetStaticDefaults() => DisplayName.SetDefault("Gastric Gusher");

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
				_finalRotation = (Vector2.Normalize(p.MountedCenter - Main.MouseWorld) * 27).ToRotation();
				if (_endCharge >= MinimumCharge)
					Fire(p);
			}

			if (p.channel) //Use turn functionality
				p.direction = Main.MouseWorld.X >= p.MountedCenter.X ? 1 : -1;

			if (_charge > _endCharge && _endCharge != -1) //Kill projectile when done shooting - does nothing special but allowed for a cooldown timer before polish
				projectile.active = false;

			_charge++; //Increase charge timer...
			projectile.timeLeft++; //...and dont die

			if (_endCharge == -1) //Wait until the player has fired to let go & set position
			{
				p.itemTime = p.HeldItem.useTime;
				p.itemAnimation = p.HeldItem.useAnimation;
				projectile.Center = p.Center - (Vector2.Normalize(p.MountedCenter - Main.MouseWorld) * 27) + new Vector2(21, 12);
			}
			else
				projectile.Center = p.Center - (new Vector2(1, 0).RotatedBy(_finalRotation) * 27) + new Vector2(21, 12);

			projectile.rotation = Vector2.Normalize(p.MountedCenter - Main.MouseWorld).ToRotation() - MathHelper.Pi; //So it looks like the player is holding it properly
		}

		private void Fire(Player p)
		{
			Vector2 vel = Vector2.Normalize(Main.MouseWorld - p.Center) * 10f * (ScalingCapped * 0.8f);
			int inc = 3 + (int)ScalingCapped;

			for (int i = 0; i < inc; i++) //Projectiles
			{
				Vector2 velocity = vel.RotatedBy((i - (inc / 2f)) * 0.16f) * Main.rand.NextFloat(0.85f, 1.15f);
				Projectile.NewProjectile(p.Center, velocity, ModContent.ProjectileType<GastricAcid>(), (int)(p.HeldItem.damage * ScalingCapped), 1f, projectile.owner);
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

			float realRot = projectile.rotation; //Rotate towards mouse
			if (_endCharge != -1) realRot = _finalRotation + MathHelper.Pi;
			if (e == SpriteEffects.FlipHorizontally)
				realRot -= MathHelper.Pi;

			Vector2 drawPos = projectile.position - Main.screenPosition; //Draw position + charge shaking
			if (_charge > MinimumCharge && _endCharge == -1)
				drawPos += new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)) * (ScalingCapped * 0.75f);

			spriteBatch.Draw(t, drawPos, new Rectangle(0, 0, 42, 24), Color.White, realRot, new Vector2(21, 12), 1f, e, 1f);
			return false;
		}
	}
}
