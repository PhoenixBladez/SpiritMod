using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class AcidKnife : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Kunai");
            Tooltip.SetDefault("A venomous, low velocity Kunai that inflicts Acid Burn");

        }


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Shuriken);
            item.width = 26;
            item.height = 26;
            item.shoot = mod.ProjectileType("AcidKnife");
            item.useAnimation = 25;
            item.useTime = 25;
            item.shootSpeed = 10f;
            item.damage = 38;
            item.knockBack = 1.0f;
			item.value = Terraria.Item.sellPrice(0, 0, 0, 60);
            item.rare = 5;
            item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null,"Kunai_Throwing", 50);
            recipe.AddIngredient(null, "Acid", 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }
    }
}
