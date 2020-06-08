
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Boss;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Spear;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Weapon.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossBags
{
    public class FlyerBag : ModItem
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
            player.QuickSpawnItem(ModContent.ItemType<FossilFeather>(), Main.rand.Next(4, 7));

            int[] lootTable = {
                ModContent.ItemType<SkeletalonStaff>(),
                ModContent.ItemType<Talonginus>(),
                ModContent.ItemType<SoaringScapula>()
            };
            int loot = Main.rand.Next(lootTable.Length);
            player.QuickSpawnItem(lootTable[loot]);

            if(Main.rand.NextDouble() < 1d / 7)
                player.QuickSpawnItem(ModContent.ItemType<FlierMask>());
            if(Main.rand.NextDouble() < 1d / 10)
                player.QuickSpawnItem(ModContent.ItemType<Trophy2>());
        }
    }
}
