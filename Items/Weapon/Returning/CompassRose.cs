using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Returning
{
	public class CompassRose : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Compass Rose");
            Tooltip.SetDefault("Occasionally explodes into seawater on enemy hits, damaging nearby foes"); 

        }


		public override void SetDefaults()
		{
            item.damage = 52;            
            item.melee = true;
            item.width = 40;
            item.height = 40;
			item.useTime = 32;
			item.useAnimation = 32;
            item.noUseGraphic = true;
            item.useStyle = 1;
			item.knockBack = 3;
            item.value = Terraria.Item.buyPrice(0, 1, 50, 0);
            item.rare = 5;
			item.shootSpeed = 13f;
			item.shoot = mod.ProjectileType ("CompassRose");
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DepthShard", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}