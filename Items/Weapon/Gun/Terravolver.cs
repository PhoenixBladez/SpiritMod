using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Bullet;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Gun
{
	public class Terravolver : SpiritItem
	{
		public override string SetDisplayName => "Terravolver";
		public override string SetTooltip => 
			"Shoots elemental bullets and bombs that inflict powerful burns\n" +
			"Right click while holding for a shotgun blast\n" +
			"33% chance to not consume ammo\n" + 
			"'Nature goes out with a bang'";
		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
		public override float DontConsumeAmmoChance => 1/3f;

		private Vector2 newVect;
		int charger;
		public override void SetDefaults()
		{
			item.damage = 43;
			item.ranged = true;
			item.width = 58;
			item.height = 32;
			item.useTime = 8;
			item.useAnimation = 8;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 0.3f;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item92;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<TerraBullet>();
			item.shootSpeed = 14f;
			item.useAmmo = AmmoID.Bullet;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if(player.IsUsingAlt()) {
				player.itemTime = 29;
				player.itemAnimation = 29;
				Vector2 origVect = new Vector2(speedX, speedY);
				for(int X = 0; X <= 3; X++) {
					if(Main.rand.Next(2) == 1) {
						newVect = origVect.RotatedBy(Math.PI / (Main.rand.Next(82, 1800) / 10));
					} else {
						newVect = origVect.RotatedBy(-Math.PI / (Main.rand.Next(82, 1800) / 10));
					}
					if(type == ProjectileID.Bullet) type = ModContent.ProjectileType<TerraBullet1>();
					Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, type, damage, 8f, player.whoAmI);
				}
				return false;
			} else {
				charger++;
				if(charger >= 7) {
					// Bombs do 33% more damage
					int bombDamage = damage + (int)(damage * (1 / 3f));
					Projectile.NewProjectile(position.X, position.Y, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), ModContent.ProjectileType<TerraBomb>(), bombDamage, knockBack, player.whoAmI, 0f, 0f);
					charger = 0;
				}

				if(type == ProjectileID.Bullet) type = ModContent.ProjectileType<TerraBullet>();
				float spread = MathHelper.PiOver4 / 10;
				float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
				double baseAngle = Math.Atan2(speedX, speedY);
				double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
				speedX = baseSpeed * (float)Math.Sin(randomAngle);
				speedY = baseSpeed * (float)Math.Cos(randomAngle);
				return true;
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TrueShadowShot>(), 1);
			recipe.AddIngredient(ModContent.ItemType<TrueHolyBurst>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(ModContent.ItemType<TrueCrimbine>(), 1);
			recipe1.AddIngredient(ModContent.ItemType<TrueHolyBurst>(), 1);
			recipe1.AddTile(TileID.MythrilAnvil);
			recipe1.SetResult(this, 1);
			recipe1.AddRecipe();
		}
	}
}