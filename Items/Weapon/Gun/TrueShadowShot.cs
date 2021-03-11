using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Bullet;

namespace SpiritMod.Items.Weapon.Gun
{
	public class TrueShadowShot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightbane");
			Tooltip.SetDefault("Summons extra shadow bullets");
		}

		public override void SetDefaults()
		{
			item.damage = 43;
			item.ranged = true;
			item.width = 65;
			item.height = 28;
			item.useTime = 16;
			item.useAnimation = 16;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 8;
			item.useTurn = false;
			item.value = Item.sellPrice(gold: 5);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item36;
			item.autoReuse = true;
			item.shoot = ProjectileID.PurificationPowder;
			item.shootSpeed = 11f;
			item.useAmmo = AmmoID.Bullet;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if(type == ProjectileID.Bullet) type = ModContent.ProjectileType<NightBullet>();
			return true;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ShadowShot>(), 1);
            recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

		public class NightBullet : ModProjectile
		{
			public override string Texture => "SpiritMod/Projectiles/Bullet/BaneBullet";

			public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Night Bullet");
				ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
				ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			}

			public override void SetDefaults()
			{
				projectile.CloneDefaults(ProjectileID.Bullet);
				projectile.light = 0f;
                projectile.hide = true;
				aiType = ProjectileID.Bullet;
			}

			public override void AI()
			{
				float brightness = 0.5f;
				Lighting.AddLight(projectile.Center, new Vector3(0.6f, 0.2f, 1f) * brightness);
                for (int i = 0; i < 10; i++)
                {
                    float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                    float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
                    int num = Dust.NewDust(new Vector2(x, y), 2, 2, 173);
                    Main.dust[num].alpha = projectile.alpha;
                    Main.dust[num].velocity = Vector2.Zero;
                    Main.dust[num].noGravity = true;
                }
            }

			public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
			{
				var player = Main.player[projectile.owner];
				Vector2 offset = new Vector2(Main.rand.Next(-100, 100), Main.rand.Next(-100, 0));
				offset.Normalize();
				offset *= 66;
				Vector2 direction9 = target.Center - (player.Center + offset);
				direction9.Normalize();
				direction9 *= 10;
				Projectile.NewProjectile(player.Center + offset, direction9, ModContent.ProjectileType<BaneBullet>(), projectile.damage, 0, projectile.owner);
			}

			public override void Kill(int timeLeft)
			{
				Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
				Main.PlaySound(SoundID.Item10, projectile.position);
			}
		}
	}
}
