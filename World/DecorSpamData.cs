namespace SpiritMod.World
{
	internal struct DecorSpamData
	{
		public int RealRepeats => (int)(BaseRepeats * GlobalExtensions.WorldSize);

		public string Name;
		public int[] Types;
		public int[] ValidGround;
		public int BaseRepeats;
		public bool Forced;
		public (int high, int low) RangeY;

		public DecorSpamData(string name, int[] types, int[] ground, int baseReps, (int high, int low) rangeY, bool forced = true)
		{
			Name = name;
			Types = types;
			ValidGround = ground;
			BaseRepeats = baseReps;
			RangeY = rangeY;
			Forced = forced;
		}
	}
}
