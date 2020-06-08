using Terraria.ID;
using Terraria.ModLoader;
using AsteroidBoxTile = SpiritMod.Tiles.MusicBox.AsteroidBox;

namespace SpiritMod.Items.Placeable.MusicBox
{
    public class AsteroidBox : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Music Box (Asteroid Field)");
        }

        public override void SetDefaults() {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.createTile = ModContent.TileType<AsteroidBoxTile>();
            item.width = 24;
            item.height = 24;
            item.rare = 4;
            item.value = 100000;
            item.accessory = true;
        }
    }
}
