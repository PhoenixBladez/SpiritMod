using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Thrown
{
	public class NebulaFlame : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Flame");
			Tooltip.SetDefault("Inflicts Nebula fire!");
		}


        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 22;
            item.value = Terraria.Item.sellPrice(0, 10, 0, 0);
            item.rare = 9;
            item.maxStack = 999;
            item.crit = 15;
            item.damage = 108;
            item.knockBack = 3.5F;
            item.useStyle = 1;
            item.useTime = 11;
            item.useAnimation = 11;
            item.thrown = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.consumable = true;
            item.noUseGraphic = true;
            item.shoot = mod.ProjectileType("NebulaFlameProjectile");
            item.shootSpeed = 11.5F;
            item.UseSound = SoundID.Item1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FragmentNebula, 2);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this, 111);
            recipe.AddRecipe();
        }
    }
}