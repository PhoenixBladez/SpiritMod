using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
    [AutoloadEquip(EquipType.Back)]
    public class CloakOfVampire : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cloak Of The Vampire");
			Tooltip.SetDefault("Minions have a large chance to return life\n Minions do 18% less damage\n~Donator Item~");
		}
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 28;
            item.rare = 6;
            item.value = Terraria.Item.sellPrice(0, 4, 0, 0);
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>(mod).VampireCloak = true;
			player.minionDamage -= 0.18f;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CloakOfSpirit");
			recipe.AddIngredient(1569, 1);
            recipe.AddTile(114);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
