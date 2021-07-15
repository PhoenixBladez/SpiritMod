using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiritMod.Items.Weapon.Magic.Rhythm
{
	public interface IRhythmWeapon
	{
		RhythmMinigame Minigame { get; set; }

		void OnBeat(bool success, int combo);
	}
}
