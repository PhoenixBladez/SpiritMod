using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class DepthStar : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Depth Star");
			Tooltip.SetDefault("Occasionally explodes into seawater, damaging nearby enemies");
		}


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 22;
            item.height = 22;
			item.autoReuse = true;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("DepthStar");
            item.useAnimation = 25;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 25;
            item.shootSpeed = 13.0f;
            item.damage = 47;
            item.knockBack = 2f;
			item.value = Terraria.Item.sellPrice(0, 0, 0, 8);
            item.rare = 5;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DepthShard", 3);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 33);
            recipe.AddRecipe();
        }

    }
}
