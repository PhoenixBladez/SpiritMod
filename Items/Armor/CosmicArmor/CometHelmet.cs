using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CosmicArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class CometHelmet : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cosmic Helm");
			Tooltip.SetDefault("Increases throwing damage by 32% and throwing critical strike chance by 15%");
		}
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = 100000;
            item.rare = 10;
            item.defense = 19;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("CometArmor") && legs.type == mod.ItemType("CometLegs");  
        }
        public override void UpdateArmorSet(Player player)
        {
            int dust = Dust.NewDust(player.position, player.width, player.height, 133);
            Main.dust[dust].scale = 0.5f;
            player.setBonus = "Press the 'Armor Bonus' hotkey to spawn unstable stars around you \n 1 minute cooldown";
            player.GetModPlayer<MyPlayer>(mod).cometSet = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.thrownCrit += 15;
            player.thrownDamage += 0.32f;
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AccursedRelic", 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}