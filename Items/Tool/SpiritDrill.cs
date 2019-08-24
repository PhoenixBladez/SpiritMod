using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
	public class SpiritDrill : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Drill");
		}


		public override void SetDefaults()
		{
            item.width = 54;
            item.height = 22;
            item.rare = 5;
            item.value = 40000;

            item.pick = 170;

            item.damage = 39;
            item.knockBack = 0;

            item.useStyle = 5;
            item.useTime = 7;
			item.useAnimation = 25;

            item.melee = true;
            item.channel = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.noUseGraphic = true;

            item.shoot = mod.ProjectileType("SpiritDrillProjectile");
			item.shootSpeed = 40f;

            item.UseSound = SoundID.Item23;
        }
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "SpiritBar", 18);
            modRecipe.AddTile(TileID.MythrilAnvil);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
    }
}
