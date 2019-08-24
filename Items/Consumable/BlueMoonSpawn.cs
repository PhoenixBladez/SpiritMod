using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class BlueMoonSpawn : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Turquoise Lens");
			Tooltip.SetDefault("Summons the Blue Moon");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.rare = 5;

            item.maxStack = 99;

            item.useStyle = 4;
            item.useTime = item.useAnimation = 20;

            item.noMelee = true;
            item.consumable = true;
            item.autoReuse = false;

            item.UseSound = SoundID.Item43;
        }

        public override bool CanUseItem(Player player)
        {
            if (Main.dayTime)
            {
                Main.NewText("The moon isn't powerful in daylight.", 80, 80, 150, true);
                return false;
            }

            return true;
        }

        public override bool UseItem(Player player)
        {
			Main.NewText("The Blue Moon is Rising...", 0, 90, 220, true);
            Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			MyWorld.BlueMoon = true; 
            return true;
        }
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Geode", 8);
			            recipe.AddIngredient(ItemID.SoulofLight, 10);
									            recipe.AddIngredient(null, "SteamParts", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
       
    }
}
