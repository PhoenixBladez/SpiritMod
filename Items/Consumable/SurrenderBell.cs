using SpiritMod.NPCs.Boss.Atlas;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class SurrenderBell : ModItem
	{
		public override void SetStaticDefaults() => Tooltip.SetDefault("Stops any invasion when used\n'A heavenly chime'");

		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 30;
			item.rare = ItemRarityID.Cyan;
			item.maxStack = 99;

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

			Main.NewText("The invaders have been cast out of the world...");
			return true;
		}
	}
}
