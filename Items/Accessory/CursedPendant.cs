
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class CursedPendant : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Cursed Pendant");
            Tooltip.SetDefault("Increases melee damage by 6%\nWeapons have a 15% chance to inflict Cursed Inferno");
        }


        public override void SetDefaults() {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 2, 0, 0);
            item.rare = 4;

            item.accessory = true;

            item.defense = 0;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetSpiritPlayer().CursedPendant = true;
            player.meleeDamage *= 1.06f;
        }
    }
}
