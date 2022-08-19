using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.BossLoot.InfernonDrops
{
	public class InfernalStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seal of Torment");
			Tooltip.SetDefault("Shoots three exploding, homing, fiery souls\n3 second cooldown");
		}


		public override void SetDefaults()
		{
			Item.width = 42;
			Item.height = 42;
			Item.rare = ItemRarityID.Pink;
			Item.mana = 12;
			Item.damage = 55;
			Item.knockBack = 5F;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(0, 2, 50, 0);
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.DamageType = DamageClass.Magic;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<FireSoul>();
			Item.shootSpeed = 12f;
		}

		public static Vector2[] randomSpread(float speedX, float speedY, int angle, int num)
		{
			var posArray = new Vector2[num];
			float spread = (float)(angle * 0.0574532925);
			float baseSpeed = (float)System.Math.Sqrt(speedX * speedX + speedY * speedY);
			double baseAngle = System.Math.Atan2(speedX, speedY);
			double randomAngle;
			for (int i = 0; i < num; ++i) {
				randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
				posArray[i] = new Vector2(baseSpeed * (float)System.Math.Sin(randomAngle), baseSpeed * (float)System.Math.Cos(randomAngle));
			}
			return (Vector2[])posArray;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			modPlayer.shootDelay = 180;
			Vector2[] speeds = randomSpread(velocity.X, velocity.Y, 8, 3);
			for (int i = 0; i < 2; ++i) {
				Projectile.NewProjectile(source, position.X, position.Y, speeds[i].X, speeds[i].Y, type, damage, knockback, player.whoAmI);
			}
			return true;
		}
		public override bool CanUseItem(Player player)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (modPlayer.shootDelay == 0)
				return true;
			return false;
		}

	}
}