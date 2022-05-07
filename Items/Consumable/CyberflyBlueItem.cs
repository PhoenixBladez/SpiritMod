using SpiritMod.NPCs.Critters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class CyberflyBlueItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blue Cyberfly");
			Tooltip.SetDefault("'Real or digital?'");
		}

		public override void SetDefaults()
		{
			item.width = item.height = 32;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 99;
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.sellPrice(0, 0, 1, 0);
			item.useTime = item.useAnimation = 20;
			item.bait = 20;

			item.noMelee = true;
			item.consumable = true;
			item.autoReuse = true;

		}
		public override bool UseItem(Player player)
		{
			NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<CyberflyBlue>());
			return true;
		}
	}
}
