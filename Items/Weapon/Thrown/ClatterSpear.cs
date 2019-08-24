using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Thrown
{
    public class ClatterSpear : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clatter Spear");
			Tooltip.SetDefault("Attacks occasionally pierce through enemies, lowering their defense");
		}


        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 22;
            item.rare = 2;
            item.maxStack = 999;
            item.damage = 19;
            item.value = Terraria.Item.sellPrice(0, 0, 0, 3);
            item.knockBack = 6;
            item.useStyle = 1;
            item.useTime = item.useAnimation = 30;
            item.thrown = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.consumable = true;
            item.noUseGraphic = true;
            item.shoot = mod.ProjectileType("ClatterSpearProj");
            item.shootSpeed = 12;
            item.UseSound = SoundID.Item1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Carapace", 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }
    }
}