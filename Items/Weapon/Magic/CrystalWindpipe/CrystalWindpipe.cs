using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.CrystalWindpipe
{
	public class CrystalWindpipe : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Crystal Windchimes");

		public override void SetDefaults()
		{
			Item.damage = 35;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 11;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 2;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<CrystalNote>();
			Item.shootSpeed = 13f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/WindChime") with { PitchVariance = 0.4f, Volume = 0.8f}, player.Center);

			for (int I = 0; I < 2; I++) {
				float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi) * 0.05f;
				Vector2 spawnPlace = Vector2.UnitX.RotatedBy(angle) * 10f;
				if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0)) 
					position += spawnPlace;

				Vector2 vel = velocity;
				Projectile projectile = Projectile.NewProjectileDirect(source, position, vel.RotatedBy(angle) * Main.rand.NextFloat(0.8f,1f), type, damage, knockback, player.whoAmI);
				if (projectile.ModProjectile is CrystalNote modProj)
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
					Projectile.netUpdate = true;

				_dying = value;
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Note");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 18;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 120;
			Projectile.alpha = 255;
			Projectile.hide = true;
			Projectile.scale = Main.rand.NextFloat(0.7f, 1.3f);
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Lighting.AddLight(Projectile.position, 0.245f, 0.103f, 0.255f);

			//Make dust when not dying
			if (Main.rand.NextBool() && !_dying)
			{
				int index2 = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.VenomStaff, 0.0f, 0.0f, Projectile.alpha, new Color(), 1f);
				Main.dust[index2].position = Projectile.Center;
				Main.dust[index2].velocity = Projectile.velocity * Projectile.Opacity;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].scale = Projectile.scale * 1.1f;
			}

			float velocityDecayRate = 0.975f;

			//Before starting to return, slow down in velocity over time and rotate its velocity
			if (Projectile.timeLeft > RETURN_STARTTIME)
			{
				if (initialSpeed == Vector2.Zero)
					initialSpeed = Projectile.velocity;

				int fadeInTime = 15;
				Projectile.alpha = Math.Max(Projectile.alpha - (255 / fadeInTime), 0);

				float veer = (float)Math.Sqrt(120 - Projectile.timeLeft) * 0.13f;
				float rotDifference = ((((initialSpeed.ToRotation() - initialAngle.ToRotation()) % 6.28f) + 9.42f) % 6.28f) - 3.14f;
				Projectile.velocity = Projectile.velocity.RotatedBy(rotDifference * veer) * velocityDecayRate;
			}

			//Interpolate its velocity back to player, fading out and dying if close to running out of timeleft or getting too close to player
			else
			{
				float lerpRate = MathHelper.Clamp(1 - (Projectile.timeLeft / RETURN_STARTTIME), 0.05f, 0.25f);
				float speed = MathHelper.Clamp(1 - (Projectile.timeLeft / RETURN_STARTTIME), 0.25f, 1f) * 20;

				Vector2 homeCenter = player.Center;
				Vector2 direction = Vector2.Normalize(homeCenter - Projectile.Center) * speed;
				if(!Dying)
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction, lerpRate);

				int fadeOutTime = 25;
				if (Vector2.Distance(Projectile.Center, player.Center) <= 40f || Projectile.timeLeft < fadeOutTime || Dying)
				{
					Dying = true;
					Projectile.velocity *= velocityDecayRate;
					Projectile.alpha = Math.Min(Projectile.alpha + (255 / fadeOutTime), 255);
					if (Projectile.alpha >= 255)
						Projectile.Kill();
				}
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (int index1 = 4; index1 < 31; ++index1)
			{
				float num1 = Projectile.oldVelocity.X * (20f / index1);
				float num2 = Projectile.oldVelocity.Y * (20f / index1);
				int index2 = Dust.NewDust(new Vector2(Projectile.oldPosition.X - num1, Projectile.oldPosition.Y - num2), 8, 8, DustID.VenomStaff, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 175, new Color(), Projectile.scale);
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

		public void AdditiveCall(SpriteBatch spriteBatch, Vector2 screenPos)
		{
			float scale = Projectile.scale * Projectile.Opacity;
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(255, 97, 244) * 0.75f * Projectile.Opacity;
			int trailLength = ProjectileID.Sets.TrailCacheLength[Projectile.type];

			//Spritebatch trail that lowers in scale with progress
			for (int i = 0; i < trailLength; i++)
			{
				float progress = 1 - (i / (float)trailLength);
				Vector2 oldPosition = Projectile.oldPos[i] + Projectile.Size / 2;
				float opacityMod = 0.75f;

				spriteBatch.Draw(tex, oldPosition - screenPos, null, color * opacityMod * progress, Projectile.rotation,
					tex.Size() / 2, scale * 1.33f * progress, default, default);
			}

			spriteBatch.Draw(tex, Projectile.Center - screenPos, null, color, Projectile.rotation, tex.Size() / 2, scale * 1.5f, default, default);
			spriteBatch.Draw(tex, Projectile.Center - screenPos, null, color, Projectile.rotation, tex.Size() / 2, scale * 1.33f, default, default);
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
