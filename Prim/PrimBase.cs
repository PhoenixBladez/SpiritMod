using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;

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

		public void CreateTrail(PrimTrail trail)
		{
			if (Main.netMode == NetmodeID.Server) //never make trails on servers
				return;
			_trails.Add(trail);
		}
	}
}