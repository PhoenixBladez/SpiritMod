using System;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class SpectreKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectre Knife");
			Tooltip.SetDefault("Upon hitting enemies or tiles, Spectre bolts are releaaed");
		}


		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 24;
			item.value = Terraria.Item.buyPrice(0, 30, 0, 0);
			item.rare = 8;
			item.maxStack = 999;
			item.crit = 6;
			item.damage = 65;
			item.knockBack = 3.5f;
			item.useStyle = 1;
			item.useTime = 15;
			item.useAnimation = 15;
			item.thrown = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.consumable = true;
			item.noUseGraphic = true;
			item.shoot = mod.ProjectileType("SpectreKnifeProj");
			item.shootSpeed = 11f;
			item.UseSound = SoundID.Item1;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpectreBar, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 33);
            recipe.AddRecipe();
        }
    }
}