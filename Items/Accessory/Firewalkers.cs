using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Shoes)]
    public class Firewalkers : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flare Treads");
			Tooltip.SetDefault("Increases movement speed by 12%\nProvides super fast running\nGrants the ability to walk on thin ice, water, and lava\nGives immunity to fire blocks and allows the player to swim in lava for 8 seconds");
		}
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 32;
            item.value = Item.buyPrice(0, 9, 0, 0);
            item.rare = 7;

            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed += 0.12f;
            player.maxRunSpeed += 3f;
            player.iceSkate = true;
            player.waterWalk = true;
            player.fireWalk = true;
            player.lavaMax += 480;
            player.rocketTimeMax = 10;
            player.rocketBoots = 1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FrostsparkBoots, 1);
            recipe.AddIngredient(ItemID.LavaWaders, 1);
            recipe.AddIngredient(null, "InfernalAppendage", 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
