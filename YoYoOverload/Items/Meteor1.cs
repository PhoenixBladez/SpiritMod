using System;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class Meteor1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tektike");
		}


		public override void SetDefaults()
		{
			base.item.CloneDefaults(3278);
			base.item.damage = 15;
			base.item.value = 29900;
			base.item.rare = 3;
			base.item.knockBack = 1.5f;
			base.item.channel = true;
			base.item.useStyle = 5;
			base.item.useAnimation = 25;
			base.item.useTime = 28;
			base.item.shoot = base.mod.ProjectileType("MeteorP");
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
			modRecipe.AddIngredient(117, 14);
			modRecipe.AddTile(16);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
