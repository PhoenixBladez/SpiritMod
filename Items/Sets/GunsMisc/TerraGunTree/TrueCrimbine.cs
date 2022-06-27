using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Bullet;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.GunsMisc.TerraGunTree
{
	public class TrueCrimbine : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Harvester");
			Tooltip.SetDefault("Rapidly shoots out wither blasts that inflict 'Wither' on hit foes\nOccasionally shoots out a giant clump of blood that steals a large amount of life");

		}


		int charger;
		public override void SetDefaults()
		{
			Item.damage = 41;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 58;
			Item.height = 32;
			Item.useTime = 9;
			Item.useAnimation = 9;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 0.2f;
			Item.useTurn = false;
			Item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item11;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<WitherBlast>();
			Item.shootSpeed = 13f;
			Item.useAmmo = AmmoID.Bullet;
			Item.crit = 6;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			charger++;
			if (charger >= 3) {
				for (int I = 0; I < 1; I++) {
					Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-230, 230) * 0.004f), speedY + ((float)Main.rand.Next(-230, 230) * 0.004f), ModContent.ProjectileType<GiantBlood>(), damage, knockback, player.whoAmI, 0f, 0f);
				}
				charger = 0;
			}
			if (type == ProjectileID.Bullet) {
				type = ModContent.ProjectileType<WitherBlast>();
			}
			float spread = 6 * 0.0174f;//45 degrees converted to radians
			float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
			double baseAngle = Math.Atan2(speedX, speedY);
			double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
			speedX = baseSpeed * (float)Math.Sin(randomAngle);
			speedY = baseSpeed * (float)Math.Cos(randomAngle);
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<Crimbine>(), 1);
            recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
	}
}