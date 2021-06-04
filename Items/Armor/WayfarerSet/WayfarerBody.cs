using SpiritMod.Items.Material;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.WayfarerSet
{
    [AutoloadEquip(EquipType.Body)]
    public class WayfarerBody : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wayfarer's Rucksack");
            Tooltip.SetDefault("8% increased movement speed");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = Terraria.Item.sellPrice(0, 0, 40, 0);
            item.rare = 1;
            item.defense = 4;
        }
        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.08f;
            player.runAcceleration += .015f;
        }
    }
}
