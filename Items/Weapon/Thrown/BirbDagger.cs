using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class BirbDagger : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Asura's Blades");
		}


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 30;
            item.height = 50;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("BirbDagger");
            item.useAnimation = 13;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 13;
            item.shootSpeed = 12.5f;
            item.damage = 55;
            item.knockBack = 2f;
			item.value = Terraria.Item.sellPrice(0, 0, 4, 0);
            item.rare = 8;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "WorshipCrystal", 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 111);
            recipe.AddRecipe();
        }
    }
}
