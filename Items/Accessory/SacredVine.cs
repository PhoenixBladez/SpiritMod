
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class SacredVine : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Sacred Vine");
            Tooltip.SetDefault("Oak Heart covers enemies in poisonous pollen\n4% increased melee critical strike chance\nAttacks may briefly increase regeneration");
        }


        public override void SetDefaults() {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 2;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetSpiritPlayer().sacredVine = true;
            player.meleeCrit += 4;
        }
    }
}
