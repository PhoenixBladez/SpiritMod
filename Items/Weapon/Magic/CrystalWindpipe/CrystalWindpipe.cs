using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.CrystalWindpipe
{
	public class CrystalWindpipe : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Crystal Windchimes");

		public override void SetDefaults()
		{
			item.damage = 35;
			item.magic = true;
			item.mana = 11;
			item.width = 40;
			item.height = 40;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 2;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<CrystalNote>();
			item.shootSpeed = 13f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Main.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/WindChime").WithPitchVariance(0.4f).WithVolume(0.8f), player.Center);

			for (int I = 0; I < 2; I++) {
				float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi) * 0.05f;
				Vector2 spawnPlace = Vector2.UnitX.RotatedBy(angle) * 10f;
				if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0)) 
					position += spawnPlace;

				Vector2 vel = new Vector2(speedX, speedY);
				Projectile projectile = Projectile.NewProjectileDirect(position, vel.RotatedBy(angle) * Main.rand.NextFloat(0.8f,1f), type, damage, knockBack, player.whoAmI);
				if (projectile.modProjectile is CrystalNote modProj)
					modProj.initialAngle = vel;
			}
			return false;
		}
	}

	public class CrystalNote : ModProjectile, IDrawAdditive
	{
		private const float RETURN_STARTTIME = 70;

		public Vector2 initialAngle;
		public Vector2 initialSpeed = Vector2.Zero;
		private bool _dying = false;

		private bool Dying
		{
			get => _dying;
			set
			{
				//Automatically sync when property changes
				if (_dying != value)
					projectile.netUpdate = true;

				_dying = value;
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Note");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 15;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 18;
			projectile.ranged = false;
			projectile.magic = true;
			projectile.friendly = true;
			projectile.penetrate = 2;
			projectile.timeLeft = 120;
			projectile.alpha = 255;
			projectile.hide = true;
			projectile.scale = Main.rand.NextFloat(0.7f, 1.3f);
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			Lighting.AddLight(projectile.position, 0.245f, 0.103f, 0.255f);

			//Make dust when not dying
			if (Main.rand.NextBool() && !_dying)
			{
				int index2 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustID.VenomStaff, 0.0f, 0.0f, projectile.alpha, new Color(), 1f);
				Main.dust[index2].position = projectile.Center;
				Main.dust[index2].velocity = projectile.velocity * projectile.Opacity;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].scale = projectile.scale * 1.1f;
			}

			float velocityDecayRate = 0.975f;

			//Before starting to return, slow down in velocity over time and rotate its velocity
			if (projectile.timeLeft > RETURN_STARTTIME)
			{
				if (initialSpeed == Vector2.Zero)
					initialSpeed = projectile.velocity;

				int fadeInTime = 15;
				projectile.alpha = Math.Max(projectile.alpha - (255 / fadeInTime), 0);

				float veer = (float)Math.Sqrt(120 - projectile.timeLeft) * 0.13f;
				float rotDifference = ((((initialSpeed.ToRotation() - initialAngle.ToRotation()) % 6.28f) + 9.42f) % 6.28f) - 3.14f;
				projectile.velocity = projectile.velocity.RotatedBy(rotDifference * veer) * velocityDecayRate;
			}

			//Interpolate its velocity back to player, fading out and dying if close to running out of timeleft or getting too close to player
			else
			{
				float lerpRate = MathHelper.Clamp(1 - (projectile.timeLeft / RETURN_STARTTIME), 0.05f, 0.25f);
				float speed = MathHelper.Clamp(1 - (projectile.timeLeft / RETURN_STARTTIME), 0.25f, 1f) * 20;

				Vector2 homeCenter = player.Center;
				Vector2 direction = Vector2.Normalize(homeCenter - projectile.Center) * speed;
				if(!Dying)
					projectile.velocity = Vector2.Lerp(projectile.velocity, direction, lerpRate);

				int fadeOutTime = 25;
				if (Vector2.Distance(projectile.Center, player.Center) <= 40f || projectile.timeLeft < fadeOutTime || Dying)
				{
					Dying = true;
					projectile.velocity *= velocityDecayRate;
					projectile.alpha = Math.Min(projectile.alpha + (255 / fadeOutTime), 255);
					if (projectile.alpha >= 255)
						projectile.Kill();
				}
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (int index1 = 4; index1 < 31; ++index1)
			{
				float num1 = projectile.oldVelocity.X * (20f / index1);
				float num2 = projectile.oldVelocity.Y * (20f / index1);
				int index2 = Dust.NewDust(new Vector2(projectile.oldPosition.X - num1, projectile.oldPosition.Y - num2), 8, 8, DustID.VenomStaff, projectile.oldVelocity.X, projectile.oldVelocity.Y, 175, new Color(), projectile.scale);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.5f;
			}
		}

		public override void Kill(int timeLeft)
		{
			/*for (int index1 = 4; index1 < 31; ++index1)
			{
				float num1 = projectile.oldVelocity.X * (20f / index1);
				float num2 = projectile.oldVelocity.Y * (20f / index1);
				int index2 = Dust.NewDust(new Vector2(projectile.oldPosition.X - num1, projectile.oldPosition.Y - num2), 8, 8, DustID.VenomStaff, projectile.oldVelocity.X, projectile.oldVelocity.Y, 175, new Color(), projectile.scale);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.5f;
				Main.dust[index2].fadeIn *= 0.5f;
			}*/
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			float scale = projectile.scale * projectile.Opacity;
			Texture2D tex = Main.projectileTexture[projectile.type];
			Color color = new Color(255, 97, 244) * 0.75f * projectile.Opacity;
			int trailLength = ProjectileID.Sets.TrailCacheLength[projectile.type];

			//Spritebatch trail that lowers in scale with progress
			for (int i = 0; i < trailLength; i++)
			{
				float progress = 1 - (i / (float)trailLength);
				Vector2 oldPosition = projectile.oldPos[i] + projectile.Size / 2;
				float opacityMod = 0.75f;

				spriteBatch.Draw(tex, oldPosition - Main.screenPosition, null, color * opacityMod * progress, projectile.rotation,
					tex.Size() / 2, scale * 1.33f * progress, default, default);
			}

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale * 1.5f, default, default);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale * 1.33f, default, default);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Dying);
			writer.WriteVector2(initialAngle);
			writer.WriteVector2(initialSpeed);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Dying = reader.ReadBoolean();
			initialAngle = reader.ReadVector2();
			initialSpeed = reader.ReadVector2();
		}
	}
}
