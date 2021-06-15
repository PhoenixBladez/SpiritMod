using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using System;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Held;
using SpiritMod.Projectiles.Returning;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown.SlingHammers
{
	public class ShadowAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Possessed Hammer");
			Tooltip.SetDefault("Hold down and release to throw the hammer like a boomerang");
		}


		private Vector2 newVect;
		public override void SetDefaults()
		{
			item.useStyle = 100;
			item.width = 40;
			item.height = 32;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.melee = true;
			item.channel = true;
			item.noMelee = true;
			item.useAnimation = 30;
			item.useTime = 30;
			item.shootSpeed = 8f;
			item.knockBack = 5f;
			item.damage = 53;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.LightRed;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ModContent.ProjectileType<ShadowAxeProj>();
		}
		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[item.shoot] == 0 && player.ownedProjectileCounts[ModContent.ProjectileType<ShadowAxeProjReturning>()] == 0;
	}
	public class ShadowAxeProj : SlingHammerProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Possessed Hammer");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}
		protected override int height => 66;
		protected override int width => 58;
		protected override int chargeTime => 43;
		protected override float chargeRate => 0.7f;
		protected override int thrownProj => ModContent.ProjectileType<ShadowAxeProjReturning>();
		protected override float damageMult => 1.25f;
		protected override int throwSpeed => 18;
	}
	public class ShadowAxeProjReturning : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Possessed Hammer");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = 44;
			projectile.height = 44;
			projectile.aiStyle = 3;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 700;
			projectile.extraUpdates = 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			if (projectile.tileCollide)
			{
				Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<ShadowAxeExplosion>(), (int)(projectile.damage * 1.5f), projectile.knockBack * 1.5f, projectile.owner);
				player.GetModPlayer<MyPlayer>().Shake += 8;
				Main.PlaySound(SoundID.Item88, projectile.Center);
			}
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (projectile.tileCollide)
				damage = (int)(damage * 1.25);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<ShadowAxeExplosion>(), (int)(projectile.damage * 1.5f), projectile.knockBack * 1.5f, projectile.owner);
			Player player = Main.player[projectile.owner];
			player.GetModPlayer<MyPlayer>().Shake += 8;
			Main.PlaySound(SoundID.Item88, projectile.Center);
			return base.OnTileCollide(oldVelocity);
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = height /= 2;
			return true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i++)
			{
				float opacity = (ProjectileID.Sets.TrailCacheLength[projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[projectile.type];
				opacity *= 0.5f;
				spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.oldPos[i] + projectile.Size / 2 - Main.screenPosition, null, projectile.GetAlpha(lightColor) * opacity, projectile.oldRot[i],
					Main.projectileTexture[projectile.type].Size() / 2, projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}
	}
	public class ShadowAxeExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosion");
			Main.projFrames[base.projectile.type] = 9;
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 144;
			projectile.height = 144;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.penetrate = -1;
		}

		public override void AI()
		{
			Lighting.AddLight(projectile.Center, Color.Purple.ToVector3());
			projectile.frameCounter++;
			if (projectile.frameCounter >= 3)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
				if (projectile.frame >= 9)
					projectile.active = false;
			}
		}
		public override bool? CanHitNPC(NPC target)
		{
			if (projectile.frame < 3 || projectile.frame > 5)
				return false;
			return base.CanHitNPC(target);
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}