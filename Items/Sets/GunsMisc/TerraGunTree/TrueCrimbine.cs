using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Bullet;
using System;
using Terraria;
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
			item.damage = 41;
			item.ranged = true;
			item.width = 58;
			item.height = 32;
			item.useTime = 9;
			item.useAnimation = 9;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 0.2f;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item11;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<WitherBlast>();
			item.shootSpeed = 13f;
			item.useAmmo = AmmoID.Bullet;
			item.crit = 6;
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			charger++;
			if (charger >= 3) {
				for (int I = 0; I < 1; I++) {
					Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-230, 230) * 0.004f), speedY + ((float)Main.rand.Next(-230, 230) * 0.004f), ModContent.ProjectileType<GiantBlood>(), damage, knockBack, player.whoAmI, 0f, 0f);
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Crimbine>(), 1);
            recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
	}
}