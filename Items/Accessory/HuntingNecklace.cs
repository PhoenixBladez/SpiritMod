using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Neck)]
    public class HuntingNecklace : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Briarhunt Charm");
            Tooltip.SetDefault("Increases armor penetration by 3\nMelee attacks occasionally strike enemies twice\nWhile standing on grass of any kind, melee speed increases by 5% and life regeneration increases slightly\nIncreases critical strike chance by 4%\nAllows for increased night vision in the Briar");

        }


        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 42;
            item.value = Item.buyPrice(0, 1, 20, 0);
            item.rare = 4;
            item.defense = 3;

            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.rangedCrit += 4;
            player.magicCrit += 4;
            player.thrownCrit += 4;
            player.meleeCrit += 4;
            player.GetSpiritPlayer().floranCharm = true;
            player.armorPenetration += 3;
            player.GetSpiritPlayer().cleftHorn = true;
            player.GetSpiritPlayer().reachBrooch = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ReachBrooch", 1);
            recipe.AddIngredient(null, "CleftHorn", 1);
            recipe.AddIngredient(null, "FloranCharm", 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
