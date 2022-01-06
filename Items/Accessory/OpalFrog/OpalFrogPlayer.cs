using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SpiritMod.Items.Accessory.OpalFrog
{
	public class OpalFrogPlayer : ModPlayer
	{
		//Whether or not the player automatically unhooks upon reaching the end of their hook
		public bool AutoUnhook { get; set; } = false;

		//Factor at which the player's hook range and pull speed is multiplied by
		public float HookStat { get; set; }

		public float LastHookStat { get; private set; } = 1f;

		public override void ResetEffects()
		{
			AutoUnhook = false;

			LastHookStat = HookStat;
			HookStat = 1f;
		}

		public override void PreUpdate()
		{
			//No tmod hook for updating misc equips for some reason, manual iteration necessary
			foreach (Item item in player.miscEquips)
				OpalFrogGItem.UpdateItem(item, player);
		}
	}
}