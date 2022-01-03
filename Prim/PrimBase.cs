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

		public BasicEffect pixelEffect;
		public BasicEffect galaxyEffect;
		public void LoadContent(GraphicsDevice GD)
		{
			InitializeTargets(GD);
			pixelEffect = (BasicEffect)SpiritMod.basicEffect.Clone();
			galaxyEffect = (BasicEffect)SpiritMod.basicEffect.Clone();

			Helpers.SetBasicEffectMatrices(ref pixelEffect, Main.GameViewMatrix.Zoom);
			Helpers.SetBasicEffectMatrices(ref galaxyEffect, new Vector2(1));
		}

		public void InitializeTargets(GraphicsDevice GD)
		{
			primTargetNPC = new RenderTarget2D(GD, Main.screenWidth / 2, Main.screenHeight / 2);
			primTargetProjectile = new RenderTarget2D(GD, Main.screenWidth / 2, Main.screenHeight / 2);
		}

		public void DrawTrailsNPC(SpriteBatch spriteBatch, GraphicsDevice gD)
		{
			List<PrimTrail> pixelTrails = new List<PrimTrail>();
			foreach (PrimTrail trail in _trails.ToArray().Where(x => x.DrawType == DrawNPC))
				if (trail.Pixellated && !trail.Disabled)
					pixelTrails.Add(trail);

			if (pixelTrails.Count > 0)
			{
				RenderTargetBinding[] bindings = gD.GetRenderTargets();

				gD.SetRenderTarget(primTargetNPC);
				gD.Clear(Color.Transparent);
				spriteBatch.Begin();

				foreach (PrimTrail trail in pixelTrails)
					trail.Draw();

				spriteBatch.End();
				gD.SetRenderTargets(bindings);
			}
		}

		public void DrawTargetNPC(SpriteBatch spriteBatch)
		{
			List<PrimTrail> pixelTrails = new List<PrimTrail>();
			List<PrimTrail> nonpixelTrails = new List<PrimTrail>();
			foreach (PrimTrail trail in _trails.ToArray().Where(x => x.DrawType == DrawNPC))
			{
				if (trail.Pixellated && !trail.Disabled)
					pixelTrails.Add(trail);

				else if (!trail.Pixellated && !trail.Disabled)
					nonpixelTrails.Add(trail);
			}

			if (primTargetNPC != null && pixelTrails.Count > 0)
			{
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				Main.spriteBatch.Draw(primTargetNPC, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
				spriteBatch.End();
			}

			foreach (PrimTrail trail in nonpixelTrails)
				trail.Draw();
		}

		public void DrawTrailsProj(SpriteBatch spriteBatch, GraphicsDevice gD)
		{
			List<PrimTrail> pixelTrails = new List<PrimTrail>();
			foreach (PrimTrail trail in _trails.ToArray().Where(x => x.DrawType == DrawProjectile))
				if (trail.Pixellated && !trail.Disabled)
					pixelTrails.Add(trail);

			if(pixelTrails.Count > 0)
			{
				RenderTargetBinding[] bindings = gD.GetRenderTargets();

				gD.SetRenderTarget(primTargetProjectile);
				gD.Clear(Color.Transparent);
				spriteBatch.Begin();

				foreach (PrimTrail trail in pixelTrails)
					trail.Draw();

				spriteBatch.End();
				gD.SetRenderTargets(bindings);
			}
		}

		public void DrawTargetProj(SpriteBatch spriteBatch)
		{
			List<PrimTrail> pixelTrails = new List<PrimTrail>();
			List<PrimTrail> nonpixelTrails = new List<PrimTrail>();
			foreach (PrimTrail trail in _trails.ToArray().Where(x => x.DrawType == DrawProjectile))
			{
				if (trail.Pixellated && !trail.Disabled)
					pixelTrails.Add(trail);

				else if (!trail.Pixellated && !trail.Disabled)
					nonpixelTrails.Add(trail);
			}

			if (primTargetProjectile != null && pixelTrails.Count > 0)
			{
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				Main.spriteBatch.Draw(primTargetProjectile, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
				spriteBatch.End();
			}

			foreach (PrimTrail trail in nonpixelTrails)
				trail.Draw();
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