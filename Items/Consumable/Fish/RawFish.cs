using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Fish
{
    public class RawFish : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Raw Fish");
			Tooltip.SetDefault("'Can be eaten... Maybe cook it first?'");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 22;
            item.rare = 1;
            item.maxStack = 99;
            item.noUseGraphic = true;
			item.useStyle = 2;
            item.useTime = item.useAnimation = 30;
			
			item.buffType = BuffID.WellFed;
			item.buffTime = 18000;
            item.noMelee = true;
            item.consumable = true;
			item.UseSound = SoundID.Item2;
            item.autoReuse = false;

        }
		public override bool CanUseItem(Player player)
		{
			player.AddBuff(BuffID.Poisoned, 600);
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(null, "RawFish", 1);
            recipe1.AddTile(TileID.CookingPots);
            recipe1.SetResult(ItemID.CookedFish, 1);
            recipe1.AddRecipe();
			
			ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(null, "RawFish", 1);
            recipe2.AddTile(TileID.WorkBenches);
            recipe2.SetResult(ItemID.Sashimi, 1);
            recipe2.AddRecipe();
		}
    }
}
