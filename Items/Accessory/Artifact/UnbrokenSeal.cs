using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Artifact
{
	public class UnbrokenSeal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seal of the Unbroken");
			Tooltip.SetDefault("'It is ages old but remains unscathed\nIncreases maximum life by 1/10 of the player's current life\nWhen under half health, your defense and life regen increase as your health wanes\nIncreases melee damage by 10% and melee speed by 5%\nPowers up Shard of Thanos with 'Unyielding Resolve,' increasing life regeneration and reducing damage taken by 5%");
		}


		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 50;
			item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 5;
			item.defense = 2;
			item.accessory = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = new TooltipLine(mod, "ItemName", "Artifact Accessory");
            line.overrideColor = new Color(100, 0, 230);
            tooltips.Add(line);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			            player.GetModPlayer<MyPlayer>(mod).Resolve = true;
            player.meleeDamage += 0.1f;
            player.meleeSpeed += 0.05f;
            float lifeBoost = (float)((player.statLifeMax2) / 10);
            player.statLifeMax2 += (int)lifeBoost;

            if (player.statLife <= player.statLifeMax2 / 2)
            {
                float defBoost = (float)(player.statLifeMax2 - player.statLife) / (float)player.statLifeMax2 * 15f;
                player.statDefense += (int)defBoost;
                float regenBoost = (float)(player.statLifeMax2 - player.statLife) / (float)player.statLifeMax2 * 6f;
                player.lifeRegen += (int)regenBoost;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FrostLotus", 1);
            recipe.AddIngredient(null, "ChaosEmber", 1);
            recipe.AddIngredient(null, "FireLamp", 1);
            recipe.AddIngredient(ItemID.WarriorEmblem, 1);
            recipe.AddIngredient(null, "SpiritBar", 5);
            recipe.AddIngredient(null, "PrimordialMagic", 50);
            recipe.AddTile(null, "CreationAltarTile");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
}
