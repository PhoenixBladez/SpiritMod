using Terraria.ModLoader;

namespace SpiritMod.Utilities.ILEdits
{
	public abstract class ILEdit
	{
		public abstract void Load(Mod mod);

		public virtual void Unload(Mod mod) { }
	}
}
