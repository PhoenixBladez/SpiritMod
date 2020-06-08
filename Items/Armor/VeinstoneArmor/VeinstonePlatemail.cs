
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.VeinstoneArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class VeinstonePlatemail : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Veinstone Platemail");
        }

        public override void SetDefaults() {
            item.width = 34;
            item.height = 30;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 4;
            item.vanity = true;
        }
    }
}
