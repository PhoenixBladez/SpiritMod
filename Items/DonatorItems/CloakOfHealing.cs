using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
    [AutoloadEquip(EquipType.Back)]
    public class CloakOfHealing : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cloak Of Healing");
			Tooltip.SetDefault("Minions have a small chance to return life\n~Donator Item~");
		}
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 28;
            item.rare = 2;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>(mod).HealCloak = true;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(188, 1);
			recipe.AddIngredient(null, "OldLeather", 5);
			recipe.AddIngredient(null, "BloodFire", 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
