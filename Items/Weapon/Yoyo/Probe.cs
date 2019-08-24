using System;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Items.Weapon.Yoyo
{
	public class Probe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Probe");
			Tooltip.SetDefault("Shoots out lasers in random arcs");
		}


		public override void SetDefaults()
		{
			base.item.CloneDefaults(3278);
			base.item.damage = 52;
            item.value = Terraria.Item.sellPrice(0, 10, 0, 0);
            base.item.rare = 6;
			base.item.knockBack = 3f;
			base.item.channel = true;
			base.item.useStyle = 5;
			base.item.useAnimation = 25;
			base.item.useTime = 24;
			base.item.shoot = base.mod.ProjectileType("ProbeP");
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HallowedBar, 11);
            recipe.AddIngredient(ItemID.SoulofMight, 13);
            recipe.AddIngredient(null, "PrintProbe", 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
