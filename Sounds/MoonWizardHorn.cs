using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace VariousWeathers.Sounds
{
	public class MoonWizardHorn : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
		{
			soundInstance = sound.CreateInstance();
			soundInstance.Volume = volume * .5f;
			soundInstance.Pan = pan;
            soundInstance.Pitch = Main.rand.NextFloat(-0.2f, 0.2f);
            return soundInstance;

		}
	}
}
