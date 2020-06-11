
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class FieryPendant : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Fiery Pendant");
            Tooltip.SetDefault("6% increased melee damage\nMelee weapons may set enemies on fire");
        }


        public override void SetDefaults() {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = 4;

            item.accessory = true;

            item.defense = 0;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            if(Main.rand.Next(10) > 3) {
                player.magmaStone = true;
            }
            player.meleeDamage *= 1.06f;
        }
    }
}
