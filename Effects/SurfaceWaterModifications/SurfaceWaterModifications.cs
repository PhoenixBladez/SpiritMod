using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.Graphics.Effects;

namespace SpiritMod.Effects.SurfaceWaterModifications
{
	public class SurfaceWaterModifications
	{
		public static RenderTarget2D transparencyTarget = null;

		public static void Load() => IL.Terraria.Main.DoDraw += AddWaterShader;
		public static void Unload() => IL.Terraria.Main.DoDraw -= AddWaterShader;

		/// <summary>MASSIVE thanks to Starlight River for the base of this IL edit.</summary>
		private static void AddWaterShader(ILContext il)
		{
			var c = new ILCursor(il);

			c.TryGotoNext(n => n.MatchLdfld<Main>("backWaterTarget")); //Back target

			c.TryGotoNext(n => n.MatchCallvirt<SpriteBatch>("Draw"));
			c.Index++;
			ILLabel label = il.DefineLabel(c.Next);

			c.TryGotoPrev(n => n.MatchLdfld<Main>("backWaterTarget"));
			c.Emit(OpCodes.Pop);
			c.Emit(OpCodes.Pop);
			c.Emit(OpCodes.Ldc_I4_0); //Push 0 because this is the back
			c.EmitDelegate<Action<bool>>(NewDraw);
			c.Emit(OpCodes.Br, label);

			c.TryGotoNext(n => n.MatchLdsfld<Main>("waterTarget")); //Front target

			c.TryGotoNext(n => n.MatchCallvirt<SpriteBatch>("Draw"));
			c.Index++;
			ILLabel label2 = il.DefineLabel(c.Next);

			c.TryGotoPrev(n => n.MatchLdsfld<Main>("waterTarget"));
			c.Emit(OpCodes.Pop);
			c.Emit(OpCodes.Ldc_I4_1); //Push 1 since this is the front
			c.EmitDelegate<Action<bool>>(NewDraw);
			c.Emit(OpCodes.Br, label2);
		}

		private static void NewDraw(bool back)
		{
			Main.spriteBatch.End();
			SetShader();

			if (back)
				Main.spriteBatch.Draw(Main.instance.backWaterTarget, Main.sceneBackgroundPos - Main.screenPosition, Color.White);
			else
				Main.spriteBatch.Draw(Main.waterTarget, Main.sceneWaterPos - Main.screenPosition, Color.White);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin();
		}

		private static void SetShader()
		{
			var effect = Filters.Scene["SpiritMod:SurfaceWaterFX"].GetShader().Shader;
			effect.Parameters["transparency"].SetValue(Main.LocalPlayer.ZoneOverworldHeight || Main.LocalPlayer.ZoneSkyHeight ? 0.6f : 0.8f);
			Main.spriteBatch.Begin(default, default, default, default, default, effect, Main.GameViewMatrix.ZoomMatrix);
		}
	}
}
