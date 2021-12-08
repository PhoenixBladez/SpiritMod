using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Utilities
{
	/// <summary>
	/// More specific use case trail projectile interface that doesn't have automatic trail spawning, used mainly to handle net syncing
	/// </summary>
	public interface IManualTrailProjectile
	{
		/// <summary>
		/// Method for creating vertex strips, called on projectile creation.
		/// </summary>
		/// <param name="tManager"></param>
		void DoTrailCreation(TrailManager tManager);
	}

	/// <summary>
	/// General use trail projectile interface, that handles automatic spawning and net syncing
	/// </summary>
	public interface ITrailProjectile : IManualTrailProjectile
	{
	}
}