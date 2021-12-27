using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.Oracle
{
	public class OracleScripture : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sacred Scripture");
			Tooltip.SetDefault("Calls the Oracle to you.\nDoes nothing when the Oracle is gone.");
		}

		public override void SetDefaults()
		{
			item.width = 46;
			item.height = 36;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 1;
			item.value = Item.sellPrice(0, 0, 3, 0);
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = item.useAnimation = 50;
            item.bait = 24;
			item.noMelee = true;
			item.autoReuse = true;
			item.noUseGraphic = true;
		}

		public override bool UseItem(Player player)
		{
			if (player.GetSpiritPlayer().inMarble)
			{
				int w = NPC.FindFirstNPC(ModContent.NPCType<Oracle>());
				if (w != -1)
				{
					var oracle = Main.npc[w].modNPC as Oracle;
					oracle.Teleport = 200;
					oracle.TeleportX = player.Center.X;
					oracle.TeleportY = player.Center.Y;
					return true;
				}
			}
			return false;
		}
	}
}
