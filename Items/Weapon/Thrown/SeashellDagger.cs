using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
    public class SeashellDagger : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidal Dagger");
			Tooltip.SetDefault("Occasionally inflicts Tidal Ebb");
		}


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 18;
            item.height = 28;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("SeashellDaggerProj");
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 22;
            item.useAnimation = 22;
            item.shootSpeed = 12.5f;
            item.damage = 24;
            item.knockBack = 2;
            item.value = 100;
            item.rare = 3;
            item.maxStack = 999;
            item.autoReuse = true;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Coral, 2);
            recipe.AddIngredient(null, "PearlFragment", 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this,50);
            recipe.AddRecipe();
        }
    }
}
