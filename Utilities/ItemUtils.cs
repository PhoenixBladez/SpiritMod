using Microsoft.Xna.Framework;
using SpiritMod.Items.Halloween;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod
{
    public static class ItemUtils
    {
        public static bool IsWeapon(this Item item)
        {
            return item.type != 0 && item.stack > 0 && item.useStyle > 0 && (item.damage > 0 || item.useAmmo > 0 && item.useAmmo != AmmoID.Solution);
        }

        public static void DropItem(this Entity ent, int type, int stack = 1)
        {
            Item.NewItem(ent.Hitbox, type, stack);
        }

        public static void DropItem(this Entity ent, int type, float chance)
        {
            if (Main.rand.NextDouble() < chance)
            {
                Item.NewItem(ent.Hitbox, type);
            }
        }

        public static void DropItem(this Entity ent, int type, int min, int max)
        {
            Item.NewItem(ent.Hitbox, type, Main.rand.Next(min, max));
        }

        public static void DropCandy(Player player)
        {
            Mod mod = SpiritMod.instance;

            int effect = Main.rand.Next(100);
            if (effect < 9)
            {
                player.QuickSpawnItem(ModContent.ItemType<Taffy>());
            }
            else if (effect < 29)
            {
                player.QuickSpawnItem(ModContent.ItemType<Candy>());
            }
            else if (effect < 49)
            {
                player.QuickSpawnItem(ModContent.ItemType<ChocolateBar>());
            }
            else if (effect < 59)
            {
                player.QuickSpawnItem(ModContent.ItemType<HealthCandy>());
            }
            else if (effect < 69)
            {
                player.QuickSpawnItem(ModContent.ItemType<ManaCandy>());
            }
            else if (effect < 79)
            {
                player.QuickSpawnItem(ModContent.ItemType<Lollipop>());
            }
            else if (effect < 83)
            {
                player.QuickSpawnItem(ModContent.ItemType<Apple>());
            }
            else if (effect < 95)
            {
                player.QuickSpawnItem(ModContent.ItemType<MysteryCandy>());
            }
            else
            {
                player.QuickSpawnItem(ModContent.ItemType<GoldCandy>());
            }
        }

        public static Color RarityColor(this Item item, float alpha = 1)
        {
            if (alpha > 1)
            {
                alpha = 1;
            }
            else if (alpha <= 0)
            {
                return Color.Transparent;
            }

            switch (item.rare)
            {
                case -11:
                    return new Color((byte)(255f * alpha), (byte)(175f * alpha), (byte)(0f * alpha), (byte)(alpha * 255));

                case -1:
                    return new Color((byte)(130f * alpha), (byte)(130f * alpha), (byte)(130f * alpha), (byte)(alpha * 255));

                case 1:
                    return new Color((byte)(150f * alpha), (byte)(150f * alpha), (byte)(255f * alpha), (byte)(alpha * 255));

                case 2:
                    return new Color((byte)(150f * alpha), (byte)(255f * alpha), (byte)(150f * alpha), (byte)(alpha * 255));

                case 3:
                    return new Color((byte)(255f * alpha), (byte)(200f * alpha), (byte)(150f * alpha), (byte)(alpha * 255));

                case 4:
                    return new Color((byte)(255f * alpha), (byte)(150f * alpha), (byte)(150f * alpha), (byte)(alpha * 255));

                case 5:
                    return new Color((byte)(255f * alpha), (byte)(150f * alpha), (byte)(255f * alpha), (byte)(alpha * 255));

                case 6:
                    return new Color((byte)(210f * alpha), (byte)(160f * alpha), (byte)(255f * alpha), (byte)(alpha * 255));

                case 7:
                    return new Color((byte)(150f * alpha), (byte)(255f * alpha), (byte)(10f * alpha), (byte)(alpha * 255));

                case 8:
                    return new Color((byte)(255f * alpha), (byte)(255f * alpha), (byte)(10f * alpha), (byte)(alpha * 255));

                case 9:
                    return new Color((byte)(5f * alpha), (byte)(200f * alpha), (byte)(255f * alpha), (byte)(alpha * 255));

                case 10:
                    return new Color((byte)(255f * alpha), (byte)(40f * alpha), (byte)(100f * alpha), (byte)(alpha * 255));
            }

            if (item.rare >= 11)
            {
                return new Color((byte)(180f * alpha), (byte)(40f * alpha), (byte)(255f * alpha), (byte)(alpha * 255));
            }

            if (item.expert || item.rare == -12)
            {
                return new Color((byte)(Main.DiscoR * alpha), (byte)(Main.DiscoG * alpha), (byte)(Main.DiscoB * alpha), (byte)(alpha * 255));
            }

            return new Color((byte)(255 * alpha), (byte)(255 * alpha), (byte)(255 * alpha), (byte)(alpha * 255));
        }
    }
}
