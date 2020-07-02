using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Sounds.Kakamora
{
	public class KakamoraHit : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
		{
			soundInstance = sound.CreateInstance();
			soundInstance.Volume = volume * .75f;
			soundInstance.Pan = pan;
			soundInstance.Pitch = Main.rand.Next(-3, 10) / 25f;
			return soundInstance;

		}
	}
}
