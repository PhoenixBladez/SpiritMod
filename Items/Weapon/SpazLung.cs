using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.Items.Weapon
{
	public class SpazLung : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spaz Lung");
			Tooltip.SetDefault("Turns Gel into Cursed Fire\nHas a 25% chance not to consume ammo");
		}



		public override void SetDefaults()
		{
			item.damage = 48;
			item.noMelee = true;
			item.ranged = true;
			item.width = 58;
			item.height = 20;
			item.useTime = 14;
			item.useAnimation = 14;
			item.useStyle = 5;
			item.shoot = 3;
			item.useAmmo = 23;
			item.knockBack = 3;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = 5;
			item.UseSound = SoundID.Item34;
			item.autoReuse = true;
			item.shootSpeed = 7f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int projectileFired = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ProjectileID.EyeFire, item.damage, item.knockBack, player.whoAmI);
			Main.projectile[projectileFired].friendly = true;
			Main.projectile[projectileFired].friendly = true;
			Main.projectile[projectileFired].GetGlobalProjectile<SpiritGlobalProjectile>(mod).shotFromSpazLung = true;
			Main.projectile[projectileFired].hostile = false;
			Main.projectile[projectileFired].netUpdate = true;
			return false;
		}

		public override bool ConsumeAmmo(Player player)
		{
			if (Main.rand.Next(4) == 0)
				return false;

			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BlueprintTwins", 1);
			recipe.AddIngredient(ItemID.HallowedBar, 6);
			recipe.AddIngredient(ItemID.SoulofSight, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}
}
