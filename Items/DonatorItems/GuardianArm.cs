using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
    [AutoloadEquip(EquipType.HandsOn)]
    public class GuardianArm : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Guardian's Arm");
			Tooltip.SetDefault("Increases sentry count by 1.\n Increases armor penetration by 5.\n~Donator Item~");
		}
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 28;
            item.rare = 5;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			 player.maxTurrets += 1;
			  player.armorPenetration += 5;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Geode", 4);
			recipe.AddIngredient(520, 5);
			recipe.AddIngredient(502, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
