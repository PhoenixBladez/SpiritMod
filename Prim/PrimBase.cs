using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Prim
{
	public class PrimTrailManager
	{
		public const int DrawProjectile = 1;
		public const int DrawNPC = 2;
		public List<PrimTrail> _trails = new List<PrimTrail>();

		public RenderTarget2D primTargetNPC;
		public RenderTarget2D primTargetProjectile;
		public void LoadContent(GraphicsDevice GD)
		{
			primTargetNPC = new RenderTarget2D(GD, Main.screenWidth / 2, Main.screenHeight / 2);
			primTargetProjectile = new RenderTarget2D(GD, Main.screenWidth / 2, Main.screenHeight / 2);
		}

		public void DrawTrailsNPC(SpriteBatch spriteBatch, GraphicsDevice gD)
		{
			RenderTargetBinding[] bindings = gD.GetRenderTargets();

			gD.SetRenderTarget(primTargetNPC);
			gD.Clear(Color.Transparent);
			spriteBatch.Begin();
			foreach (PrimTrail trail in _trails.ToArray().Where(x => x.DrawType == DrawNPC)) {
				if (trail.Pixellated && !trail.Disabled)
					trail.Draw();
			}
			spriteBatch.End();
			gD.SetRenderTargets(bindings);
		}
		public void DrawTargetNPC(SpriteBatch spriteBatch)
		{
			if (primTargetNPC != null)
				Main.spriteBatch.Draw(primTargetNPC, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
			foreach (PrimTrail trail in _trails.ToArray().Where(x => x.DrawType == DrawNPC)) {
				if (!trail.Pixellated && !trail.Disabled)
					trail.Draw();
			}
		}
		public void DrawTrailsProj(SpriteBatch spriteBatch, GraphicsDevice gD)
		{
			RenderTargetBinding[] bindings = gD.GetRenderTargets();

			gD.SetRenderTarget(primTargetProjectile);
			gD.Clear(Color.Transparent);
			spriteBatch.Begin();
			foreach (PrimTrail trail in _trails.ToArray().Where(x => x.DrawType == DrawProjectile)) {
				if (trail.Pixellated && !trail.Disabled)
					trail.Draw();
			}
			spriteBatch.End();
			gD.SetRenderTargets(bindings);
		}
		public void DrawTargetProj(SpriteBatch spriteBatch)
		{
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			if (primTargetProjectile != null)
				Main.spriteBatch.Draw(primTargetProjectile, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
			spriteBatch.End();
			foreach (PrimTrail trail in _trails.ToArray().Where(x => x.DrawType == DrawProjectile)) {
				if (!trail.Pixellated && !trail.Disabled)
					trail.Draw();
			}
		}

		public void UpdateTrails()
		{
			foreach (PrimTrail trail in _trails.ToArray()) 
				trail.Update();
		}

		public void CreateTrail(PrimTrail trail)
		{
			if (!Main.dedServ) //never make trails on servers
				_trails.Add(trail);
		}
	}
}