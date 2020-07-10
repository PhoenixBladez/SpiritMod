using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Sounds
{
	public class SamuraiUnsheathe : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
		{
			soundInstance = sound.CreateInstance();
			soundInstance.Volume = volume * .75f;
			soundInstance.Pan = pan;
			soundInstance.Pitch = Main.rand.Next(-1, 8) / 25f;
			return soundInstance;

		}
	}
}
