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
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Windpipe");
		}


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
			item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<CrystalNote>();
			item.shootSpeed = 10f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int I = 0; I < 2; I++) {
				float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
				Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 20f;
				if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0)) {
					position += spawnPlace;
				}

				Vector2 velocity = Vector2.Normalize(Main.MouseWorld - position) * item.shootSpeed;
				Projectile projectile = Projectile.NewProjectileDirect(position, velocity * Main.rand.NextFloat(0.8f,1f), type, damage, knockBack, player.whoAmI);
				if (projectile.modProjectile is CrystalNote modProj)
					modProj.initialAngle = new Vector2(speedX, speedY);
			}

			return false;
		}
	}
	public class CrystalNote : ModProjectile
	{
		public Vector2 initialAngle;
		public Vector2 initialSpeed = Vector2.Zero;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Note");
		}

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.damage = 0;
			projectile.height = 24;
			projectile.ranged = false;
			projectile.magic = true;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 90;
		}

		public override void AI()
		{
			if (initialSpeed == Vector2.Zero)
			{
				initialSpeed = projectile.velocity;
			}
			float veer = (float)Math.Sqrt(90 - projectile.timeLeft) * 0.1f;
			float rotDifference = ((((initialSpeed.ToRotation() - initialAngle.ToRotation()) % 6.28f) + 9.42f) % 6.28f) - 3.14f;
			projectile.velocity = projectile.velocity.RotatedBy(rotDifference * veer);
			if (projectile.timeLeft == 45)
				projectile.timeLeft = 2;
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
