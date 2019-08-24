using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Magic
{
    public class FrostTome : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Tome");
			Tooltip.SetDefault("Fires homing snowflakes at foes!");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 26;
            item.crit = 4;
            item.mana = 6;
            item.damage = 54;
            item.knockBack = 0;
            item.useStyle = 5;
            item.useTime = item.useAnimation = 17;
            item.magic = true;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 9, 0, 0);
            item.rare = 6;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("FrostFlake");
            item.shootSpeed = 10;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "IcyEssence", 14);
            recipe.AddTile(null, "EssenceDistorter");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}