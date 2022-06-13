using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class SurrenderBell : ModItem
	{
		public override void SetStaticDefaults() => Tooltip.SetDefault("Stops any invasion when used\nCan only be used when the invasion has arrived, not when the invasion is approaching\n'A heavenly chime'");

		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 30;
			item.rare = ItemRarityID.Cyan;
			item.maxStack = 99;
			item.value = Item.buyPrice(gold: 25);
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.useTime = item.useAnimation = 20;
			item.noMelee = true;
			item.consumable = true;
			item.autoReuse = false;
		}

		public override bool CanUseItem(Player player) => Main.invasionType != 0;

		public override bool UseItem(Player player)
		{
			Main.PlaySound(SoundID.CoinPickup, (int)player.Center.X, (int)player.Center.Y, 2);
			Main.invasionType = 0;
			Main.invasionSize = 0;

			NetMessage.SendData(MessageID.InvasionProgressReport, -1, -1, null, Main.invasionProgress, Main.invasionProgressMax, Main.invasionProgressIcon, 0f, 0, 0, 0);

			if (Main.netMode == NetmodeID.SinglePlayer)
				Main.NewText("The invaders have called off their attack!", Color.MediumPurple);
			else if (Main.netMode == NetmodeID.Server)
				NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("The invaders have called off their attack!"), Color.MediumPurple, -1);
			return true;
		}
	}
}
