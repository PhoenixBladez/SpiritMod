using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class Corrupt : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Vile Spitball");
			Tooltip.SetDefault("Gross...");
		}


		public override void SetDefaults()
		{
			base.item.CloneDefaults(3278);
			base.item.damage = 17;
			base.item.value = 60000;
			base.item.rare = 3;
			base.item.knockBack = 2f;
			base.item.channel = true;
			base.item.useStyle = 5;
			base.item.useAnimation = 25;
			base.item.useTime = 25;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (Main.rand.Next(3) == 0)
			{
				base.item.shoot = base.mod.ProjectileType("CorrT");
				return true;
			}
			base.item.shoot = base.mod.ProjectileType("CorrP");
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
			modRecipe.AddIngredient(68, 14);
			modRecipe.AddIngredient(86, 8);
			modRecipe.AddTile(16);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
