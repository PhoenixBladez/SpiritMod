using System;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.EventSystem
{
	public class EventPlayer : ModPlayer
	{
		public static event Action OnModifyScreenPosition;

		public override void ModifyScreenPosition() => OnModifyScreenPosition?.Invoke();
	}
}
