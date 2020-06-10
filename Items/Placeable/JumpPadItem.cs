using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable
{
    public class JumpPadItem : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Jump Pad");
            Tooltip.SetDefault("Launches you");
        }


        public override void SetDefaults() {
            item.width = 20;
            item.height = 20;
            item.rare = 3;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.createTile = mod.TileType("JumpPadTile");
            item.maxStack = 999;
            item.autoReuse = false;
            item.useAnimation = 15;
            item.useTime = 10;
            item.consumable = true;

        }
    }
}
