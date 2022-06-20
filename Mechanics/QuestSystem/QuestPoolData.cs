namespace SpiritMod.Mechanics.QuestSystem
{
	public struct QuestPoolData
	{
		public float? NewRate; //Spawn rate mod
		public bool Exclusive; //By default, increase spawns only if

		public QuestPoolData(float? rate, bool exc)
		{
			NewRate = rate;
			Exclusive = exc;
		}
	}
}
