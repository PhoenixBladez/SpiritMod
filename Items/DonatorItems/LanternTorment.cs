using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
    //[AutoloadEquip(EquipType.Back)]
	//[AutoloadEquip(EquipType.HandsOn)]
    public class LanternTorment : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lantern Of The Torment");
			Tooltip.SetDefault("Minions have a chance to spawn Tormented Soldiers \n~Donator Item~");
		}
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 28;
            item.rare = 4;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>(mod).TormentLantern= true;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(177, 5);
			recipe.AddIngredient(521, 5);
			recipe.AddIngredient(180, 5);
			recipe.AddIngredient(null, "Chitin", 10);
             recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
