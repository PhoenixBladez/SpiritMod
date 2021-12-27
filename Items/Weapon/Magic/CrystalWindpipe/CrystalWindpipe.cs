using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Magic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.CrystalWindpipe
{
	public class CrystalWindpipe : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Crystal Windpipe");

		public override void SetDefaults()
		{
			item.damage = 50;
			item.magic = true;
			item.mana = 11;
			item.width = 40;
			item.height = 40;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 2;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<CrystalNote>();
			item.shootSpeed = 13f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Main.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/WindChime").WithPitchVariance(0.4f).WithVolume(0.8f), player.Center);

			for (int I = 0; I < 2; I++) {
				float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
				Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 20f;
				if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0)) {
					position += spawnPlace;
				}

				Vector2 velocity = Vector2.Normalize(Main.MouseWorld - position) * item.shootSpeed;
				Projectile projectile = Projectile.NewProjectileDirect(position, velocity * Main.rand.NextFloat(0.8f,1f), type, damage, knockBack, player.whoAmI);
				if (projectile.modProjectile is CrystalNote modProj)
				{
					modProj.initialAngle = new Vector2(speedX, speedY);
					projectile.scale = Main.rand.NextFloat(.8f, 1.1f);
				}
			}
			return false;
		}
	}

	public class CrystalNote : ModProjectile, IDrawAdditive
	{
		public Vector2 initialAngle;
		public Vector2 initialSpeed = Vector2.Zero;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Crystal Note");

		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 18;
			projectile.ranged = false;
			projectile.magic = true;
			projectile.friendly = true;
			projectile.penetrate = 2;
			projectile.timeLeft = 120;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			Lighting.AddLight(projectile.position, 0.245f, 0.103f, 0.255f);

			int index2 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustID.VenomStaff, 0.0f, 0.0f, 0, new Color(), 1f);
			Main.dust[index2].position = projectile.Center;
			Main.dust[index2].velocity = projectile.velocity;
			Main.dust[index2].noGravity = true;
			Main.dust[index2].scale = projectile.scale * 1.1f;

			if (projectile.timeLeft > 80)
			{
				if (initialSpeed == Vector2.Zero)
				{
					initialSpeed = projectile.velocity;
				}
				float veer = (float)Math.Sqrt(120 - projectile.timeLeft) * 0.09f;
				float rotDifference = ((((initialSpeed.ToRotation() - initialAngle.ToRotation()) % 6.28f) + 9.42f) % 6.28f) - 3.14f;
				projectile.velocity = projectile.velocity.RotatedBy(rotDifference * veer);
			}
			else if (projectile.timeLeft > 70 && projectile.timeLeft < 80)
            {
				projectile.velocity *= .91f;
            }
			else if (projectile.timeLeft < 60)
			{
				Vector2 vector2_1 = player.Center;
				Vector2 vector2_2 = Vector2.Normalize(vector2_1 - projectile.Center) * 9f;
				projectile.velocity = vector2_2;
				if (Vector2.Distance(projectile.Center, player.Center) <= 12.0)
				{
					projectile.Kill();
					return;
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
			for (int index1 = 4; index1 < 31; ++index1)
			{
				float num1 = projectile.oldVelocity.X * (20f / index1);
				float num2 = projectile.oldVelocity.Y * (20f / index1);
				int index2 = Dust.NewDust(new Vector2(projectile.oldPosition.X - num1, projectile.oldPosition.Y - num2), 8, 8, DustID.VenomStaff, projectile.oldVelocity.X, projectile.oldVelocity.Y, 175, new Color(), projectile.scale);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.5f;
				Main.dust[index2].fadeIn *= 0.5f;
			}
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			float scale = projectile.scale;
			Texture2D tex = Main.projectileTexture[projectile.type];
			Color color = new Color(139, 9, 214) * 0.36f;
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale * 1.5f, default, default);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale * 1.33f, default, default);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawPos = projectile.Center - Main.screenPosition;
			Color color = new Color(Color.Purple.R, Color.Purple.G, Color.Purple.B, 0);
			Texture2D tex = Main.projectileTexture[projectile.type];
			spriteBatch.Draw(tex, drawPos, null, color, 0, new Vector2(tex.Width, tex.Height) / 2, projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}
