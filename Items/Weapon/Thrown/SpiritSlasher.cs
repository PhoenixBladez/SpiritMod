using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Thrown
{
    public class SpiritSlasher : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Edge");
			Tooltip.SetDefault("Explodes on contact with fos, dealing Soul Burn");
		}


        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 20;
            item.value = Item.sellPrice(0, 0, 3, 0);
            item.rare = 6;
            item.maxStack = 999;
            item.crit = 4;
            item.damage = 39;
            item.knockBack = 3;
            item.useStyle = 1;
            item.useTime = item.useAnimation = 22;
            item.thrown = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.consumable = true;
            item.noUseGraphic = true;
            item.shoot = mod.ProjectileType("SpiritSlasher");
            item.shootSpeed = 11;
            item.UseSound = SoundID.Item1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SpiritBar", 3);
            recipe.AddIngredient(null, "SoulShred", 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }
}