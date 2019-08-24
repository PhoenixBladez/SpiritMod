using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.Items.Weapon.Bow
{
	public class CygnusCrossbow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cygnus Crossbow");
			Tooltip.SetDefault("Arrows shot inflict Star Fracture");
		}



		public override void SetDefaults()
		{
			item.damage = 45;
			item.noMelee = true;
			item.ranged = true;
			item.width = 20;
			item.height = 38;
			item.useTime = 19;
			item.useAnimation = 19;
			item.useStyle = 5;
			item.shoot = 3;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 3;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 5;
			item.UseSound = SoundID.Item5;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.autoReuse = true;
			item.shootSpeed = 11f;
			item.crit = 4;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int I = 0; I < 2; I++)
			{
				int p = Projectile.NewProjectile(position.X, position.Y, speedX * (Main.rand.Next(400, 800) / 90), speedY * (Main.rand.Next(400, 800) / 90), mod.ProjectileType("TwilightArrow"), damage, knockBack, player.whoAmI);
				Main.projectile[p].GetGlobalProjectile<SpiritGlobalProjectile>(mod).shotFromStellarCrosbow = true;
			}

			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "StellarBar", 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}