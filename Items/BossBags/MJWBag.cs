
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Armor.JellynautHelmet;
using SpiritMod.Items.Boss;
using SpiritMod.Items.Equipment;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Weapon.Yoyo;
using SpiritMod.Items.Equipment;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable;

namespace SpiritMod.Items.BossBags
{
	public class MJWBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Consumable\nRight Click to open");
		}


		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.rare = -2;

			item.maxStack = 30;

			item.expert = true;
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(3, 6));
			if (Main.rand.NextDouble() < 1d / 7)
				player.QuickSpawnItem(ModContent.ItemType<MJWMask>());
			if (Main.rand.NextDouble() < 1d / 10)
				player.QuickSpawnItem(ModContent.ItemType<MJWTrophy>());
            player.QuickSpawnItem(ModContent.ItemType<Cornucopion>());
            int[] lootTable = {
                ModContent.ItemType<Moonshot>(),
                ModContent.ItemType<Moonburst>(),
                ModContent.ItemType<JellynautBubble>(),
                ModContent.ItemType<MoonjellySummonStaff>()
            };
            int loot = Main.rand.Next(lootTable.Length);
			int lunazoastack = Main.rand.Next(20, 22);
			player.QuickSpawnItem(lootTable[loot]);
			if (lootTable[loot] == ModContent.ItemType<Moonshot>())
				lunazoastack += Main.rand.Next(20, 30);

			player.QuickSpawnItem(ModContent.ItemType<TinyLunazoaItem>(), lunazoastack);
		}
	}
}
