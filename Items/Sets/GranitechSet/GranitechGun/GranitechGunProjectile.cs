using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechGun
{
	public class GranitechGunProjectile : ModProjectile
	{
		private int _charge = 0;
		private int _endCharge = -1;
		private float _finalRotation = 0f;
		private SpriteEffects _effect = SpriteEffects.None; //So there's no wackies with the semiauto mode

		const int ChargeUp = 16; //How long it takes to start up

		public override void SetStaticDefaults() => DisplayName.SetDefault("Granitech Blaster");

		public override void SetDefaults()
		{
			projectile.width = 56;
			projectile.height = 36;
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
			p.heldProj = projectile.whoAmI;

			if (p.whoAmI != Main.myPlayer) return; //mp check (hopefully)

			if (p.channel) //Use turn functionality
				p.direction = Main.MouseWorld.X >= p.MountedCenter.X ? 1 : -1;

			projectile.rotation = Vector2.Normalize(p.MountedCenter - Main.MouseWorld).ToRotation() - MathHelper.Pi; //So it looks like the player is holding it properly

			_charge++; //Increase charge timer...
			projectile.timeLeft++; //...and dont die

			if (_endCharge == -1) //Wait until the player has fired to let go & set position
			{
				p.itemTime = p.HeldItem.useTime;
				p.itemAnimation = p.HeldItem.useAnimation;
				projectile.Center = p.Center - (Vector2.Normalize(p.MountedCenter - Main.MouseWorld) * 27) + new Vector2(21, 12 + p.gfxOffY);
			}
			else
				projectile.Center = p.Center - (new Vector2(1, 0).RotatedBy(_finalRotation) * 27) + new Vector2(21, 12 + p.gfxOffY);

			if (!p.channel && _endCharge == -1) //the player has stopped shooting
			{
				_endCharge = _charge;
				_finalRotation = (Vector2.Normalize(p.MountedCenter - Main.MouseWorld) * 27).ToRotation();
				_endCharge += p.HeldItem.useAnimation;
				_effect = Main.MouseWorld.X >= p.MountedCenter.X ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			}

			if (_charge > _endCharge && _endCharge != -1) //Kill projectile when done shooting
				projectile.Kill();

			if (_charge > ChargeUp && (_charge - ChargeUp) % p.HeldItem.useTime == 1)
				Fire(p);
		}

		private void Fire(Player player)
		{
			var baseVel = player.DirectionTo(Main.MouseWorld).RotatedByRandom(0.02f) * player.HeldItem.shootSpeed;

			Vector2 pos = player.Center - new Vector2(0, 8);
			Vector2 muzzleOffset = Vector2.Normalize(baseVel) * (player.HeldItem.width / 2f);
			if (Collision.CanHit(pos, 0, 0, pos + muzzleOffset, 0, 0))
				pos += muzzleOffset;

			var p = Projectile.NewProjectileDirect(pos, baseVel, ModContent.ProjectileType<GranitechGunBullet>(), player.HeldItem.damage, 0f, player.whoAmI);
			if (p.modProjectile is GranitechGunBullet bullet)
				bullet.spawnRings = true;

			//Main.PlaySound(Terraria.ID.SoundID.Item11, projectile.Center);
			Main.PlaySound(SpiritMod.instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/EnergyShoot").WithPitchVariance(0.1f).WithVolume(0.25f), pos);
			VFX(pos + muzzleOffset, baseVel * 0.2f);

			GItem.UseAmmo(player, AmmoID.Bullet);
		}

		private void VFX(Vector2 position, Vector2 velocity)
		{
			for (int i = 0; i < 6; ++i)
			{
				Vector2 vel = velocity.RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(2f, 6f);
				ParticleHandler.SpawnParticle(new GranitechGunParticle(position, vel, Main.rand.NextFloat(2.5f, 3f), 20, Main.rand.Next(2, 5)));
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player p = Main.player[projectile.owner];
			Texture2D t = Main.projectileTexture[projectile.type];

			_effect = Main.MouseWorld.X >= p.MountedCenter.X ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			float realRot = projectile.rotation; //Rotate towards mouse
			if (_endCharge != -1) realRot = _finalRotation + MathHelper.Pi;
			if (_effect == SpriteEffects.FlipHorizontally)
				realRot -= MathHelper.Pi;

			Vector2 drawPos = projectile.position - Main.screenPosition; //Draw position + charge shaking

			const int Width = 82;
			const int Height = 38;

			const int MuzzleFlashDuration = 2;

			var frame = new Rectangle(0, 0, Width, Height);

			if (_charge > ChargeUp && (_charge - ChargeUp) % p.HeldItem.useTime < MuzzleFlashDuration)
			{
				int offset = (_charge - ChargeUp) % (p.HeldItem.useTime * 3);
				int variation = offset / p.HeldItem.useTime;

				frame = new Rectangle(0, Height * (4 + (variation * 2)), Width, Height);
			}
			else if (_charge > ChargeUp && (_charge - ChargeUp) % p.HeldItem.useTime >= MuzzleFlashDuration)
			{
				int offset = (_charge - ChargeUp) % (p.HeldItem.useTime * 3);
				int variation = offset / p.HeldItem.useTime;

				frame = new Rectangle(0, Height * (5 + (variation * 2)), Width, Height);
			}

			if (_charge < ChargeUp / 4)
				frame = new Rectangle(0, 0, Width, Height);
			else if (_charge < ChargeUp / 2)
				frame = new Rectangle(0, Height, Width, Height);
			else if (_charge < ChargeUp / 1.5f)
				frame = new Rectangle(0, Height * 2, Width, Height);
			else if (_charge <= ChargeUp)
				frame = new Rectangle(0, Height * 3, Width, Height);

			spriteBatch.Draw(t, drawPos, frame, lightColor, realRot, new Vector2(42, 22), 1f, _effect, 1f);

			Texture2D glowmask = ModContent.GetTexture(Texture + "_glow");
			spriteBatch.Draw(glowmask, drawPos, frame, Color.White, realRot, new Vector2(42, 22), 1f, _effect, 1f);

			return false;
		}

		public override void Kill(int timeLeft) //paranoia - I don't know if this is necessary
		{
			_endCharge = -1;
			_finalRotation = 0;
		}
	}
}
