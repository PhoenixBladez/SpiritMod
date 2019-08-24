using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class BloodCharm : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Charm");
			Tooltip.SetDefault("'Grace the moon with blood'");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.rare = 5;

            item.maxStack = 99;

            item.useStyle = 4;
            item.useTime = item.useAnimation = 20;

            item.noMelee = true;
            item.consumable = false;
            item.autoReuse = false;

            item.UseSound = SoundID.Item43;
        }

        public override bool CanUseItem(Player player)
        {
            if (Main.dayTime)
            {
                Main.NewText("The moon cannot dampen with blood in daylight.", 200, 80, 130, true);
                return false;
            }

            return true;
        }

        public override bool UseItem(Player player)
        {
			Main.NewText("The Blood Moon is Rising", 200, 80, 130, true);
            Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			Main.bloodMoon = true; 
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Deathweed, 5);
			recipe.AddIngredient(ItemID.Chain);
			recipe.AddIngredient(null, "Veinstone", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
