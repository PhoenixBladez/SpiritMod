using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.OverseerArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class ShadowMagic : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowspirit Headdress");
			Tooltip.SetDefault("Increases magic and summon damage by 27% \n Increases magic crit chance by 16% and max minions by 4\n Increases movement speed by 25% \n Increases max mana by 120 and reduces magic cost by 7%");
		}

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 30;
            item.value = 200000;
            item.rare = 11;

            item.defense = 16;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("ShadowSBody") && legs.type == mod.ItemType("ShadowLegs");
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Magic hits spawn Soul Shards to chase down foes! \n 'You have become the Guardian' \n Double tap to dash repeatedly \n You are surrounded by protective Spirits, which deflect projectiles";
            player.GetModPlayer<MyPlayer>(mod).magicshadowSet = true;
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
            player.magicCrit += 16;
            player.minionDamage += 0.27f;
            player.maxMinions += 4;
            player.magicDamage += 0.27F;
            player.moveSpeed += 0.25F;
            player.manaCost -= 0.07f;
            player.statManaMax2 += 120;
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
