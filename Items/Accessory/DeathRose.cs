using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class DeathRose : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Briar Blossom");
            Tooltip.SetDefault("Successfully hitting enemies may cause the player to recieve 'Nature's Fury', increasing damage and movement speed by 5%\nIncreases summoning damage by 6%\nPress a hotkey to rapidly grow homing nature energy around the cursor position\n1 minute cooldown");
        }


        public override void SetDefaults() {
            item.width = 32;
            item.height = 36;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = 6;
            item.accessory = true;
            item.expert = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetSpiritPlayer().deathRose = true;
            player.minionDamage += 0.06f;
        }
    }
}
