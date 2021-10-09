using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechGun
{
	public class GranitechGunProjectile : ModProjectile
	{
		private int _charge = 0;
		private int _endCharge = -1;
		private float _finalRotation = 0f;
		private SpriteEffects _effect = SpriteEffects.None; //So there's no wackies with the semiauto mode

		const int MinimumCharge = 30; //How long it takes for a minimum charge - 1/2 second by default

		public bool LaserMode => projectile.ai[0] == 0; //player.altFunctionUse is a bit wacky so projectile.ai[0] is used to check if it's in firing mode or not

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
				projectile.Center = p.Center - (Vector2.Normalize(p.MountedCenter - Main.MouseWorld) * 27) + new Vector2(21, 12);
			}
			else
				projectile.Center = p.Center - (new Vector2(1, 0).RotatedBy(_finalRotation) * 27) + new Vector2(21, 12);

			bool letGo = p.altFunctionUse == 0 ? !p.channel && _endCharge == -1 : _endCharge < 1;
			if (letGo) //Fire (if possible)
			{
				_endCharge = _charge;
				_finalRotation = (Vector2.Normalize(p.MountedCenter - Main.MouseWorld) * 27).ToRotation();

				_endCharge += p.HeldItem.useAnimation;
				if (!LaserMode)
				{
					Fire(p);
				}
				else if (_endCharge >= MinimumCharge)
				{
					Fire(p, true);
				}

				_effect = Main.MouseWorld.X >= p.MountedCenter.X ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			}

			if (_charge > _endCharge && _endCharge != -1) //Kill projectile when done shooting
				projectile.Kill();
		}

		private void Fire(Player player, bool laser = false)
		{
			var baseVel = player.DirectionTo(Main.MouseWorld).RotatedByRandom(0.03f) * player.HeldItem.shootSpeed;

			Vector2 pos = player.Center - new Vector2(0, 8);
			Vector2 muzzleOffset = Vector2.Normalize(baseVel) * (player.HeldItem.width / 2f);
			if (Collision.CanHit(pos, 0, 0, pos + muzzleOffset, 0, 0))
				pos += muzzleOffset;

			int alt = player.altFunctionUse == 2 ? 0 : 2;
			var p = Projectile.NewProjectileDirect(pos, baseVel, ModContent.ProjectileType<GranitechGunBullet>(), player.HeldItem.damage, 0f, player.whoAmI, alt);
			if (laser)
				p.penetrate = -1;

			if (p.modProjectile is GranitechGunBullet bullet)
				bullet.spawnRings = true;

			VFX(pos + muzzleOffset, baseVel * 0.2f);
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

			float realRot = projectile.rotation; //Rotate towards mouse
			if (_endCharge != -1) realRot = _finalRotation + MathHelper.Pi;
			if (_effect == SpriteEffects.FlipHorizontally)
				realRot -= MathHelper.Pi;

			Vector2 drawPos = projectile.position - Main.screenPosition; //Draw position + charge shaking

			var frame = new Rectangle(0, 0, 86, 36);

			if (!LaserMode && _charge >= 5)
				frame = new Rectangle(0, 38, 86, 36);

			if (LaserMode)
			{
				_effect = Main.MouseWorld.X >= p.MountedCenter.X ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				frame = new Rectangle(0, 38, 86, 36);

				spriteBatch.Draw(Main.magicPixel, drawPos, new Rectangle(0, 0, 1, 1), Color.White, projectile.rotation, new Vector2(0, 1), new Vector2(10000, 2f), SpriteEffects.None, 0f);
			}

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
