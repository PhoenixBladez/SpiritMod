using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
namespace SpiritMod.Items.Weapon.Magic
{
	public class FloranSporeWand : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Spore Wand");
			Tooltip.SetDefault("Shoots out a floating Floran Spore! \n Hit enemies are occasionally ensnared by vines and lose speed");
		}


		public override void SetDefaults()
		{
			item.width = 48;
			item.height = 50;			
			item.value = Item.buyPrice(0, 0, 20, 0);
			item.rare = 2;
			item.damage = 16;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.useTime = 25;
			item.useAnimation = 25;
			item.mana = 6;
            item.knockBack = 3;
            item.crit = 8;
			item.magic = true;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("FloranSpore");
			item.shootSpeed = 10f;
		}

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "FloranBar", 15);
            modRecipe.AddTile(TileID.Anvils);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
    }
}
