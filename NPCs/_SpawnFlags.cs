namespace SpiritMod.NPCs
{
	public enum SpawnFlags
	{
		None = 0,

		//This exists because it is part of NPCSpawnInfo
		// It is true, if the player stands in front of desert background
		Desertcave = 1,
		Granite = 1 << 1,
		Marble = 1 << 2,
		Lihzahrd = 1 << 3,
		SpiderCave = 1 << 4,

		//This field is true when sky mobs should spawn,
		// it's not the same as SkyLayer in SpawnFlagsZone!
		Sky = 1 << 5,
		Water = 1 << 6,

		//This is true, when the player is standing in front of a non-natural wall.
		//Enewmies that don't collide with tiles or teleport shouldn't spawn when this is active.
		SafeWall = 1 << 7,
		Town = 1 << 8,
		Invasion = 1 << 9,

		Bloodmoon = 1 << 10,
		Eclipse = 1 << 11,
		PumpkinMoon = 1 << 12,
		FrostMoon = 1 << 13,
		Slimerain = 1 << 14,
		Expert = 1 << 15,
		Hardmode = 1 << 16,
		Daytime = 1 << 17,
		Danger = 1 << 18,

		//Spirit specific flags
		Spirit = 1 << 28,
		Reach = 1 << 29,
		BlueMoon = 1 << 30,
		Tide = 1 << 31,

		//All flags, which do not usually hinder spawning.
		Forbidden = Desertcave | Lihzahrd
			| Sky | Water | Invasion
			| Eclipse | Slimerain
			| PumpkinMoon | FrostMoon
			| BlueMoon | Tide,
		Allowed = ~Forbidden
	}

	public enum SpawnZones
	{
		None = 0,

		//Mapping every field in player.zoneX
		Dungeon = 1,
		Corrupt = 1 << 1,
		Hallow = 1 << 2,
		Meteor = 1 << 3,
		Jungle = 1 << 4,
		Snow = 1 << 5,
		Crimson = 1 << 6,
		Watercandle = 1 << 7,
		Peacecandle = 1 << 8,
		Solar = 1 << 9,
		Vortex = 1 << 10,
		Nebula = 1 << 11,
		Stardust = 1 << 12,
		Desert = 1 << 13,
		Mushroom = 1 << 14,
		Deepdesert = 1 << 15,
		SkyLayer = 1 << 16,
		Overworld = 1 << 17,
		DirtLayer = 1 << 18,
		RockLayer = 1 << 19,
		Underworld = 1 << 20,
		Beach = 1 << 21,
		Rain = 1 << 22,
		Sandstorm = 1 << 23,
		OldOnesArmy = 1 << 24,

		Towers = Solar | Vortex | Nebula | Stardust,
		Underground = DirtLayer | RockLayer,
		Weather = Rain | Sandstorm,

		Forbidden = Dungeon | Meteor | Towers
			| Underworld | OldOnesArmy,
		Allowed = ~Forbidden
	}

}
