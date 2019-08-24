using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class BeserkCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Core of the Berserker");
			Tooltip.SetDefault("Increases armor penetration by 6\nIncreases melee critical strike chance and melee damage by 5%\nHitting foes may cause them to release a cloud of gas and melee critical hits may cause foes to explode\nIncreases melee damage and melee speed by 12% when underground and melee damage and melee speed by 7% when under half health\nReduces damage taken by 4% and occasionally nullifies hostile projectiles");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
        }


		public override void SetDefaults()
		{
			item.width = 22;
            item.height = 22;
			item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 8;
            item.defense = 2;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
            if (player.ZoneRockLayerHeight)
            {
                player.meleeDamage += .12f;
                player.meleeSpeed += .12f;
            }
            player.GetModPlayer<MyPlayer>(mod).wheezeScale = true;
            player.meleeCrit += 5;
            player.armorPenetration += 6;
            player.GetModPlayer<MyPlayer>(mod).infernalFlame = true;
            player.meleeDamage += .05f;
            if (player.statLife <= player.statLifeMax2 / 2)
            {
                player.meleeDamage += 0.07f;
                player.meleeSpeed += 0.07f;
            }
            player.GetModPlayer<MyPlayer>(mod).atmos = true;
            player.endurance += 0.04f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BeserkerShard", 1);
            recipe.AddIngredient(null, "WheezerScale", 1);
            recipe.AddIngredient(null, "BoneTotem", 1);
            recipe.AddIngredient(null, "FieryTrident", 1);
            recipe.AddIngredient(null, "Atmos", 1);
            recipe.AddIngredient(null, "FossilFlower", 1);
            recipe.AddIngredient(null, "SunShard", 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

    }
}
