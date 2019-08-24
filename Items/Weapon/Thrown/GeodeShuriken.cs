using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class GeodeShuriken : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geode Shuriken");
			Tooltip.SetDefault("Critical hits inflict a multitude of debuffs");
		}


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Shuriken);
            item.width = 26;
            item.height = 26;           
            item.shoot = mod.ProjectileType("GeodeShurikenProjectile");
            item.useAnimation = 26;
            item.useTime = 26;
            item.shootSpeed = 13f;
            item.damage = 31;
            item.knockBack = 1.0f;
			item.value = Terraria.Item.sellPrice(0, 0, 1, 0);
            item.rare = 4;
            item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null,"Geode", 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }
}
