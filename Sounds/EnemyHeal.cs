using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Sounds
{
	public class EnemyHeal : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
		{
			soundInstance = sound.CreateInstance();
			soundInstance.Volume = volume * .45f;
			soundInstance.Pan = pan;
			soundInstance.Pitch = Main.rand.Next(0, 7) / 30f;
			return soundInstance;

		}
	}
}
