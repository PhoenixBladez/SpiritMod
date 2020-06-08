using Terraria.ModLoader;


namespace SpiritMod.Items.Armor.AstronautVanity
{
    [AutoloadEquip(EquipType.Head)]
    public class AstronautHelm : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Astronaut Helmet");

        }
        public override void SetDefaults() {
            item.width = 30;
            item.height = 30;
            item.value = Terraria.Item.sellPrice(0, 0, 25, 0);
            item.rare = 2;
            item.vanity = true;
        }
    }
}
