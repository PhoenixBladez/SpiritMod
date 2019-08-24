using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class BismiteKnife : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Knife");
			Tooltip.SetDefault("Flies straight, ignoring gravity \n Occasionally poisons foes");
		}


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 24;
            item.height = 24;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("BismiteKnifeProjectile");
            item.useAnimation = 19;
            item.useTime = 19;
            item.consumable = true;
            item.maxStack = 999;
            item.shootSpeed = 10.0f;
            item.damage = 10;
            item.knockBack = 3.5f;
			item.value = Item.sellPrice(0, 0, 1, 0);
            item.crit = 16;
            item.rare = 2;
            item.autoReuse = false;
			item.maxStack = 999;
			item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BismiteCrystal", 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 30);
            recipe.AddRecipe();
        }
    }
}