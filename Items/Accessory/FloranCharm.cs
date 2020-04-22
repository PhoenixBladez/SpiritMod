using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Neck)]
    public class FloranCharm : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Hunting Charm");
			Tooltip.SetDefault("While standing on grass of any kind, maximum life increases by 20 and life regeneration increases slightly");
		}


        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.value = Item.buyPrice(0, 0, 20, 0);
            item.rare = 2;
            item.defense = 2;

            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetSpiritPlayer().floranCharm = true;
        }
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "FloranBar", 9);
            modRecipe.AddTile(TileID.Anvils);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
    }
}
