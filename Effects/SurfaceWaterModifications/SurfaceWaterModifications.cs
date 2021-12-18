using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace SpiritMod.Effects.SurfaceWaterModifications
{
	public class SurfaceWaterModifications
	{
		public static RenderTarget2D transparencyTarget = null;
		public static Effect transparencyEffect = null;

		public static void Load()
		{
			IL.Terraria.Main.DoDraw += AddWaterShader;

			IL.Terraria.Main.DrawTiles += Main_DrawTiles;
			IL.Terraria.Main.DrawBlack += Main_DrawBlack;

			transparencyEffect = ModContent.GetInstance<SpiritMod>().GetEffect("Effects/SurfaceWaterModifications/SurfaceWaterFX");
		}

		public static void Unload()
		{
			IL.Terraria.Main.DoDraw -= AddWaterShader;

			IL.Terraria.Main.DrawTiles -= Main_DrawTiles;
			IL.Terraria.Main.DrawBlack -= Main_DrawBlack;

			transparencyEffect = null;
		}

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
			Main.spriteBatch.Begin(default, default, default, default, default, null, Main.GameViewMatrix.ZoomMatrix);
		}

		private static void SetShader()
		{
			transparencyEffect = ModContent.GetInstance<SpiritMod>().GetEffect("Effects/SurfaceWaterModifications/SurfaceWaterFX");

			transparencyEffect.Parameters["transparency"].SetValue(GetTransparency());
			Main.spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, transparencyEffect, Main.GameViewMatrix.ZoomMatrix);
		}

		private static float GetTransparency()
		{
			bool aboveGround = Main.LocalPlayer.ZoneOverworldHeight || Main.LocalPlayer.ZoneSkyHeight;
			if (Main.LocalPlayer.ZoneBeach)
				return 1f;
			return aboveGround ? 0.90f : 0.5f;
		}

		internal static void ModifyBrightness(ref float scale)
		{
		}

		// below is code for fixing the black tile rendering issue with slopes and transparency for the fake liquid that is drawn
		private static void Main_DrawBlack(ILContext il)
		{
			ILCursor cursor = new ILCursor(il);
			//PrintInstrs(il);

			// get instruction post break
			cursor.TryGotoNext(i => i.MatchLdsfld<Main>("blackTileTexture"));
			cursor.TryGotoPrev(i => i.MatchSub());
			cursor.Index -= 2;
			ILLabel breakLabel = il.DefineLabel(cursor.Next);

			cursor.Goto(0);

			cursor.TryGotoNext(i => i.MatchCall<Lighting>("Brightness"));
			cursor.Index -= 2;

			ILLabel normal = il.DefineLabel(cursor.Next);

			// load tile
			cursor.Emit(OpCodes.Ldloc_S, (byte)13);
			// get wall value
			cursor.Emit(OpCodes.Ldfld, typeof(Tile).GetField("wall", BindingFlags.Public | BindingFlags.Instance));
			// if there's a wall, continue on
			cursor.Emit(OpCodes.Brtrue_S, normal);
			// load tile
			cursor.Emit(OpCodes.Ldloc_S, (byte)13);
			// get slope value
			cursor.Emit(OpCodes.Callvirt, typeof(Tile).GetMethod("slope", BindingFlags.Public | BindingFlags.Instance, Type.DefaultBinder, CallingConventions.HasThis, Type.EmptyTypes, null));
			// if there's no slope, continue on
			cursor.Emit(OpCodes.Brfalse_S, normal);
			// load tile
			cursor.Emit(OpCodes.Ldloc_S, (byte)13);
			// get halfBrick value
			cursor.Emit(OpCodes.Callvirt, typeof(Tile).GetMethod("halfBrick", BindingFlags.Public | BindingFlags.Instance, Type.DefaultBinder, CallingConventions.HasThis, Type.EmptyTypes, null));
			// if there's halfbrick, continue on
			cursor.Emit(OpCodes.Brtrue_S, normal);

			// get x and y
			cursor.Emit(OpCodes.Ldloc_S, (byte)11);
			cursor.Emit(OpCodes.Ldloc_S, (byte)9);
			// get brightness
			cursor.Emit(OpCodes.Call, typeof(Lighting).GetMethod("Brightness", BindingFlags.Public | BindingFlags.Static));
			cursor.Emit(OpCodes.Ldc_R4, 0f);
			// if brightness is 0, continue on
			cursor.Emit(OpCodes.Beq_S, normal);

			cursor.Emit(OpCodes.Br, breakLabel);
		}

		private static void Main_DrawTiles(ILContext il)
		{
			ILCursor cursor = new ILCursor(il);
			//PrintInstrs(il);

			cursor.TryGotoNext(i => i.MatchLdsfld<Main>(nameof(Main.liquidTexture)));
			cursor.TryGotoNext(i => i.MatchCallvirt<SpriteBatch>("Draw"));

			// remove spritebatch draw
			cursor.Index++;
			ILLabel postDraw = il.DefineLabel(cursor.Next);

			cursor.Index--;
			// emit our custom code
			for (int i = 0; i < 10; i++)
			{
				cursor.Emit(OpCodes.Pop);
			}
			cursor.Emit(OpCodes.Ldloc_S, (byte)16); // x
			cursor.Emit(OpCodes.Ldloc_S, (byte)15); // y
			cursor.Emit(OpCodes.Ldloc_S, (byte)151); // num116 (water style being used)
			cursor.Emit(OpCodes.Ldloc_S, (byte)147); // flag7
			cursor.Emit(OpCodes.Ldloc_S, (byte)148); // flag8
			cursor.Emit(OpCodes.Ldloc_S, (byte)149); // flag9
			cursor.Emit(OpCodes.Ldloc_S, (byte)150); // flag10
			cursor.Emit(OpCodes.Ldloc_S, (byte)146); // num115 (some liquid amount thing)
			cursor.EmitDelegate<Action<int, int, int, bool, bool, bool, bool, int>>(SlopeHalfBrickLiquidReplacement);

			cursor.Emit(OpCodes.Br, postDraw);

			//cursor.Emit(OpCodes.Ldc_R4, 0.65f);
			//cursor.Emit(OpCodes.Stloc_S, (byte)159);
		}

		private static void SlopeHalfBrickLiquidReplacement(int x, int y, int style, bool flag7, bool flag8, bool flag9, bool flag10, int num115)
		{
			CodeFromSource(x, y, style, flag7, flag8, flag9, flag10, num115, out Vector2 rawPosition, out Rectangle rectangle4, out float gameAlpha);

			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
				zero = Vector2.Zero;

			Vector2 drawPosition = rawPosition - Main.screenPosition + zero;

			Vector2 tl = Main.sceneBackgroundPos;
			Vector2 tile = new Vector2(x * 16f, y * 16f);
			Vector2 pos = tile - tl;
			Rectangle space = new Rectangle((int)pos.X, (int)pos.Y, 16, 16);

			// if the tile above doesn't have full liquid, use the left or right one
			if (Framing.GetTileSafely(x - 1, y).liquid > 0) space.X -= 16;
			else if (Framing.GetTileSafely(x + 1, y).liquid > 0) space.X += 16;
			else if (Framing.GetTileSafely(x, y - 1).liquid < 255) space.Y -= 16;

			Main.spriteBatch.End();

			transparencyEffect.Parameters["transparency"].SetValue(GetTransparency());

			Main.spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, transparencyEffect);

			Main.spriteBatch.Draw(Main.instance.backWaterTarget, drawPosition, space, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin();
			//Main.spriteBatch.Draw(liquidTexture, vector248, rectangle4, color * gameAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}

		private static void CodeFromSource(int x, int y, int style, bool flag7, bool flag8, bool flag9, bool flag10, int num115, out Vector2 vectorPosition, out Rectangle rectangle4, out float gameAlpha)
		{
			Tile tile = Framing.GetTileSafely(x, y);
			Tile tile1 = Framing.GetTileSafely(x + 1, y);
			Tile tile2 = Framing.GetTileSafely(x - 1, y);
			Tile tile3 = Framing.GetTileSafely(x, y - 1);
			Tile tile4 = Framing.GetTileSafely(x, y + 1);

			vectorPosition = new Vector2(x * 16, y * 16);
			rectangle4 = new Rectangle(0, 4, 16, 16);
			if (flag10 && flag7 | flag8)
			{
				flag7 = true;
				flag8 = true;
			}
			if ((!flag9 || !flag7 && !flag8) && (!flag10 || !flag9))
			{
				if (flag9)
				{
					rectangle4 = new Rectangle(0, 4, 16, 4);
					if (tile.halfBrick() || tile.slope() != 0)
						rectangle4 = new Rectangle(0, 4, 16, 12);
				}
				else if (!flag10 || flag7 || flag8)
				{
					float single1 = (256 - num115);
					single1 /= 32f;
					int num118 = 4;

					if (tile3.liquid == 0 && !WorldGen.SolidTile(x, y - 1))
						num118 = 0;

					if (flag7 & flag8 || tile.halfBrick() || tile.slope() != 0)
					{
						vectorPosition = new Vector2(x * 16, y * 16 + (int)single1 * 2);
						rectangle4 = new Rectangle(0, num118, 16, 16 - (int)single1 * 2);
					}
					else if (!flag7)
					{
						vectorPosition = new Vector2(x * 16 + 12, y * 16 + (int)single1 * 2);
						rectangle4 = new Rectangle(0, num118, 4, 16 - (int)single1 * 2);
					}
					else
					{
						vectorPosition = new Vector2(x * 16, y * 16 + (int)single1 * 2);
						rectangle4 = new Rectangle(0, num118, 4, 16 - (int)single1 * 2);
					}
				}
				else
				{
					vectorPosition = new Vector2(x * 16, y * 16 + 12);
					rectangle4 = new Rectangle(0, 4, 16, 4);
				}
			}
			gameAlpha = 0.5f;
			if (style == 1)
			{
				gameAlpha = 1f;
			}
			else if (style == 11)
			{
				gameAlpha *= 1.7f;
				if (gameAlpha > 1f)
				{
					gameAlpha = 1f;
				}
			}
			if (y < Main.worldSurface || gameAlpha > 1f)
			{
				gameAlpha = 1f;
				if (tile3.wall > 0 || tile2.wall > 0 || tile1.wall > 0 || tile4.wall > 0)
					gameAlpha = 0.65f;
				if (tile.wall > 0)
					gameAlpha = 0.5f;
			}
			if (tile.halfBrick() && tile3.liquid > 0 && tile.wall > 0)
				gameAlpha = 0f;
		}

		private static void PrintInstrs(ILContext il)
		{
			foreach (var instr in il.Instrs)
			{
				string s = "";
				try
				{
					s = instr.ToString();
				}
				catch
				{
					if (instr.Operand is ILLabel label && label.Target != null)
						SpiritMod.Instance.Logger.Debug(instr.OpCode + " IL_" + label.Target.Offset.ToString("x4"));
					else
						SpiritMod.Instance.Logger.Debug(instr.OpCode);
					continue;
				}
				SpiritMod.Instance.Logger.Debug(s);
			}
		}
	}
}
