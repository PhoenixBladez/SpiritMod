using System.Collections.Generic;
using System.Linq;

namespace SpiritMod.Prim
{
	public class PrimTrailManager
	{
		public const int DrawProjectile = 1;
		public const int DrawNPC = 2;
		public List<PrimTrail> _trails = new List<PrimTrail>();

		public void DrawTrailsNPC()
		{
			foreach (PrimTrail trail in _trails.ToArray().Where(x => x.DrawType == DrawNPC)) 
				trail.Draw();
		}

		public void DrawTrailsProj()
		{
			foreach (PrimTrail trail in _trails.ToArray().Where(x => x.DrawType == DrawProjectile)) 
				trail.Draw();
		}

		public void UpdateTrails()
		{
			foreach (PrimTrail trail in _trails.ToArray()) 
				trail.Update();
		}

		public void CreateTrail(PrimTrail trail) => _trails.Add(trail);

	}
}