using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public static class SpawnHelper
	{
		//Compare these with Main.invasionType to determine the invasion type.
		public const int INVASION_NONE = 0;
		public const int INVASION_GOBLINS = 1;
		public const int INVASION_FROST = 2;
		public const int INVASION_PIRATES = 3;
		public const int INVASION_MARTIANS = 4;

		private static double currentTime = -1;
		private static int currentPlayer;
		private static SpawnFlags currentFlags;
		private static SpawnZones currentZones;

		private static void ProcessSpawnInfo(NPCSpawnInfo info)
		{
			Player player = info.player;
			if (Main.time == currentTime &&
				player.whoAmI == currentPlayer)
				return;
			currentTime = Main.time;
			currentPlayer = player.whoAmI;

			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			currentZones = (SpawnZones)
				((int)player.zone1 +
				((int)player.zone2 << 1) +
				((int)player.zone3 << 2) +
				((int)player.zone4 << 3));
			bool notUnderground = player.position.Y < Main.worldSurface;
			currentFlags =
				(info.desertCave ? SpawnFlags.Desertcave : SpawnFlags.None) |
				(info.granite ? SpawnFlags.Granite : SpawnFlags.None) |
				(info.marble ? SpawnFlags.Marble : SpawnFlags.None) |
				(info.lihzahrd ? SpawnFlags.Lihzahrd : SpawnFlags.None) |
				(info.spiderCave ? SpawnFlags.SpiderCave : SpawnFlags.None) |
				(info.sky ? SpawnFlags.Sky : SpawnFlags.None) |
				(info.water ? SpawnFlags.Water : SpawnFlags.None) |
				(info.playerSafe ? SpawnFlags.SafeWall : SpawnFlags.None) |
				(info.playerInTown ? SpawnFlags.Town : SpawnFlags.None) |
				(info.invasion ? SpawnFlags.Invasion : SpawnFlags.None) |
				(Main.bloodMoon && !Main.dayTime && notUnderground ? SpawnFlags.Bloodmoon : SpawnFlags.None) |
				(Main.eclipse && Main.dayTime && notUnderground ? SpawnFlags.Eclipse : SpawnFlags.None) |
				(Main.pumpkinMoon && !Main.dayTime && notUnderground ? SpawnFlags.PumpkinMoon : SpawnFlags.None) |
				(Main.snowMoon && !Main.dayTime && notUnderground ? SpawnFlags.FrostMoon : SpawnFlags.None) |
				(Main.slimeRainTime != 0.0 && notUnderground ? SpawnFlags.Slimerain : SpawnFlags.None) |
				(Main.expertMode ? SpawnFlags.Expert : SpawnFlags.None) |
				(Main.hardMode ? SpawnFlags.Hardmode : SpawnFlags.None) |
				(Main.dayTime ? SpawnFlags.Daytime : SpawnFlags.None) |
				(NPC.AnyDanger() ? SpawnFlags.Danger : SpawnFlags.None) |
				(modPlayer.ZoneSpirit ? SpawnFlags.Spirit : SpawnFlags.None) |
				(modPlayer.ZoneReach ? SpawnFlags.Reach : SpawnFlags.None) |
				(MyWorld.BlueMoon && !Main.dayTime && notUnderground ? SpawnFlags.BlueMoon : SpawnFlags.None) |
				(Tide.TideWorld.TheTide && player.ZoneBeach ? SpawnFlags.Tide : SpawnFlags.None);
		}

		/* Useful info
			Main.halloween;
			Main.xMas;
			Main.moonPhase;
			NPC.MoonLordCountdown;
			NPC.saved???
			NPC.downed???
			NPC.waveNumber
		*/

		public static bool SupressSpawns(NPCSpawnInfo info,
				SpawnFlags required = SpawnFlags.None, SpawnZones requiredZones = SpawnZones.None,
				SpawnFlags forbidden = SpawnFlags.Forbidden, SpawnZones forbiddenZones = SpawnZones.Forbidden)
		{
			forbidden &= ~required;
			forbiddenZones &= ~requiredZones;
			ProcessSpawnInfo(info);
			return
				(currentFlags & forbidden) != SpawnFlags.None ||
				(currentZones & forbiddenZones) != SpawnZones.None ||
				(currentFlags & required) != required ||
				(currentZones & requiredZones) != requiredZones;
		}


		public static float LayerSpaceEnd
			=> (float)(3.2 * Main.worldSurface + (Main.maxTilesX > 7400 ? 640f : 160f) + 1060d); //1040f
		public static float LayerSurfaceStart
			=> LayerSpaceEnd;
		public static float LayerSurfaceEnd
			=> (float)(Main.worldSurface * 16f);
		public static float LayerDirtStart
			=> LayerSurfaceEnd;
		public static float LayerDirtEnd
			=> (float)(Main.rockLayer * 16f + 600f);
		public static float LayerCavernStart
			=> LayerDirtEnd;
		public static float LayerCavernEnd
			=> (float)(Main.bottomWorld - 4800f - (Main.maxTilesX>7400 ? -24f : Main.maxTilesX > 5300 ? 40f : 8f));
		public static float LayerPreUnderworldStart
			=> LayerCavernEnd;
		public static float LayerPreUnderworldEnd
			=> (float)(Main.bottomWorld - 3152f - (Main.maxTilesX > 7400 ? -24f : Main.maxTilesX > 5300 ? 40f : 8f));
		public static float LayerUnderworldStart
			=> LayerPreUnderworldEnd;
		public static float LayerLavaStart //I have no idea if this is correct
			=> (float)(Main.bottomWorld - Main.rockLayer * 16f);


		public static bool ZoneOcean(Entity ent)
		{
			int tileX = (int)((ent.position.X + (ent.width >> 1)) * 0.0625f);
			int tileY = (int)((ent.position.Y + ent.height) * 0.0625f);

			return (tileX < 380 || tileX > Main.maxTilesX - 380) && tileY < Main.rockLayer;
		}

		public static bool ZoneUnderworld(Entity ent)
		{
			float Y = ent.position.Y + ent.height;
			return Y > LayerUnderworldStart;
		}

		public static bool ZonePreUnderworld(Entity ent)
		{
			float Y = ent.position.Y + ent.height;
			return Y > LayerPreUnderworldStart && Y <= LayerPreUnderworldEnd;
		}

		public static bool ZoneCavern(Entity ent)
		{
			float Y = ent.position.Y + ent.height;
			return Y > LayerCavernStart && Y <= LayerCavernEnd;
		}

		public static bool ZoneDirt(Entity ent)
		{
			float Y = ent.position.Y + ent.height;
			return Y > LayerDirtStart && Y <= LayerDirtEnd;
		}

		public static bool ZoneSurface(Entity ent)
		{
			float Y = ent.position.Y + ent.height;
			return Y > LayerSurfaceStart && Y <= LayerSurfaceEnd;
		}

		public static bool ZoneSpace(Entity ent)
		{
			float Y = ent.position.Y + ent.height;
			return Y <= LayerSpaceEnd;
		}

		//Best ore
		//Lang.mapLegend.FromType(Main.player[Main.myPlayer].bestOre)


	}
}
