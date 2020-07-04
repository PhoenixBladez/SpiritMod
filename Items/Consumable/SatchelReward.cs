using SpiritMod.Items.Material;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Items.Placeable.Furniture.AdvPaintings;
using SpiritMod.Items.Weapon.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class SatchelReward : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mysterious Satchel");
			Tooltip.SetDefault("'The Adventurer's been feeling inspired!'\nContains two random paintings");
		}


		public override void SetDefaults()
		{
			item.width = 52;
			item.height = 32;
			item.rare = -11;
			item.maxStack = 999;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.consumable = true;

		}
		public override bool CanRightClick()
		{
			return true;
		}

		public override void RightClick(Player player)
		{
			int[] lootTable = {
				ModContent.ItemType<AdvPainting1>(),
                ModContent.ItemType<AdvPainting2>(),
                ModContent.ItemType<AdvPainting3>(),
                ModContent.ItemType<AdvPainting4>(),
                ModContent.ItemType<AdvPainting5>(),
                ModContent.ItemType<AdvPainting6>(),
                ModContent.ItemType<AdvPainting7>(),
                ModContent.ItemType<AdvPainting8>(),
                ModContent.ItemType<AdvPainting9>(),
                ModContent.ItemType<AdvPainting10>(),
                ModContent.ItemType<AdvPainting11>(),
                ModContent.ItemType<AdvPainting12>(),
                ModContent.ItemType<AdvPainting13>(),
                ModContent.ItemType<AdvPainting14>(),
                ModContent.ItemType<AdvPainting15>(),
                ModContent.ItemType<AdvPainting16>(),
                ModContent.ItemType<AdvPainting17>(),
                ModContent.ItemType<AdvPainting18>(),
                ModContent.ItemType<FishingPainting>(),

            };
			int loot = Main.rand.Next(lootTable.Length);
            int loot1 = Main.rand.Next(lootTable.Length);

            player.QuickSpawnItem(lootTable[loot]);
            player.QuickSpawnItem(lootTable[loot1]);
        }
	}
}
