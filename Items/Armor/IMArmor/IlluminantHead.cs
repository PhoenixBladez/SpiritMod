using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.IMArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class IlluminantHead : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Cowl");
			Tooltip.SetDefault("Increases max damage by 10% and reduces damage taken by 5%");
		}
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = 100000;
            item.rare = 7;
            item.defense = 14;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("IlluminantGarb") && legs.type == mod.ItemType("IlluminantLegs");  
        }
        public override void UpdateArmorSet(Player player)
        {            
            player.setBonus = "Your attacks bathe enemies in Holy Light\nHoly Light reduces your foes' defense\nWisps of illuminant energy surround for you ";
            player.GetModPlayer<MyPlayer>(mod).illuminantSet = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.magicDamage += 0.1f;
            player.meleeDamage += 0.1f;
            player.thrownDamage += 0.1f;
            player.rangedDamage += 0.1f;
            player.minionDamage += 0.1f;

            player.endurance += 0.05f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "IlluminatedCrystal", 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}