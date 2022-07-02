using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Bullet;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.GunsMisc.TerraGunTree
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
		public override float DontConsumeAmmoChance => 1 / 3f;

		private Vector2 newVect;
		int charger;
		public override void SetDefaults()
		{
			Item.damage = 43;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 58;
			Item.height = 32;
			Item.useTime = 8;
			Item.useAnimation = 8;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 0.3f;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item92;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<TerraBullet>();
			Item.shootSpeed = 14f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (player.IsUsingAlt()) {
				player.itemTime = 29;
				player.itemAnimation = 29;
				Vector2 origVect = velocity;
				for (int X = 0; X <= 3; X++) {
					if (Main.rand.Next(2) == 1) {
						newVect = origVect.RotatedBy(Math.PI / (Main.rand.Next(82, 1800) / 10));
					}
					else {
						newVect = origVect.RotatedBy(-Math.PI / (Main.rand.Next(82, 1800) / 10));
					}
					if (type == ProjectileID.Bullet) type = ModContent.ProjectileType<TerraBullet1>();
					Projectile.NewProjectile(source, position.X, position.Y, newVect.X, newVect.Y, type, damage, 8f, player.whoAmI);
				}
				return false;
			}
			else {
				charger++;
				if (charger >= 7) {
					// Bombs do 33% more damage
					int bombDamage = damage + (int)(damage * (1 / 3f));
					Projectile.NewProjectile(source, position.X, position.Y, velocity.X + ((float)Main.rand.Next(-230, 230) / 100), velocity.Y + ((float)Main.rand.Next(-230, 230) / 100), ModContent.ProjectileType<TerraBomb>(), bombDamage, knockback, player.whoAmI, 0f, 0f);
					charger = 0;
				}
				return true;
			}
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (!player.IsUsingAlt())
			{
				if (type == ProjectileID.Bullet) type = ModContent.ProjectileType<TerraBullet>();
				float spread = MathHelper.PiOver4 / 10;
				float baseSpeed = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
				double baseAngle = Math.Atan2(velocity.X, velocity.Y);
				double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
				velocity.X = baseSpeed * (float)Math.Sin(randomAngle);
				velocity.Y = baseSpeed * (float)Math.Cos(randomAngle);
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<TrueShadowShot>(), 1);
			recipe.AddIngredient(ModContent.ItemType<TrueHolyBurst>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe1 = CreateRecipe(1);
			recipe1.AddIngredient(ModContent.ItemType<TrueCrimbine>(), 1);
			recipe1.AddIngredient(ModContent.ItemType<TrueHolyBurst>(), 1);
			recipe1.AddTile(TileID.MythrilAnvil);
			recipe1.Register();
		}
	}
}