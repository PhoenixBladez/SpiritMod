using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class GeodePickaxe : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geode Pickaxe");
		}


        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 36;
            item.value = 30000;
            item.rare = 4;

            item.pick = 150;

            item.damage = 14;
            item.knockBack = 4;

            item.useStyle = 1;
            item.useTime = 8;
            item.useAnimation = 23;

            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;

            item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()  
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Geode", 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(12) == 0)
                target.AddBuff(BuffID.CursedInferno, 200, true);
            if (Main.rand.Next(12) == 0)
                target.AddBuff(BuffID.Frostburn, 200, true);
            if (Main.rand.Next(12) == 0)
                target.AddBuff(BuffID.OnFire, 200, true);
        }
    }
}
