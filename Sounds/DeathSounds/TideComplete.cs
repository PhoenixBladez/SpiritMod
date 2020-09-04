using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace SpiritMod.Sounds.DeathSounds
{
	public class TideComplete : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
		{
			soundInstance = sound.CreateInstance();
            type = SoundType.Music;
			return soundInstance;

		}
	}
}
