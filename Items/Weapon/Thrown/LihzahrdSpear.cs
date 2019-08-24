using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class LihzahrdSpear : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lihzahrd Spear");
			Tooltip.SetDefault("Sticks to enemies and explodes\nLights enemies on fire");
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
            item.shoot = mod.ProjectileType("LihzahrdSpear");
            item.useAnimation = 13;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 13;
            item.shootSpeed = 11.0f;
            item.damage = 70;
            item.knockBack = 9f;
			item.value = Terraria.Item.sellPrice(0, 4, 0, 0);
            item.rare = 7;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SunShard", 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 99);
            recipe.AddRecipe();
        }
    }
}
