using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace SpiritMod.Sounds
{
	public class SlayerStart : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
		{
			soundInstance = sound.CreateInstance();
            type = SoundType.Music;
            return soundInstance;

		}
	}
}
