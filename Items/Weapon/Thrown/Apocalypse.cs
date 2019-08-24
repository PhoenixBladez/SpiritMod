using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Thrown
{
    public class Apocalypse : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Apocalypse");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 42;
            item.rare = 8;
            item.maxStack = 999;
            item.crit = 11;
            item.damage = 72;
            item.knockBack = 5;
            item.useStyle = 1;
            item.useTime = item.useAnimation = 13;
            item.value = Terraria.Item.sellPrice(0, 0, 3, 0);
            item.thrown = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.consumable = true;
            item.noUseGraphic = true;
            item.shoot = mod.ProjectileType("ApocalypseProj");
            item.shootSpeed = 11;
            item.UseSound = SoundID.Item1;
        }
       public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null,"CursedFire", 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 33);
            recipe.AddRecipe();
        }
    }
}