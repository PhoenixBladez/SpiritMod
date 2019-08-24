using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class CoilSpear : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coil Spear");
			Tooltip.SetDefault("Occasionally burns foes");
		}


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 8;
            item.height = 25;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("CoilSpearProj");
            item.useAnimation = 26;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 26;
            item.shootSpeed = 14f;
            item.damage = 23;
            item.knockBack = 2.7f;
			item.value = Item.sellPrice(0, 0, 0, 65);
            item.rare = 2;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "TechDrive", 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 75);
            recipe.AddRecipe();
        }
    }
}
