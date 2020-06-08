
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class WheezerScale : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Wheezer Scale");
            Tooltip.SetDefault("Melee hits on foes may cause them to emit a cloud of poisonous gas\nIncreases melee speed by 5%");

        }


        public override void SetDefaults() {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 1, 40, 0);
            item.rare = 2;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetSpiritPlayer().wheezeScale = true;
            player.meleeSpeed += .05f;
        }
    }
}
