
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Weapon.Swung;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossBags
{
    public class SpiritCoreBag : ModItem
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
            player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(7, 12));
            player.QuickSpawnItem(ModContent.ItemType<ShadowLens>());

            int[] lootTable = { 
                ModContent.ItemType<SummonStaff>(), 
                ModContent.ItemType<ShadowStaff>(), 
                ModContent.ItemType<WispSword>() 
            };
            int loot = Main.rand.Next(lootTable.Length);
            player.QuickSpawnItem(lootTable[loot]);

            if(Main.rand.Next(4) == 1)
                player.QuickSpawnItem(ModContent.ItemType<SpiritBar>(), Main.rand.Next(3, 6));

            if(Main.rand.Next(4) == 1)
                player.QuickSpawnItem(ModContent.ItemType<StellarBar>(), Main.rand.Next(3, 6));

            if(Main.rand.Next(4) == 1)
                player.QuickSpawnItem(ModContent.ItemType<Rune>(), Main.rand.Next(3, 6));

            if(Main.rand.Next(4) == 1)
                player.QuickSpawnItem(ModContent.ItemType<SoulShred>(), Main.rand.Next(5, 9));

            if(Main.rand.Next(4) == 1)
                player.QuickSpawnItem(ModContent.ItemType<DuskStone>(), Main.rand.Next(4, 8));

            if(Main.rand.Next(4) == 1)
                player.QuickSpawnItem(ModContent.ItemType<SpiritCrystal>(), Main.rand.Next(3, 6));

            if(Main.rand.NextDouble() < 1d / 7)
                player.QuickSpawnItem(Armor.Masks.SpiritCoreMask._type);
            if(Main.rand.NextDouble() < 1d / 10)
                player.QuickSpawnItem(Boss.Trophy10._type);
        }
    }
}
