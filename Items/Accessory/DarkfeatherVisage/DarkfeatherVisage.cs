using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.DarkfeatherVisage
{
    [AutoloadEquip(EquipType.Head)]
    public class DarkfeatherVisage : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkfeather Visage");
            Tooltip.SetDefault("Generates exploding darkfeather bolts around the player\nIncreases magic damage by 5%\nCan be worn in the accessory or helmet slot");

			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = false;
		}

        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 49;
            Item.value = Item.sellPrice(0, 1, 6, 0);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 1;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += .05f;
            player.GetSpiritPlayer().darkfeatherVisage = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += .05f;
            player.GetSpiritPlayer().darkfeatherVisage = true;
        }
    }
}
