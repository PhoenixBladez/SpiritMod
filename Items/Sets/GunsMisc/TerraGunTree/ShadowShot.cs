using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Bullet;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GunsMisc.TerraGunTree
{
	public class ShadowShot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadeblaster");
			Tooltip.SetDefault("Converts regular bullets into vile bullets\nRight-click to shoot out a cursed tracker that sticks to enemies\nVile bullets home onto tracked enemies");
		}


		public override void SetDefaults()
		{
			Item.damage = 22;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 50;
			Item.height = 38;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 2;
			Item.useTurn = false;
			Item.value = Terraria.Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<VileBullet>();
			Item.shootSpeed = 10f;
			Item.useAmmo = AmmoID.Bullet;
			Item.crit = 6;
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2) {

				MyPlayer modPlayer = player.GetSpiritPlayer();
				if (modPlayer.shootDelay2 == 0)
					return true;
				return false;
			}
			else {
				return true;
			}
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y - 1)) * 45f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
			if (player.altFunctionUse == 2) {
				SoundEngine.PlaySound(SoundID.Item94);
				MyPlayer modPlayer = player.GetSpiritPlayer();
				modPlayer.shootDelay2 = 300;
				Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ShadowShotTracker>(), Item.damage / 3, knockback, player.whoAmI);
			}
			else {
				SoundEngine.PlaySound(SoundID.Item11);
				float spread = 30f * 0.0174f;//45 degrees converted to radians
				float baseSpeed = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
				double baseAngle = Math.Atan2(velocity.X, velocity.Y);
				{
					double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
					velocity.X = baseSpeed * (float)Math.Sin(randomAngle);
					velocity.Y = baseSpeed * (float)Math.Cos(randomAngle);
					if (type == ProjectileID.Bullet) {
						type = ModContent.ProjectileType<VileBullet>();
					}
						Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, Item.damage, knockback, Item.playerIndexTheItemIsReservedFor, 0, 0);
				}
			}
			return false;
		}
		public override void HoldItem(Player player)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (modPlayer.shootDelay2 == 1) {
				SoundEngine.PlaySound(SoundID.MaxMana, -1, -1, 1, 1f, 0.0f);
				for (int index1 = 0; index1 < 5; ++index1) {
					int index2 = Dust.NewDust(player.position, player.width, player.height, DustID.CursedTorch, 0.0f, 0.0f, (int)byte.MaxValue, new Color(), (float)Main.rand.Next(20, 26) * 0.1f);
					Main.dust[index2].noLight = false;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].velocity *= 0.5f;
				}
			}
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.Boomstick);
			recipe.AddIngredient(ItemID.Musket);
			recipe.AddIngredient(ItemID.Handgun, 1);
			recipe.AddIngredient(ModContent.ItemType<Items.Sets.CoilSet.CoilPistol>(), 1);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
	}
}