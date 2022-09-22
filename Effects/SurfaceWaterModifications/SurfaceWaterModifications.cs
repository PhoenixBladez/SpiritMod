using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using SpiritMod.Mechanics.OceanWavesSystem;
using SpiritMod.Utilities;
using SpiritMod.Utilities.Helpers;
using System;
using System.Diagnostics;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Liquid;
using Terraria.GameContent.Shaders;
using Terraria.Graphics;
using Terraria.Graphics.Light;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Effects.SurfaceWaterModifications
{
	public class SurfaceWaterModifications
	{
		public static RenderTarget2D transparencyTarget = null;
		public static Effect transparencyEffect = null;
		public static Texture2D rippleTex = null;

		public static int leftOceanHeight = 0;
		public static int rightOceanHeight = 0;

		public static ILog Logger => ModContent.GetInstance<SpiritMod>().Logger;

		public static void Load()
		{
			if (ModContent.GetInstance<SpiritClientConfig>().SurfaceWaterTransparency)
			{
				IL.Terraria.Main.DoDraw += AddWaterShader; //Transparency shader
														   //IL.Terraria.Main.DrawTiles += Main_DrawTiles; //Liquid slope fix (tentative)
														   //IL.Terraria.Main.DrawBlack += Main_DrawBlack; //^^
				On.Terraria.Main.DrawBlack += Main_DrawBlack;
				On.Terraria.GameContent.Drawing.TileDrawing.DrawPartialLiquid += TileDrawing_DrawPartialLiquid;
			}

			IL.Terraria.GameContent.Shaders.WaterShaderData.QueueRipple_Vector2_Color_Vector2_RippleShape_float += IncreaseRippleSize; //Makes ripple bigger
			IL.Terraria.GameContent.Shaders.WaterShaderData.DrawWaves += WaterShaderData_DrawWaves;

			if (!Main.dedServ)
			{
				transparencyEffect = ModContent.Request<Effect>("SpiritMod/Effects/SurfaceWaterModifications/SurfaceWaterFX", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				rippleTex = ModContent.Request<Texture2D>("Terraria/Images/Misc/Ripples", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			}
		}

		private static void Main_DrawBlack(On.Terraria.Main.orig_DrawBlack orig, Main self, bool force)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			Vector2 value = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
			int num = (Main.tileColor.R + Main.tileColor.G + Main.tileColor.B) / 3;
			float num2 = (float)((double)num * 0.4) / 255f;

			if (Lighting.Mode == LightMode.Retro)
			{
				num2 = (float)(Main.tileColor.R - 55) / 255f;
				if (num2 < 0f)
					num2 = 0f;
			}
			else if (Lighting.Mode == LightMode.Trippy)
			{
				num2 = (float)(num - 55) / 255f;
				if (num2 < 0f)
					num2 = 0f;
			}

			Point screenOverdrawOffset = Main.GetScreenOverdrawOffset();
			Point point = new Point(-Main.offScreenRange / 16 + screenOverdrawOffset.X, -Main.offScreenRange / 16 + screenOverdrawOffset.Y);
			int num3 = (int)((Main.screenPosition.X - value.X) / 16f - 1f) + point.X;
			int num4 = (int)((Main.screenPosition.X + (float)Main.screenWidth + value.X) / 16f) + 2 - point.X;
			int num5 = (int)((Main.screenPosition.Y - value.Y) / 16f - 1f) + point.Y;
			int num6 = (int)((Main.screenPosition.Y + (float)Main.screenHeight + value.Y) / 16f) + 5 - point.Y;

			if (num3 < 0)
				num3 = point.X;

			if (num4 > Main.maxTilesX)
				num4 = Main.maxTilesX - point.X;

			if (num5 < 0)
				num5 = point.Y;

			if (num6 > Main.maxTilesY)
				num6 = Main.maxTilesY - point.Y;

			if (!force)
			{
				if (num5 < Main.maxTilesY / 2)
				{
					num6 = Math.Min(num6, (int)Main.worldSurface + 1);
					num5 = Math.Min(num5, (int)Main.worldSurface + 1);
				}
				else
				{
					num6 = Math.Max(num6, Main.UnderworldLayer);
					num5 = Math.Max(num5, Main.UnderworldLayer);
				}
			}
			for (int i = num5; i < num6; i++)
			{
				bool flag = i >= Main.UnderworldLayer;
				if (flag)
					num2 = 0.2f;

				for (int j = num3; j < num4; j++)
				{
					int num7 = j;
					for (; j < num4; j++)
					{
						if (!WorldGen.InWorld(j, i))
							return;

						if (Main.tile[j, i] == null)
							Main.tile[j, i].ClearEverything();

						Tile tile = Main.tile[j, i];
						float num8 = Lighting.Brightness(j, i);
						num8 = (float)Math.Floor(num8 * 255f) / 255f;
						byte b = tile.LiquidAmount;

						if (!(num8 <= num2) || ((flag || b >= 250) && !WorldGen.SolidTile(tile) && (b < 200 || num8 != 0f)) || (WallID.Sets.Transparent[tile.WallType] && (!Main.tile[j, i].HasTile || !Main.tileBlockLight[(int)tile.BlockType])) || Framing.GetTileSafely(j, i - 1).LiquidAmount > 0 || Framing.GetTileSafely(j, i + 1).LiquidAmount > 0 || Framing.GetTileSafely(j + 1, i).LiquidAmount > 0 || Framing.GetTileSafely(j - 1, i).LiquidAmount > 0 || (tile.IsHalfBlock && Framing.GetTileSafely(j - 1, i).LiquidAmount > 0 && Lighting.Brightness(j, i) > 0f) || (!Main.drawToScreen && LiquidRenderer.Instance.HasFullWater(j, i) && tile.WallType == 0 && !tile.IsHalfBlock && !((double)i <= Main.worldSurface)))
							break;
					}

					if (j - num7 > 0)
						Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Vector2(num7 << 4, i << 4) - Main.screenPosition + value, new Rectangle(0, 0, j - num7 << 4, 16), Color.Black);
				}
			}
			TimeLogger.DrawTime(5, stopwatch.Elapsed.TotalMilliseconds);
		}

		private static unsafe void TileDrawing_DrawPartialLiquid(On.Terraria.GameContent.Drawing.TileDrawing.orig_DrawPartialLiquid orig, Terraria.GameContent.Drawing.TileDrawing self, Tile tileCache, Vector2 position, Rectangle liquidSize, int liquidType, Color aColor)
		{
			Vector2 rectangle = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
			Vector2 archivePos = position;

			position += Main.screenPosition;
			position -= rectangle;

			Vector2 drawOffset = (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange)) - Main.screenPosition;

			int i = (int)((position.X) / 16f);
			int j = (int)((position.Y) / 16f);

			int slope = (int)tileCache.Slope;

			if (!TileID.Sets.BlocksWaterDrawingBehindSelf[(int)tileCache.BlockType] || slope == 0)
			{
				if (IsWaterTransparent())
					Main.tileBatch.Begin(default, BlendState.NonPremultiplied, SamplerState.PointClamp, default, Main.Rasterizer, transparencyEffect);
				else
					Main.tileBatch.Begin();

				Main.DrawTileInWater(drawOffset, (int)((position.X) / 16f), (int)((position.Y) / 16f));

				Lighting.GetCornerColors(i, j, out VertexColors vertices);

				float opacity = Main.tile[i, j].LiquidType == 0 ? 0.65f : 0.95f;

				Color bottomLeft = vertices.BottomLeftColor;
				Color bottomRight = vertices.BottomRightColor;
				Color topLeft = vertices.TopLeftColor;
				Color topRight = vertices.TopRightColor;

				if (IsWaterTransparent())
				{
					vertices.BottomLeftColor = aColor * Main.liquidAlpha[Main.waterStyle] * (bottomLeft.A / 255f);
					vertices.BottomRightColor = aColor * Main.liquidAlpha[Main.waterStyle] * (bottomRight.A / 255f);
					vertices.TopLeftColor = aColor * Main.liquidAlpha[Main.waterStyle] * (topLeft.A / 255f);
					vertices.TopRightColor = aColor * Main.liquidAlpha[Main.waterStyle] * (topRight.A / 255f);

					//vertices.BottomLeftColor.A = 255;
				}
				else
				{
					vertices.BottomLeftColor = aColor * Main.liquidAlpha[Main.waterStyle] * (bottomLeft.A / 255f);
					vertices.BottomRightColor = aColor * Main.liquidAlpha[Main.waterStyle] * (bottomRight.A / 255f);
					vertices.TopLeftColor = aColor * Main.liquidAlpha[Main.waterStyle] * (topLeft.A / 255f);
					vertices.TopRightColor = aColor * Main.liquidAlpha[Main.waterStyle] * (topRight.A / 255f);
				}

				Main.DrawTileInWater(drawOffset, i, j);

				if (Main.tile[i, j].IsHalfBlock)
					Main.tileBatch.Draw(TextureAssets.Liquid[liquidType].Value, archivePos + new Vector2(0, 8), liquidSize, vertices, default(Vector2), 1f, SpriteEffects.None);
				else
					Main.tileBatch.Draw(TextureAssets.Liquid[liquidType].Value, archivePos, liquidSize, vertices, default(Vector2), 1f, SpriteEffects.None);

				Main.tileBatch.End();

				return;
			}

			liquidSize.X += 18 * (slope - 1);

			if (tileCache.Slope == (SlopeType)1 || tileCache.Slope == (SlopeType)2 || tileCache.Slope == (SlopeType)3 || tileCache.Slope == (SlopeType)4)
			{
				Main.spriteBatch.Draw(TextureAssets.LiquidSlope[liquidType].Value, archivePos, liquidSize, aColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}


		public static void Unload()
		{
			transparencyTarget = null;
			transparencyEffect = null;
			rippleTex = null;
		}

		private static void WaterShaderData_DrawWaves(ILContext il)
		{
			var c = new ILCursor(il);

			if (!c.TryGotoNext(x => x.MatchLdfld<WaterShaderData>("_useRippleWaves")))
			{
				Logger.Debug("FAILED _useRippleWaves GOTO [SpiritMod.WaterShaderData.DrawWaves]");
				return;
			}

			if (!c.TryGotoNext(x => x.MatchCallvirt(typeof(TileBatch).GetMethod("End"))))
			{
				Logger.Debug("FAILED _useRippleWaves GOTO [SpiritMod.WaterShaderData.DrawWaves]");
				return;
			}

			c.Emit(OpCodes.Ldloc_2);
			c.EmitDelegate(DoWaves);
		}

		private static void DoWaves(Vector2 offset)
		{
			bool validPlayer = false;
			(bool, bool) sides = (false, false);
			for (int i = 0; i < Main.maxPlayers; ++i)
			{
				Player p = Main.player[i];
				if (p.active && p.ZoneBeach)
				{
					validPlayer = true;
					CheckBeachHeightsSet();

					if (p.position.X / 16f < Main.maxTilesX / 2)
						sides.Item1 = true;
					else
						sides.Item2 = true;

					if (sides.Item1 && sides.Item2)
						break;
				}
			}

			float Speed() => 0.75f + Main.rand.NextFloat(0, 0.25f); 

			if (validPlayer) //Draw here to draw stuff only when there's a player at a beach
			{
				if (sides.Item1 && Main.GameUpdateCount % 50 == 0)
					OceanWaveManager.AddWave(new OceanWaveManager.Wave(new Vector2(600, leftOceanHeight), new Vector2(10, 30), Main.rand.NextFloat(0.6f, 1f), Speed()));
				if (sides.Item2 && Main.GameUpdateCount % 50 == 0)
					OceanWaveManager.AddWave(new OceanWaveManager.Wave(new Vector2(Main.maxTilesX * 16 - 550, rightOceanHeight), new Vector2(10, 30), Main.rand.NextFloat(0.6f, 1f), Speed()));
			}

			OceanWaveManager.UpdateWaves(sides.Item1, sides.Item2, offset);
		}

		private static void CheckBeachHeightsSet()
		{
			int maxYAllowed = Main.maxTilesY - 200;
			if (leftOceanHeight == 0 || leftOceanHeight / 16f > Main.worldSurface)
			{
				var start = new Point(60, (int)(Main.maxTilesY * 0.35f / 16f));
				while (Framing.GetTileSafely(start.X, start.Y).LiquidAmount < 255)
				{
					start.Y++;
					if (start.Y > maxYAllowed) break;
				}

				leftOceanHeight = start.Y * 16 - (int)(18 * (Framing.GetTileSafely(start.X, start.Y).LiquidAmount / 255f));
			}

			if (rightOceanHeight == 0 || rightOceanHeight / 16f > Main.worldSurface)
			{
				var start = new Point(Main.maxTilesX - 60, (int)(Main.maxTilesY * 0.35f / 16f));
				while (Framing.GetTileSafely(start.X, start.Y).LiquidAmount < 255)
				{
					start.Y++;
					if (start.Y > maxYAllowed) break;
				}

				rightOceanHeight = start.Y * 16 - (int)(18 * (Framing.GetTileSafely(start.X, start.Y).LiquidAmount / 255f));
			}
		}

		private static void IncreaseRippleSize(ILContext il)
		{
			var c = new ILCursor(il);

			c.Emit(OpCodes.Ldarg_3);
			c.Emit(OpCodes.Ldc_R4, 2f);
			var vec2Mul = typeof(Vector2).GetMethod("op_Multiply", new Type[2] { typeof(Vector2), typeof(float) }, new ParameterModifier[] { new ParameterModifier(3) });
			c.Emit(OpCodes.Call, vec2Mul);
			c.Emit(OpCodes.Starg, 3);
		}

		/// <summary>MASSIVE thanks to Starlight River for the base of this IL edit.</summary>
		private static void AddWaterShader(ILContext il)
		{
			var c = new ILCursor(il);

			//ILHelper.CompleteLog(c, true);

			//BACK TARGET
			c.TryGotoNext(n => n.MatchLdfld<Main>("backWaterTarget"));

			c.TryGotoNext(n => n.MatchCallvirt<SpriteBatch>("Draw"));
			c.Index++;
			ILLabel postDrawLabel = c.MarkLabel();

			c.TryGotoPrev(MoveType.Before, n => n.MatchLdfld<Main>("backWaterTarget"));
			c.TryGotoNext(MoveType.Before, n => n.MatchLdsfld<Main>("sceneBackgroundPos"));

			ILLabel exitLabel = c.MarkLabel();

			c.TryGotoPrev(MoveType.Before, n => n.MatchLdfld<Main>("backWaterTarget"));
			c.Index++;

			c.EmitDelegate(IsWaterTransparent);
			c.Emit(OpCodes.Brfalse, exitLabel);
			c.Emit(OpCodes.Nop);

			c.Index--;
			c.Emit(OpCodes.Pop);
			c.Emit(OpCodes.Pop);
			c.Emit(OpCodes.Ldc_I4_0); //Push 0 because this is the back
			int v = c.EmitDelegate(NewDraw);
			c.Emit(OpCodes.Br, postDrawLabel);

			//FRONT TARGET
			c.TryGotoNext(n => n.MatchLdsfld<Main>("waterTarget"));

			c.TryGotoNext(n => n.MatchCallvirt<SpriteBatch>("Draw"));
			c.Index++;
			ILLabel label2 = c.MarkLabel();

			c.TryGotoPrev(n => n.MatchLdsfld<Main>("waterTarget"));
			c.TryGotoNext(MoveType.Before, n => n.MatchLdsfld<Main>("sceneWaterPos"));

			ILLabel exitLabelFront = c.MarkLabel();

			c.TryGotoPrev(n => n.MatchLdsfld<Main>("waterTarget"));
			c.Index++;

			c.EmitDelegate(IsWaterTransparent);
			c.Emit(OpCodes.Brfalse, exitLabelFront);
			c.Emit(OpCodes.Nop);

			c.Index--;
			c.Emit(OpCodes.Pop);
			c.Emit(OpCodes.Pop);
			c.Emit(OpCodes.Ldc_I4_1); //Push 1 since this is the front
			c.EmitDelegate(NewDraw);
			c.Emit(OpCodes.Br, label2);

			//ILHelper.CompleteLog(c);
		}

		private static bool IsWaterTransparent() => (Lighting.Mode == LightMode.Color || Lighting.Mode == LightMode.White) && Main.LocalPlayer.ZoneBeach && !Main.bloodMoon && !Main.LocalPlayer.ZoneSkyHeight;

		private static void NewDraw(bool back)
		{
			Main.spriteBatch.End();

			SetShader(back);

			if (back)
			{
				Main.spriteBatch.Draw(Main.instance.backWaterTarget, Main.sceneBackgroundPos - Main.screenPosition, Color.White);
				Terraria.Graphics.Effects.Overlays.Scene.Draw(Main.spriteBatch, Terraria.Graphics.Effects.RenderLayers.BackgroundWater);
			}
			else
			{
				Main.spriteBatch.Draw(Main.waterTarget, Main.sceneWaterPos - Main.screenPosition, Color.White * Main.liquidAlpha[Main.waterStyle]);
				Terraria.Graphics.Effects.Overlays.Scene.Draw(Main.spriteBatch, Terraria.Graphics.Effects.RenderLayers.ForegroundWater);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(default, default, default, default, default, null, Main.GameViewMatrix.ZoomMatrix);
		}

		/// <summary>Just a little test I did. Don't mind this. :)</summary>
		//private static void DrawReflectedPlayer()
		//{
		//	Player plr = Main.LocalPlayer;
		//	plr.direction = plr.direction == -1 ? 1 : -1;

		//	Main.instance.DrawPlayer(plr, plr.position, MathHelper.Pi, new Vector2(plr.width / 2f, plr.height), 0);

		//	plr.direction = plr.direction == -1 ? 1 : -1;
		//}

		private static void SetShader(bool back)
		{
			transparencyEffect = ModContent.Request<Effect>("SpiritMod/Effects/SurfaceWaterModifications/SurfaceWaterFX", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			transparencyEffect.Parameters["transparency"].SetValue(GetTransparency());

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, transparencyEffect, Main.GameViewMatrix.ZoomMatrix);
		}

		private static float GetTransparency()
		{
			bool aboveGround = Main.LocalPlayer.Center.Y / 16f < Main.worldSurface;
			if (aboveGround && Main.LocalPlayer.ZoneBeach && ModContent.GetInstance<SpiritClientConfig>().SurfaceWaterTransparency)
				return 1.25f;

			return aboveGround ? 1f : 0.5f;
		}

		// below is code for fixing the black tile rendering issue with slopes and transparency for the fake liquid that is drawn
		//private static void Main_DrawBlack(ILContext il)
		//{
		//	ILCursor cursor = new ILCursor(il);

		//	ILHelper.CompleteLog(cursor, true);

		//	// get instruction post break
		//	cursor.TryGotoNext(i => i.MatchLdsfld<Main>("blackTileTexture"));
		//	cursor.TryGotoPrev(i => i.MatchSub());
		//	cursor.Index -= 2;
		//	ILLabel breakLabel = il.DefineLabel(cursor.Next);

		//	cursor.Goto(0);

		//	cursor.TryGotoNext(i => i.MatchCall<Lighting>("Brightness"));
		//	cursor.Index -= 2;

		//	ILLabel normal = il.DefineLabel(cursor.Next);

		//	// load tile
		//	//cursor.Emit(OpCodes.Ldloc_S, (byte)14);
		//	//// load x, y
		//	////cursor.Emit(OpCodes.Ldloc_S, (byte)12);
		//	////cursor.Emit(OpCodes.Ldloc_S, (byte)10);
		//	//// check all conditions on tile
		//	//cursor.EmitDelegate((Tile tile, int x, int y) =>
		//	//{
		//	//	if (tile.WallType > 0 || tile.Slope == Terraria.ID.SlopeType.Solid || tile.IsHalfBlock)
		//	//		return false;
		//	//	//if (Lighting.Brightness(x, y) == 0f)
		//	//	//	return false;
		//	//	return true;
		//	//});
		//	//cursor.Emit(OpCodes.Brtrue_S, normal);
		//	cursor.Emit(OpCodes.Br, breakLabel);

		//	ILHelper.CompleteLog(cursor);

		//	// get wall value
		//	//cursor.Emit(OpCodes.Call, typeof(Tile).GetMethod("get_WallType", BindingFlags.Public | BindingFlags.Instance));
		//	//// if there's a wall, continue on
		//	//cursor.Emit(OpCodes.Brtrue_S, normal);

		//	//// load tile
		//	//cursor.Emit(OpCodes.Ldloc_S, (byte)13);
		//	//// get slope value
		//	//cursor.Emit(OpCodes.Call, typeof(Tile).GetMethod("get_Slope", BindingFlags.Public | BindingFlags.Instance));
		//	//// if there's no slope, continue on
		//	//cursor.Emit(OpCodes.Brfalse_S, normal);

		//	//// load tile
		//	//cursor.Emit(OpCodes.Ldloc_S, (byte)13);
		//	//// get halfBrick value
		//	//cursor.Emit(OpCodes.Call, typeof(Tile).GetMethod("get_IsHalfBlock", BindingFlags.Public | BindingFlags.Instance));
		//	//// if there's halfbrick, continue on
		//	//cursor.Emit(OpCodes.Brtrue_S, normal);

		//	//// get x and y
		//	//cursor.Emit(OpCodes.Ldloc_S, (byte)11);
		//	//cursor.Emit(OpCodes.Ldloc_S, (byte)9); 
		//	//// get brightness
		//	//cursor.Emit(OpCodes.Call, typeof(Lighting).GetMethod("Brightness", BindingFlags.Public | BindingFlags.Static));
		//	//cursor.Emit(OpCodes.Ldc_R4, 0f);
		//	//// if brightness is 0, continue on
		//	//cursor.Emit(OpCodes.Beq_S, normal);

		//}

		//private static void Main_DrawTiles(ILContext il)
		//{
		//	ILCursor cursor = new ILCursor(il);

		//	cursor.TryGotoNext(i => i.MatchLdsfld<Main>(nameof(TextureAssets.Liquid)));
		//	cursor.TryGotoNext(i => i.MatchCallvirt<SpriteBatch>("Draw"));

		//	// remove spritebatch draw
		//	cursor.Index++;
		//	ILLabel postDraw = il.DefineLabel(cursor.Next);

		//	cursor.Index--;
		//	// emit our custom code
		//	for (int i = 0; i < 10; i++)
		//		cursor.Emit(OpCodes.Pop);
		//	cursor.Emit(OpCodes.Ldloc_S, (byte)16); // x
		//	cursor.Emit(OpCodes.Ldloc_S, (byte)15); // y
		//	cursor.Emit(OpCodes.Ldloc_S, (byte)151); // num116 (water style being used)
		//	cursor.Emit(OpCodes.Ldloc_S, (byte)147); // flag7
		//	cursor.Emit(OpCodes.Ldloc_S, (byte)148); // flag8
		//	cursor.Emit(OpCodes.Ldloc_S, (byte)149); // flag9
		//	cursor.Emit(OpCodes.Ldloc_S, (byte)150); // flag10
		//	cursor.Emit(OpCodes.Ldloc_S, (byte)146); // num115 (some liquid amount thing)
		//	cursor.EmitDelegate(SlopeHalfBrickLiquidReplacement);

		//	cursor.Emit(OpCodes.Br, postDraw);
		//}

		//private static void SlopeHalfBrickLiquidReplacement(int x, int y, int style, bool flag7, bool flag8, bool flag9, bool flag10, int num115)
		//{
		//	CodeFromSource(x, y, style, flag7, flag8, flag9, flag10, num115, out Vector2 rawPosition, out Rectangle rectangle4, out float gameAlpha);

		//	Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		//	if (Main.drawToScreen)
		//		zero = Vector2.Zero;

		//	Vector2 drawPosition = rawPosition - Main.screenPosition + zero;

		//	Vector2 tl = Main.sceneBackgroundPos;
		//	Vector2 tile = new Vector2(x * 16f, y * 16f);
		//	Vector2 pos = tile - tl;
		//	Rectangle space = new Rectangle((int)pos.X, (int)pos.Y, 16, 16);

		//	// if the tile above doesn't have full liquid, use the left or right one
		//	if (Framing.GetTileSafely(x - 1, y).LiquidAmount > 0) space.X -= 16;
		//	else if (Framing.GetTileSafely(x + 1, y).LiquidAmount > 0) space.X += 16;
		//	else if (Framing.GetTileSafely(x, y - 1).LiquidAmount < 255) space.Y -= 16;

		//	Main.spriteBatch.End();

		//	transparencyEffect.Parameters["transparency"].SetValue(GetTransparency());

		//	Main.spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, transparencyEffect);

		//	Main.spriteBatch.Draw(Main.instance.backWaterTarget, drawPosition, space, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

		//	Main.spriteBatch.End();
		//	Main.spriteBatch.Begin();
		//	//Main.spriteBatch.Draw(liquidTexture, vector248, rectangle4, color * gameAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		//}

		//private static void CodeFromSource(int x, int y, int style, bool flag7, bool flag8, bool flag9, bool flag10, int num115, out Vector2 vectorPosition, out Rectangle rectangle4, out float gameAlpha)
		//{
		//	Tile tile = Framing.GetTileSafely(x, y);
		//	Tile tile1 = Framing.GetTileSafely(x + 1, y);
		//	Tile tile2 = Framing.GetTileSafely(x - 1, y);
		//	Tile tile3 = Framing.GetTileSafely(x, y - 1);
		//	Tile tile4 = Framing.GetTileSafely(x, y + 1);

		//	vectorPosition = new Vector2(x * 16, y * 16);
		//	rectangle4 = new Rectangle(0, 4, 16, 16);
		//	if (flag10 && flag7 | flag8)
		//	{
		//		flag7 = true;
		//		flag8 = true;
		//	}
		//	if ((!flag9 || !flag7 && !flag8) && (!flag10 || !flag9))
		//	{
		//		if (flag9)
		//		{
		//			rectangle4 = new Rectangle(0, 4, 16, 4);
		//			if (tile.IsHalfBlock || tile.Slope != 0)
		//				rectangle4 = new Rectangle(0, 4, 16, 12);
		//		}
		//		else if (!flag10 || flag7 || flag8)
		//		{
		//			float single1 = (256 - num115);
		//			single1 /= 32f;
		//			int num118 = 4;

		//			if (tile3.LiquidAmount == 0 && !WorldGen.SolidTile(x, y - 1))
		//				num118 = 0;

		//			if (flag7 & flag8 || tile.IsHalfBlock || tile.Slope != 0)
		//			{
		//				vectorPosition = new Vector2(x * 16, y * 16 + (int)single1 * 2);
		//				rectangle4 = new Rectangle(0, num118, 16, 16 - (int)single1 * 2);
		//			}
		//			else if (!flag7)
		//			{
		//				vectorPosition = new Vector2(x * 16 + 12, y * 16 + (int)single1 * 2);
		//				rectangle4 = new Rectangle(0, num118, 4, 16 - (int)single1 * 2);
		//			}
		//			else
		//			{
		//				vectorPosition = new Vector2(x * 16, y * 16 + (int)single1 * 2);
		//				rectangle4 = new Rectangle(0, num118, 4, 16 - (int)single1 * 2);
		//			}
		//		}
		//		else
		//		{
		//			vectorPosition = new Vector2(x * 16, y * 16 + 12);
		//			rectangle4 = new Rectangle(0, 4, 16, 4);
		//		}
		//	}
		//	gameAlpha = 0.5f;
		//	if (style == 1)
		//	{
		//		gameAlpha = 1f;
		//	}
		//	else if (style == 11)
		//	{
		//		gameAlpha *= 1.7f;
		//		if (gameAlpha > 1f)
		//			gameAlpha = 1f;
		//	}
		//	if (y < Main.worldSurface || gameAlpha > 1f)
		//	{
		//		gameAlpha = 1f;
		//		if (tile3.WallType > 0 || tile2.WallType > 0 || tile1.WallType > 0 || tile4.WallType > 0)
		//			gameAlpha = 0.65f;
		//		if (tile.WallType > 0)
		//			gameAlpha = 0.5f;
		//	}
		//	if (tile.IsHalfBlock && tile3.LiquidAmount > 0 && tile.WallType > 0)
		//		gameAlpha = 0f;
		//}

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
