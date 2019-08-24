using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
    //[AutoloadEquip(EquipType.Back)]
	[AutoloadEquip(EquipType.HandsOn)]
    public class GuardianCloak : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Guardian's Cloak");
			Tooltip.SetDefault("Minions have a large chance to return life\nMinions do 18% less damage\nIncreases sentry count by 1.\nIncreases armor penetration by 5.\n~Donator Item~");
		}
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 28;
            item.rare = 9;
            item.value = Terraria.Item.sellPrice(0, 7, 0, 0);
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>(mod).VampireCloak = true;
			player.minionDamage -= 0.18f;
			 player.maxTurrets += 1;
			  player.armorPenetration += 5;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CloakOfVampire");
			recipe.AddIngredient(null, "GuardianArm");
			recipe.AddIngredient(null, "EternityEssence");
            recipe.AddTile(114);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
