using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.FrostTroll
{
	public class Chillrend : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chillrend");
			Tooltip.SetDefault("Fires three consecutive rounds of bullets\nShoots out a homing chilly blast occasionally");
		}

		int charger;

		public override void SetDefaults()
		{
			Item.damage = 24;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 58;
			Item.height = 32;
			Item.useTime = 9;
			Item.useAnimation = 9;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 1;
			Item.value = 91950;
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item31;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
			Item.crit = 6;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			charger++;
			if (charger >= 4)
			{
				Projectile.NewProjectile(source, position.X - 8, position.Y + 8, velocity.X + ((float)Main.rand.Next(-230, 230) / 100), velocity.Y + ((float)Main.rand.Next(-230, 230) / 100), ModContent.ProjectileType<FrostBolt>(), 64, knockback, player.whoAmI, 0f, 0f);
				charger = 0;
			}
			return true;
		}
	}
}