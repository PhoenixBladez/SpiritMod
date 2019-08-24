/*using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class MartianTeleporter : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Strange Beacon");
			Tooltip.SetDefault("'How exactly does this work? I think I need to aim upwards' \n 'Hopefully it calls down something friendly...'");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.rare = 9;
            item.maxStack = 99;
            item.value = 100000;
            item.useStyle = 4;
            item.useTime = item.useAnimation = 20;

            item.noMelee = true;
            item.consumable = true;
            item.autoReuse = false;

            item.UseSound = SoundID.Item43;
        }
        public override bool CanUseItem(Player player)
        {
            if (!NPC.AnyNPCs(mod.NPCType("Martian")))
                return true;
            return false;
        }

        public override bool UseItem(Player player)
        {
            NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - 64, mod.NPCType("Martian"));
            return true;
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ItemID.Ectoplasm, 8);
            modRecipe.AddIngredient(ItemID.LihzahrdPowerCell, 1);
            modRecipe.AddIngredient(ItemID.MartianConduitPlating, 100);
            modRecipe.AddTile(134);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }

    }
}*/
