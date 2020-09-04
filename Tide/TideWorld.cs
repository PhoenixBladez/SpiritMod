using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod;
namespace SpiritMod.Tide
{
	public class TideWorld : ModWorld
	{
		public static int TidePoints = 0;
		public static int EnemyKills = 0;
		public static bool TheTide;

		public override void Initialize()
		{
			TheTide = false;
		}

		public override void PostUpdate()
		{
			//TidePoints = EnemyKills / 2;
			if (TidePoints >= 100) {
				Main.NewText("The Tide has waned!", 61, 255, 142);
				TidePoints = 0;
				EnemyKills = 0;
				TheTide = false;
				if (!MyWorld.downedTide)
                {
                    Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/DeathSounds/TideComplete"));
                    MyWorld.downedTide = true;
                }
			}
		}
	}
}
