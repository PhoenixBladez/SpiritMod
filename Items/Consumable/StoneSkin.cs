using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class StoneSkin : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stone Fist");
			Tooltip.SetDefault("Summons nature's Protector");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.rare = 9;
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
            if (!NPC.AnyNPCs(mod.NPCType("Atlas")))
                return true;
            return false;
        }

        public override bool UseItem(Player player)
        {
            Main.PlaySound(15, (int)player.Center.X, (int)player.Center.Y, 0);
            NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - 600, mod.NPCType("Atlas"));

            Main.NewText("The earth is trembling", 255, 60, 255);
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LihzahrdPowerCell, 1);
            recipe.AddIngredient(ItemID.MartianConduitPlating, 20);
            recipe.AddIngredient(ItemID.StoneBlock, 100);
            recipe.AddIngredient(154, 10);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
