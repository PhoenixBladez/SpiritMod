using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.Items.Sets.GunsMisc.LadyLuck
{
	public class LadyLuck : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lady Luck");
			Tooltip.SetDefault("Right click to throw out a lucky coin\nShoot this coin to richochet the bullet towards nearby enemies\n'Luck favors the rich'");

		}
		public override void SetDefaults()
		{
			item.damage = 34;
			item.ranged = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 9;
			item.useAnimation = 9;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 0;
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item41;
			item.shoot = ModContent.ProjectileType<LadyLuckProj>();
			item.shootSpeed = 12f;
			item.useAmmo = AmmoID.Bullet;
			item.value = Item.sellPrice(0, 2, 20, 0);
			item.autoReuse = false;
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
            {
				type = item.shoot;
				Vector2 direction = new Vector2(speedX, speedY);
				/*direction = (3.14f + (float)Math.Sqrt(Math.Abs(direction.ToRotation() - 1.57f))).ToRotationVector2() * direction.Length();
				direction.X *= -1;*/
				int proj = Projectile.NewProjectile(position, direction, type, 0, knockBack, player.whoAmI);
				return false;
			}
			else
			{
				Vector2 direction = new Vector2(speedX, speedY);

				int p = Projectile.NewProjectile(position + (direction * 2.5f) + (direction.RotatedBy(-1.57f * player.direction) * -.12f), direction, ModContent.ProjectileType<LadyLuckFlash>(), 0, 0, player.whoAmI);
				Main.projectile[p].frame = Main.rand.Next(0, 3);
				Gore.NewGore(player.Center, new Vector2(player.direction * -1, -0.5f) * 4, mod.GetGoreSlot("Gores/BulletCasing"), 1f);
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
				Main.projectile[proj].GetGlobalProjectile<LLProj>().shotFromGun = true;
			}
			return false;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				item.shootSpeed = 9f;
				item.useTime = 16;
				item.useAnimation = 16;
				item.UseSound = SoundID.Item1;
				item.noUseGraphic = true;
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.useAmmo = 0;
			}
			else
			{
				item.shootSpeed = 16f;
				item.useTime = 9;
				item.useAnimation = 9;
				item.useStyle = ItemUseStyleID.HoldingOut;
				item.noUseGraphic = false;
				item.useAmmo = AmmoID.Bullet;
				item.UseSound = SoundID.Item41;
			}
			return base.CanUseItem(player);
		}
	}
	internal class LadyLuckFlash : ModProjectile
	{

		protected virtual Color color => Color.White;

		int direction = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("");
			Main.projFrames[projectile.type] = 3;
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.tileCollide = false;
			projectile.Size = new Vector2(20, 10);
			projectile.penetrate = -1;
			projectile.timeLeft = 3;
		}
		public override void AI()
		{
			if (projectile.velocity != Vector2.Zero)
			{
				direction = Math.Sign(projectile.velocity.X);
				projectile.rotation = projectile.velocity.ToRotation();
			}
			projectile.velocity = Vector2.Zero;
			CreateParticles();
		}

		protected virtual void CreateParticles()
		{
			Vector2 lineDirection = projectile.rotation.ToRotationVector2() * (projectile.width * 0.7f);
			Vector2 lineOffshoot = (projectile.rotation + 1.57f).ToRotationVector2() * projectile.height * 0.3f;
			for (int i = 0; i < 3; i++)
			{
				Vector2 position = projectile.Center + (lineDirection * Main.rand.NextFloat()) + (lineOffshoot * Main.rand.NextFloat(-1f, 1f));
				Dust.NewDustPerfect(position, 6, Main.rand.NextVector2Circular(1, 1) + ((projectile.rotation + Main.rand.NextFloat(-0.35f, 0.35f)).ToRotationVector2() * 5), 0, default, 1.3f).noGravity = true;
			}
		}
		public override Color? GetAlpha(Color lightColor) => Color.White * .8f;

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			int frameHeight = tex.Height / Main.projFrames[projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight);
			if (direction == 1)
			{
				spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, frame, color * .8f, projectile.rotation, new Vector2(0, frameHeight / 2), projectile.scale, SpriteEffects.None, 0f);
			}
			else
			{
				spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, frame, color * .8f, projectile.rotation + 3.14f, new Vector2(tex.Width, frameHeight / 2), projectile.scale, SpriteEffects.FlipHorizontally, 0f);
			}
			return false;
		}
	}
	public class LLProj : GlobalProjectile
	{
		public override bool InstancePerEntity => true;
		public bool shotFromGun = false;

		public bool hit = false;
		public NPC target;
		public float initialVel = 0f;

		public override void AI(Projectile projectile)
		{
			if (!hit)
				return;

			if (!target.active)
			{
				hit = false;
				return;
			}
			projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(target.Center) * initialVel, 0.1f);
		}
	}	
}