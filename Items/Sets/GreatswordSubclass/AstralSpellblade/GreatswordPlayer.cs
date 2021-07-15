using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpiritMod.Items.Sets.GreatswordSubclass.AstralSpellblade
{
	public class GreatswordPlayer : ModPlayer
	{
		public int Combo { get; set; }
		private int _timeLeft;

		public override void Initialize() => Combo = 0;

		public override void ResetEffects()
		{
			_timeLeft = (player.itemTime > 0) ? 40 : Math.Max(_timeLeft - 1, 0);
			Combo = (_timeLeft == 0) ? 0 : Combo;
		}
	}
}