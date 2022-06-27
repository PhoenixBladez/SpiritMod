using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Bullet;

namespace SpiritMod.Items.Sets.GunsMisc.TerraGunTree
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
			Item.damage = 43;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 65;
			Item.height = 28;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 8;
			Item.useTurn = false;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item36;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 11f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.Bullet) 
				type = ModContent.ProjectileType<NightBullet>();
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<ShadowShot>(), 1);
            recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}

		public class NightBullet : ModProjectile
		{
			public override string Texture => "SpiritMod/Projectiles/Bullet/BaneBullet";

			public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Night Bullet");
				ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
				ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			}

			public override void SetDefaults()
			{
				Projectile.CloneDefaults(ProjectileID.Bullet);
				Projectile.light = 0f;
                Projectile.hide = true;
				AIType = ProjectileID.Bullet;
			}

			public override void AI()
			{
				float brightness = 0.5f;
				Lighting.AddLight(Projectile.Center, new Vector3(0.6f, 0.2f, 1f) * brightness);
                for (int i = 0; i < 10; i++)
                {
                    float x = Projectile.Center.X - Projectile.velocity.X / 10f * i;
                    float y = Projectile.Center.Y - Projectile.velocity.Y / 10f * i;
                    int num = Dust.NewDust(new Vector2(x, y), 2, 2, DustID.ShadowbeamStaff);
                    Main.dust[num].alpha = Projectile.alpha;
                    Main.dust[num].velocity = Vector2.Zero;
                    Main.dust[num].noGravity = true;
                }
            }

			public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
			{
				var player = Main.player[Projectile.owner];
				Vector2 offset = new Vector2(Main.rand.Next(-100, 100), Main.rand.Next(-100, 0));
				offset.Normalize();
				offset *= 66;
				Vector2 direction9 = target.Center - (player.Center + offset);
				direction9.Normalize();
				direction9 *= 10;
				Projectile.NewProjectile(player.Center + offset, direction9, ModContent.ProjectileType<BaneBullet>(), Projectile.damage, 0, Projectile.owner);
			}

			public override void Kill(int timeLeft)
			{
				Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			}
		}
	}
}
