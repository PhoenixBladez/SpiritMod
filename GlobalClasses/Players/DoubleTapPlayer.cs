using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.GlobalClasses.Players
{
	/// <summary>Class that handles double tap abilities in either direction.</summary>
	class DoubleTapPlayer : ModPlayer
	{
		public static event Action<Player, int> OnDoubleTap = null;

		public const int UpTapThreshold = 14;

		public int lastTapUpTimer = 0;
		public bool controlUpLast = false;

		public bool UpPress => !player.controlUp && controlUpLast;

		public override void ResetEffects() => lastTapUpTimer--;

		public override void SetControls()
		{
			if (UpPress)
			{
				lastTapUpTimer = lastTapUpTimer < 0 ? UpTapThreshold : lastTapUpTimer + UpTapThreshold;

				if (lastTapUpTimer > UpTapThreshold)
				{
					OnDoubleTap?.Invoke(player, !Main.ReversedUpDownArmorSetBonuses ? 1 : 0);

					lastTapUpTimer = 0;
				}
			}

			controlUpLast = player.controlUp;
		}

		public override void PostUpdate()
		{
			base.PostUpdate();
		}

		internal void DoubleTapDown() => OnDoubleTap?.Invoke(player, Main.ReversedUpDownArmorSetBonuses ? 1 : 0);
	}
}
