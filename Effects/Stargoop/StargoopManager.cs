using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Effects.Stargoop
{
	public class StargoopManager
	{
		private readonly Color EnemyBorderColor = new Color(255, 171, 51);
		private readonly Color FriendlyBorderColor = new Color(242, 240, 134);
		private readonly Color NebulaBorderColor = new Color(255, 102, 200);
		public StargoopLayer FriendlyLayer { get; set; }
		public StargoopLayer EnemyLayer { get; set; }
		public StargoopLayer NebulaLayer { get; set; }
		public RenderTarget2D TmpTarget { get; protected set; }
		public Texture2D Galaxy0 { get; set; }
		public Texture2D Galaxy1 { get; set; }
		public Texture2D Galaxy2 { get; set; }
		public Texture2D Mask { get; set; }

		public Effect metaballColorCode;
		public Effect metaballEdgeDetection;
		public Effect borderNoise;
		public Effect galaxyParallax;

		private bool _loaded = false;

		public void Initialize(GraphicsDevice graphicsDevice)
		{
			UpdateWindowSize(graphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2);
		}

		public void LoadContent()
		{
			Mask = ModContent.Request<Texture2D>("SpiritMod/Effects/Masks/Mask", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			Galaxy0 = ModContent.Request<Texture2D>("SpiritMod/Effects/Masks/Galaxy0", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Galaxy1 = ModContent.Request<Texture2D>("SpiritMod/Effects/Masks/Galaxy1", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Galaxy2 = ModContent.Request<Texture2D>("SpiritMod/Effects/Masks/Galaxy2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			metaballColorCode = ModContent.Request<Effect>("SpiritMod/Effects/MetaballColorCode", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			metaballEdgeDetection = ModContent.Request<Effect>("SpiritMod/Effects/MetaballEdgeDetection", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			borderNoise = ModContent.Request<Effect>("SpiritMod/Effects/BorderNoise", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			galaxyParallax = ModContent.Request<Effect>("SpiritMod/Effects/GalaxyParallax", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			FriendlyLayer = new StargoopLayer(FriendlyBorderColor, Galaxy0, Galaxy1, Galaxy2);
			EnemyLayer = new StargoopLayer(EnemyBorderColor, Galaxy0, Galaxy1, Galaxy2);
			NebulaLayer = new StargoopLayer(NebulaBorderColor, ModContent.Request<Texture2D>("SpiritMod/Effects/Masks/Nebula0", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
															  ModContent.Request<Texture2D>("SpiritMod/Effects/Masks/Nebula1", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
															  ModContent.Request<Texture2D>("SpiritMod/Effects/Masks/Nebula2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);

			_loaded = true;
		}

		public void UpdateWindowSize(GraphicsDevice graphicsDevice, int width, int height)
		{
			return;
			if (!_loaded)
				return;

			FriendlyLayer.UpdateWindowSize(graphicsDevice, width, height);
			EnemyLayer.UpdateWindowSize(graphicsDevice, width, height);
			NebulaLayer.UpdateWindowSize(graphicsDevice, width, height);
			Main.QueueMainThreadAction(() => TmpTarget = new RenderTarget2D(graphicsDevice, width, height));
		}

		public void DrawToTarget(SpriteBatch sB, GraphicsDevice graphicsDevice)
		{
			return;
			var prevTarget = graphicsDevice.GetRenderTargets();
			
			if (FriendlyLayer.Metaballs.Count > 0 || FriendlyLayer.Sprites.Count > 0)
				FriendlyLayer.DrawMetaballTarget(sB, graphicsDevice);

			if (EnemyLayer.Metaballs.Count > 0 || EnemyLayer.Sprites.Count > 0)
				EnemyLayer.DrawMetaballTarget(sB, graphicsDevice);

			if (NebulaLayer.Metaballs.Count > 0 || NebulaLayer.Sprites.Count > 0)
				NebulaLayer.DrawMetaballTarget(sB, graphicsDevice);

			graphicsDevice.SetRenderTargets(prevTarget);
		}

		public void DrawEnemyLayer(SpriteBatch sB)
		{
			if (EnemyLayer.Metaballs.Count > 0 || EnemyLayer.Sprites.Count > 0)
			{
				sB.End();
				EnemyLayer.DrawLayer(sB);
				sB.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			}
		}
		public void DrawFriendlyLayer(SpriteBatch sB)
		{
			if (FriendlyLayer.Metaballs.Count > 0 || FriendlyLayer.Sprites.Count > 0)
				FriendlyLayer.DrawLayer(sB);
		}

		public void DrawNebulaLayer(SpriteBatch sB)
		{
			if (NebulaLayer.Metaballs.Count > 0 || NebulaLayer.Sprites.Count > 0)
			{
				sB.End();
				NebulaLayer.DrawLayer(sB);
				sB.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			}
		}
	}

	public interface IGalaxySprite
	{
		/// <summary>
		/// Draw parts of sprite that are color coded, and should be drawn with the metaball layer. Galaxy Parallax shader is active.
		/// </summary>
		/// <param name="sB"></param>
		void DrawGalaxyMappedSprite(SpriteBatch sB);
	}

	public interface IMetaball
	{
		/// <summary>
		/// Draws metaball masks on the metaball target. The borded noise shader is active.
		/// </summary>
		/// <param name="sB">SpriteBatch to draw to</param>
		void DrawOnMetaballLayer(SpriteBatch sB);
	}
}
