using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Shield)]
    public class TitanboundBulwark : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Titanbound Bulwark");
            Tooltip.SetDefault("As your health decreases, your mana regeneration increases\nReduces damage taken by 10%\nIncreases life regeneration");
        }
        public override void SetDefaults() {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 51, 0, 0);
            item.rare = 9;
            item.accessory = true;
            item.defense = 2;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            float manaBoost = (float)(player.statLifeMax2 - player.statLife) / (float)player.statLifeMax2 * 50f;
            player.manaRegen += (int)manaBoost;
            player.endurance += .1f;
            player.lifeRegen += 3;
        }
    }
}
