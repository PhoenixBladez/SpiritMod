using Terraria.ID;
using Terraria.ModLoader;
using ScarabBoxTile = SpiritMod.Tiles.MusicBox.ScarabBox;
namespace SpiritMod.Items.Placeable.MusicBox
{
    public class ScarabBox : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Music Box (Scarabeus)");
        }

        public override void SetDefaults() {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.createTile = ModContent.TileType<ScarabBoxTile>();
            item.width = 24;
            item.height = 24;
            item.rare = 4;
            item.value = 100000;
            item.accessory = true;
        }
    }
}
