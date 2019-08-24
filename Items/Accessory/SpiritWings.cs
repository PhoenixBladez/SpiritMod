using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Wings)]
    public class SpiritWings : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Wings");
			Tooltip.SetDefault("Allows for flight and slow fall.");
		}
        public override void SetDefaults()
        {
            item.width = 47;
            item.height = 37;
            item.value = 60000;
            item.rare = 5;

            item.accessory = true;

            item.rare = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.wingTimeMax = 150;
			player.GetModPlayer<MyPlayer>(mod).BlueDust = true;
		}

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        { 
            ascentWhenFalling = 0.75f;
			ascentWhenRising = 0.11f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 2.6f;
			constantAscend = 0.135f;
		}

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {

            speed = 7f;
			acceleration *= 2f;
		}  
public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(null, "SpiritBar", 14);
			modRecipe.AddIngredient(575, 12);
			 modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}		
    }
}