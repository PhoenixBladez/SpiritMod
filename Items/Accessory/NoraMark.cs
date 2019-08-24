using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class NoraMark : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nora's Mark");
			Tooltip.SetDefault("Increases minion damage by 18%\nIncreases maximum amount of minions by 1\nReduces damage taken by 15% when under half health\n~Donator Item~");
		}


        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 4;
            item.accessory = true;
            item.defense = 2;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.minionDamage += 0.18f;
            player.maxMinions += 1;
            if (player.statLife < player.statLifeMax2 / 2)
            {
                player.endurance += 0.15f;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SummonerEmblem, 1);
            recipe.AddIngredient(ItemID.CobaltShield, 1);
            recipe.AddRecipeGroup("GoldBars", 5);
            recipe.AddIngredient(ItemID.SoulofSight, 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }
    }
}
