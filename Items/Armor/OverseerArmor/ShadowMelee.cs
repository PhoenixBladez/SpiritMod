using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.OverseerArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class ShadowMelee : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowspirit Visor");
			Tooltip.SetDefault("Increases melee damage by 28% and melee crit chance by 25% \n Increases melee speed by 30% \n Reduces damage taken by 17%");
		}

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 30;
            item.value = 200000;
            item.rare = 11;

            item.defense = 38;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("ShadowSBody") && legs.type == mod.ItemType("ShadowLegs");
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Melee hits spawn Soul Shards to chase down foes! \n 'You have become the Guardian' \n Double tap to dash repeatedly \n You are surrounded by protective Spirits, which deflect projectiles.";
            player.GetModPlayer<MyPlayer>(mod).meleeshadowSet = true;
            player.GetModPlayer<MyPlayer>(mod).shadowSet = true;

            if (Main.rand.Next(4) == 1)
            {

                Dust.NewDust(player.position, player.width, player.height, 187);
            }
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeCrit += 25;
            player.endurance += 0.17F;
            player.meleeDamage += 0.28F;
            player.meleeSpeed += 0.30F;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "EternityEssence", 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
