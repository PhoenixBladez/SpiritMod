using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Sounds
{
	public class AvianScreech : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
		{
			soundInstance = sound.CreateInstance();
			soundInstance.Volume = volume * .6f;
			soundInstance.Pan = pan;
			return soundInstance;

		}
	}
}
