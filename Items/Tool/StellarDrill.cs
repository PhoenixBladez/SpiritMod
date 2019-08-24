using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
	public class StellarDrill : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Drill");
		}


		public override void SetDefaults()
		{
            item.width = 48;
            item.height = 22;
            item.rare = 5;

            item.pick = 180;

            item.damage = 22;
            item.knockBack = 0;

            item.useStyle = 5;
            item.useTime = 13;
			item.useAnimation = 13;

            item.melee = true;
            item.channel = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.noUseGraphic = true;

			item.shoot = mod.ProjectileType("StellarDrillProjectile");
			item.shootSpeed = 40f;

            item.UseSound = SoundID.Item23;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "StellarBar", 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
