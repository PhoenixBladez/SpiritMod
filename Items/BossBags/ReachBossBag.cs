using SpiritMod.Items.Accessory;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Boss;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.Items.Weapon.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossBags
{
    public class ReachBossBag : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("Consumable\nRight Click to open");
        }

        public override void SetDefaults() {
            item.width = 20;
            item.height = 20;
            item.rare = -2;

            item.maxStack = 30;

            item.expert = true;
        }

        public override bool CanRightClick() {
            return true;
        }

        public override void RightClick(Player player) {
            player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(1, 3));
            player.QuickSpawnItem(ModContent.ItemType<DeathRose>());
            player.QuickSpawnItem(ModContent.ItemType<ReachFlowers>(), Main.rand.Next(14, 20));

            int[] lootTable = {
                ModContent.ItemType<ThornBow>(),
                ModContent.ItemType<SunbeamStaff>(),
                ModContent.ItemType<ReachVineStaff>(),
                ModContent.ItemType<ReachBossSword>(),
                ModContent.ItemType<ReachKnife>()
            };
            int loot = Main.rand.Next(lootTable.Length);
            player.QuickSpawnItem(lootTable[loot]);

            if(Main.rand.NextDouble() < 1d / 7)
                player.QuickSpawnItem(ModContent.ItemType<ReachMask>());
            if(Main.rand.NextDouble() < 1d / 10)
                player.QuickSpawnItem(ModContent.ItemType<Trophy5>());
        }
    }
}
