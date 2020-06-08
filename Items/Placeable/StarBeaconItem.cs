using SpiritMod.Tiles.Ambient;
using SpiritMod.Tiles.Furniture.Reach;
using SpiritMod.NPCs.Critters;
using SpiritMod.Mounts;
using SpiritMod.NPCs.Boss.SpiritCore;
using SpiritMod.Boss.SpiritCore;
using SpiritMod.Buffs.Candy;
using SpiritMod.Buffs.Potion;
using SpiritMod.Projectiles.Pet;
using SpiritMod.Buffs.Pet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable
{
    public class StarBeaconItem : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Astralite Beacon");
            Tooltip.SetDefault("'Powered by galactic energy'");
        }


        public override void SetDefaults() {
            item.width = 28;
            item.height = 22;
            item.value = Item.sellPrice(0, 0, 10, 0);

            item.maxStack = 99;

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = ModContent.TileType<StarBeacon>();
        }
    }
}