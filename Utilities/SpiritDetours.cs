using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using SpiritMod.Particles;
using SpiritMod.Tiles;
using System;
using ReLogic.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Items.Sets.ToolsMisc.Evergreen;
using SpiritMod.Mechanics.PortraitSystem;
using SpiritMod.Mechanics.BackgroundSystem;
using System.Collections.Generic;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Players;
using SpiritMod.Effects.SurfaceWaterModifications;
using SpiritMod.Items.Sets.FloatingItems.MessageBottle;
using MonoMod.RuntimeDetour.HookGen;
using System.Reflection;
using SpiritMod.Items.Weapon.Summon.StardustBomb;
using SpiritMod.NPCs.Town.Oracle;
using SpiritMod.NPCs.Hydra;
using SpiritMod.NPCs.StarjinxEvent.Enemies.Pathfinder;

namespace SpiritMod.Utilities
{
	public static class SpiritDetours
	{
		public static void Initialize()
		{
			On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;
			On.Terraria.Main.DrawCachedProjs += Main_DrawCachedProjs;
			On.Terraria.Main.DrawNPCs += Main_DrawNPCs;
			On.Terraria.Main.DrawPlayers += Main_DrawPlayers;
			On.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float += Projectile_NewProjectile;
			On.Terraria.Player.KeyDoubleTap += Player_KeyDoubleTap;
			On.Terraria.Main.DrawDust += AdditiveCalls;
			On.Terraria.Player.ToggleInv += Player_ToggleInv;
			On.Terraria.Main.DrawInterface += DrawParticles;
			On.Terraria.Localization.LanguageManager.GetTextValue_string += LanguageManager_GetTextValue_string1;

			On.Terraria.Main.DrawPlayerChat += Main_DrawPlayerChat;

			On.Terraria.Main.DrawNPCChatButtons += Main_DrawNPCChatButtons;
			On.Terraria.WorldGen.SpreadGrass += WorldGen_SpreadGrass;

			On.Terraria.Main.DrawBackgroundBlackFill += Main_DrawBackgroundBlackFill; //BackgroundItemManager.Draw()
			On.Terraria.Main.Update += Main_Update; //BackgroundItemManager.Update()

			On.Terraria.Main.DrawMap += Main_DrawMap;

			Main.OnPreDraw += Main_OnPreDraw;
			On.Terraria.Main.DrawWater += Main_DrawWater;

			IL.Terraria.Player.Hurt += Player_Hurt;
			IL.Terraria.Player.ItemCheck += Player_ItemCheck;
			IL.Terraria.WorldGen.hardUpdateWorld += WorldGen_hardUpdateWorld;
			IL.Terraria.Main.DrawMouseOver += Main_DrawMouseOver;

			SurfaceWaterModifications.Load();
			HookEndpointManager.Add<hook_NPCAI>(NPCAIMethod, (hook_NPCAI)NPCAIMod);
		}

		public static void Unload()
		{
			On.Terraria.Main.DrawProjectiles -= Main_DrawProjectiles;
			On.Terraria.Main.DrawNPCs -= Main_DrawNPCs;
			On.Terraria.Main.DrawPlayers -= Main_DrawPlayers;
			On.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float -= Projectile_NewProjectile;
			On.Terraria.Player.KeyDoubleTap -= Player_KeyDoubleTap;
			On.Terraria.Main.DrawDust -= AdditiveCalls;
			On.Terraria.Player.ToggleInv -= Player_ToggleInv;
			On.Terraria.Main.DrawInterface -= DrawParticles;
			On.Terraria.Localization.LanguageManager.GetTextValue_string -= LanguageManager_GetTextValue_string1;
			On.Terraria.Main.DrawPlayerChat -= Main_DrawPlayerChat;

			On.Terraria.Main.DrawNPCChatButtons -= Main_DrawNPCChatButtons;
			On.Terraria.WorldGen.SpreadGrass -= WorldGen_SpreadGrass;

			Main.OnPreDraw -= Main_OnPreDraw;
			On.Terraria.Main.DrawWater -= Main_DrawWater;

			IL.Terraria.Player.Hurt -= Player_Hurt;
			IL.Terraria.Player.ItemCheck -= Player_ItemCheck;
			IL.Terraria.WorldGen.hardUpdateWorld -= WorldGen_hardUpdateWorld;
			IL.Terraria.Main.DrawMouseOver -= Main_DrawMouseOver;

			SurfaceWaterModifications.Unload();
			HookEndpointManager.Remove<hook_NPCAI>(NPCAIMethod, (hook_NPCAI)NPCAIMod);
		}

		private static readonly MethodInfo NPCAIMethod = typeof(NPCLoader).GetMethod(nameof(NPCLoader.NPCAI));
		public delegate void orig_NPCAI(NPC npc);
		public delegate void hook_NPCAI(orig_NPCAI orig, NPC npc);

		/// <summary>Detour for NPCLoader.NPCAI; allows the Message in a Bottle mount to still have waterborne enemies target it when not "in" the water.</summary>
		private static void NPCAIMod(orig_NPCAI orig, NPC npc)
		{
			bool removeWet = false;
			Player target = null;

			if (npc.target >= 0)
			{
				target = Main.player[npc.target];
				removeWet = target.HasBuff(ModContent.BuffType<BottleMountBuff>()) && (target.velocity.Y == -target.gravity || target.velocity.Y == 0);
			}

			if (removeWet)
				target.wet = true;

			orig(npc);

			if (removeWet)
				target.wet = false;
		}

		private static void Main_DrawMap(On.Terraria.Main.orig_DrawMap orig, Main self)
		{
			orig(self);

			if (Main.mapEnabled && !Main.mapFullscreen) //Draw only on the minimap
				DrawPinsOnMiniMap(); //Main.mapStyle == 0, fullscreen; == 1, minimap; == 2, overlay
		}

		private static void DrawPinsOnMiniMap()
		{
			var pins = ModContent.GetInstance<Items.Pins.PinWorld>().pins;

			foreach (var pair in pins)
			{
				float scale = Main.mapMinimapScale;

				Vector2 realPos = GetMiniMapPosition(pins.Get<Vector2>(pair.Key), scale);

				if (scale > 1f)
					scale = 1f;
				scale *= Main.UIScale;

				float drawScale = (scale * 0.5f + 1f) / 3f;
				if (drawScale > 1f)
					drawScale = 1f;

				if (PointOnMinimap(realPos))
					DrawOnMinimap((int)realPos.X, (int)realPos.Y, drawScale, ModContent.GetTexture($"SpiritMod/Items/Pins/Textures/Pin{pair.Key}Map"));
			}
		}

		public static bool PointOnMinimap(Vector2 pos, float fluff = 4) => pos.X > (Main.miniMapX + fluff) && pos.X < (Main.miniMapX + Main.miniMapWidth - fluff) && pos.Y > (Main.miniMapY + fluff) && pos.Y < (Main.miniMapY + Main.miniMapHeight - fluff);

		/// <summary>Gets the position of something on the minimap given the tile coordinates.</summary>
		/// <param name="scale"></param>
		/// <param name="tilePos"></param>
		/// <returns></returns>
		private static Vector2 GetMiniMapPosition(Vector2 tilePos, float scale = 1f)
		{
			float screenHalfWidthInTiles = (Main.screenPosition.X + (PlayerInput.RealScreenWidth / 2f)) / 16f;
			float screenHalfHeightInTiles = (Main.screenPosition.Y + (PlayerInput.RealScreenHeight / 2f)) / 16f;
			float offsetX = screenHalfWidthInTiles - (Main.miniMapWidth / scale) / 2f;
			float offsetY = screenHalfHeightInTiles - (Main.miniMapHeight / scale) / 2f;

			float drawPosX = (tilePos.X - offsetX) * scale;
			float drawPosY = (tilePos.Y - offsetY) * scale;

			Vector2 miniMapSize = new Vector2(Main.miniMapX, Main.miniMapY);
			drawPosX += miniMapSize.X;
			drawPosY += miniMapSize.Y;
			drawPosY -= 2f * scale / 5f;

			return new Vector2(drawPosX, drawPosY);
		}

		public static void DrawOnMinimap(int posX, int posY, float scale, Texture2D tex)
		{
			Vector2 scrPos = new Vector2(posX, posY);
			Main.spriteBatch.Draw(
				texture: tex,
				position: scrPos,
				sourceRectangle: null,
				color: Color.White,
				rotation: 0f,
				origin: new Vector2(tex.Width / 2, tex.Height / 2),
				scale: scale,
				effects: SpriteEffects.None,
				layerDepth: 1f
			);
		}

		private static void Main_DrawWater(On.Terraria.Main.orig_DrawWater orig, Main self, bool bg, int Style, float Alpha)
		{
			if (Main.LocalPlayer.ZoneBeach)
				Alpha *= 0.7f; //Cute little effect to make the water seem more seethrough

			orig(self, bg, Style, Alpha);
		}

		private static void Main_Update(On.Terraria.Main.orig_Update orig, Main self, GameTime gameTime)
		{
			if (Main.playerLoaded && BackgroundItemManager.Loaded && !Main.gamePaused) //Update all background items
				BackgroundItemManager.Update();

			SpiritMod.deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if(SpiritMod.Instance != null)
				SpiritMod.Instance.CheckScreenSize();

			orig(self, gameTime);
		}

		private static void Main_DrawBackgroundBlackFill(On.Terraria.Main.orig_DrawBackgroundBlackFill orig, Main self)
		{
			orig(self);

			if (Main.playerLoaded && BackgroundItemManager.Loaded && !Main.gameMenu)
				BackgroundItemManager.Draw(); //Draw all background items
		}

		private static void Main_DrawPlayerChat(On.Terraria.Main.orig_DrawPlayerChat orig, Main self)
		{
			orig(self);

			// to make quest text make the menu tick when you hover over it we need a way to only play it once
			// so this is just here to call PostUpdate any QuestSnippet objects.
			int num3 = Main.startChatLine;
			int num4 = Main.startChatLine + Main.showCount;
			if (num4 >= Main.numChatLines)
			{
				int num5 = Main.numChatLines - 1;
				Main.numChatLines = num5;
				num4 = num5;
				num3 = num4 - Main.showCount;
			}
			for (int i = num3; i < num4; i++)
			{
				int len = Main.chatLine[i].parsedText.Length;
				for (int j = 0; j < len; j++)
				{
					if (Main.chatLine[i].parsedText[j] is UI.Chat.QuestTagHandler.QuestSnippet snip)
						snip.PostUpdate();
				}
			}
		}

		private static void AdditiveCalls(On.Terraria.Main.orig_DrawDust orig, Main self)
		{
			AdditiveCallManager.DrawAdditiveCalls(Main.spriteBatch);
			orig(self);
		}

		private const float ProfileNameScale = 1f; //Profile name scale - 1f because the higher is poorly resized
		public static bool HoveringQuestButton = false;
		private static void Main_DrawNPCChatButtons(On.Terraria.Main.orig_DrawNPCChatButtons orig, int superColor, Color chatColor, int numLines, string focusText, string focusText3) //Portrait drawing - Gabe
		{
			if (Main.LocalPlayer.talkNPC != -1)
			{
				NPC talkNPC = Main.npc[Main.LocalPlayer.talkNPC];
				//bool hasPortrait = PortraitManager.HasPortrait(talkNPC.type) || PortraitManager.HasCallPortrait(talkNPC.type);

				//if (ModContent.GetInstance<SpiritClientConfig>().ShowNPCPortraits && hasPortrait)
				//{
				//	Point size = new Point(108, 108);
				//	if (PortraitManager.HasPortrait(talkNPC.type)) //In-house portrait
				//	{
				//		BasePortrait portrait = PortraitManager.GetPortrait(talkNPC.type); //Gets portrait
				//		size = portrait.BaseSize;

				//		var pos = new Vector2(Main.screenWidth / 2 - (252 + portrait.BaseSize.X), 100); //Portrait position
				//		Main.spriteBatch.Draw(portrait.Texture, pos, portrait.GetFrame(Main.npcChatText, talkNPC), Color.White, 0f, default, 1f, SpriteEffects.None, 0f); //Portrait
				//	}
				//	else if (PortraitManager.HasCallPortrait(talkNPC.type)) //Mod.Call portrait
				//	{
				//		ModCallPortrait portrait = PortraitManager.GetCallPortrait(talkNPC.type); //Gets portrait
				//		size = portrait.BaseSize;

				//		var pos = new Vector2(Main.screenWidth / 2 - (252 + portrait.BaseSize.X), 100); //Portrait position
				//		Main.spriteBatch.Draw(portrait.Texture, pos, portrait.GetFrame(Main.npcChatText, talkNPC), Color.White, 0f, default, 1f, SpriteEffects.None, 0f); //Portrait
				//	}

				//	DrawPortraitName(talkNPC, size); //Draws the name
				//}

				if (talkNPC.type == NPCID.Angler)
					focusText = ""; // empty string, we'll add our own angler quest button

				var queue = ModContent.GetInstance<QuestWorld>().NPCQuestQueue;

				if (queue.ContainsKey(talkNPC.type) && queue[talkNPC.type].Count > 0) //If this NPC has a quest
				{
					// TODO: localization
					string questText = "Quest";

					DynamicSpriteFont font = Main.fontMouseText;
					Vector2 scale = new Vector2(0.9f);
					Vector2 stringSize = ChatManager.GetStringSize(font, questText, scale);
					Vector2 position = new Vector2((180 + Main.screenWidth / 2) + stringSize.X - 50f, 130 + numLines * 30);
					Color baseColor = new Color(superColor, (int)(superColor / 1.1), superColor / 2, superColor);
					Vector2 mousePos = new Vector2(Main.mouseX, Main.mouseY);

					if (mousePos.Between(position, position + stringSize * scale) && !PlayerInput.IgnoreMouseInterface) //Mouse hovers over button
					{
						Main.LocalPlayer.mouseInterface = true;
						Main.LocalPlayer.releaseUseItem = true;
						scale *= 1.1f;

						if (!HoveringQuestButton)
							Main.PlaySound(SoundID.MenuTick);

						HoveringQuestButton = true;

						if (Main.mouseLeft && Main.mouseLeftRelease) //If clicked on, unlock a quest.
						{
							Quest q = queue[talkNPC.type].Dequeue();
							QuestManager.UnlockQuest(q, true);

							Main.npcChatText = q.QuestDescription;
						}
					}
					else
					{
						if (HoveringQuestButton)
							Main.PlaySound(SoundID.MenuTick);

						HoveringQuestButton = false;
					}

					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, questText, position + new Vector2(16f, 14f), baseColor, 0f,
						stringSize * 0.5f, scale * new Vector2(1f));
				}

				if (talkNPC.type == ModContent.NPCType<Oracle>())
					Oracle.DrawBuffButton(superColor, numLines);
			}
			orig(superColor, chatColor, numLines, focusText, focusText3);
		}

		private static void DrawPortraitName(NPC talkNPC, Point size)
		{
			string name = talkNPC.GivenName;
			Vector2 centring = ChatManager.GetStringSize(Main.fontItemStack, name, new Vector2(ProfileNameScale)) / 2; //Position centring
			Vector2 textPos = new Vector2(Main.screenWidth / 2 - (198 + size.X), 114 + size.Y) - centring; //Real position
			ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontItemStack, name, textPos, new Color(240, 240, 240), 0f, new Vector2(), new Vector2(ProfileNameScale), -1, 2f); //Name
		}

		private static void Main_OnPreDraw(GameTime obj)
		{
			if (Main.spriteBatch != null && SpiritMod.primitives != null) 
			{
				SpiritMod.primitives.DrawTrailsProj(Main.spriteBatch, Main.graphics.GraphicsDevice);
				SpiritMod.primitives.DrawTrailsNPC(Main.spriteBatch, Main.graphics.GraphicsDevice);
			}
		}

		private static void Main_DrawCachedProjs(On.Terraria.Main.orig_DrawCachedProjs orig, Main self, List<int> projCache, bool startSpriteBatch)
		{
			if (!Main.dedServ && projCache == Main.instance.DrawCacheProjsBehindNPCs)
				SpiritMod.TrailManager.DrawTrails(Main.spriteBatch, TrailLayer.UnderCachedProjsBehindNPC);

			orig(self, projCache, startSpriteBatch);
		}
		private static void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
		{
			if (!Main.dedServ)
			{
				SpiritMod.TrailManager.DrawTrails(Main.spriteBatch, TrailLayer.UnderProjectile);
				SpiritMod.primitives.DrawTargetProj(Main.spriteBatch);
			}
			orig(self);
		}

		private static void Main_DrawNPCs(On.Terraria.Main.orig_DrawNPCs orig, Main self, bool behindTiles)
		{
			if (!Main.dedServ)
				SpiritMod.primitives.DrawTargetNPC(Main.spriteBatch);

			SpiritMod.Metaballs.DrawEnemyLayer(Main.spriteBatch);
			SpiritMod.Metaballs.DrawNebulaLayer(Main.spriteBatch);

			PathfinderGNPC.DrawBuffedOutlines(Main.spriteBatch);
			orig(self, behindTiles);
		}

		private static void Main_DrawPlayers(On.Terraria.Main.orig_DrawPlayers orig, Main self)
		{
			orig(self);

			ExtraDrawOnPlayer.DrawPlayers();
			SpiritMod.Metaballs.DrawFriendlyLayer(Main.spriteBatch);
		}

		private static int Projectile_NewProjectile(On.Terraria.Projectile.orig_NewProjectile_float_float_float_float_int_int_float_int_float_float orig, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner, float ai0, float ai1)
		{
			int index = orig(X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1);
			Projectile projectile = Main.projectile[index];

			if (projectile.modProjectile is ITrailProjectile) {
				if(Main.netMode == NetmodeID.SinglePlayer)
					(projectile.modProjectile as ITrailProjectile).DoTrailCreation(SpiritMod.TrailManager);

				else 
					SpiritMod.WriteToPacket(SpiritMod.Instance.GetPacket(), (byte)MessageType.SpawnTrail, index).Send();
			}
			//if (Main.netMode != NetmodeID.Server) SpiritMod.TrailManager.DoTrailCreation(projectile);

			return index;
		}

		private static void Player_KeyDoubleTap(On.Terraria.Player.orig_KeyDoubleTap orig, Player self, int keyDir)
		{
			orig(self, keyDir);
			self.GetSpiritPlayer().DoubleTapEffects(keyDir);
		}

		private static void Player_ToggleInv(On.Terraria.Player.orig_ToggleInv orig, Player self)
		{
			SpiritMod spirit = ModContent.GetInstance<SpiritMod>();

			if (spirit.BookUserInterface.CurrentState != null) {
				spirit.BookUserInterface.SetState(null);
				Main.PlaySound(SoundID.MenuClose);
				return;
			}

			orig(self);
		}

        private static string LanguageManager_GetTextValue_string1(On.Terraria.Localization.LanguageManager.orig_GetTextValue_string orig, Terraria.Localization.LanguageManager self, string key)
        {
            if (key == "GameUI.LightRain" || key == "GameUI.Rain" || key == "GameUI.HeavyRain" || key == "GameUI.Clear" || key == "GameUI.PartlyCloudy" || key == "GameUI.MostlyCloudy" || key == "GameUI.Overcast"|| key == "GameUI.Cloudy")
            {
                return SpiritMod.GetWeatherRadioText(key);
            }
            return orig(self, key);
        }

		private static void DrawParticles(On.Terraria.Main.orig_DrawInterface orig, Main self, GameTime gameTime)
		{
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);
			ParticleHandler.DrawAllParticles(Main.spriteBatch);
			Main.spriteBatch.End();

			orig(self, gameTime);
		}

		// detour to stop evil grass from spreading into areas protected by super sunflowers
		private static void WorldGen_SpreadGrass(On.Terraria.WorldGen.orig_SpreadGrass orig, int i, int j, int dirt, int grass, bool repeat, byte color)
		{
			if (grass == TileID.CorruptGrass || grass == TileID.FleshGrass)
				foreach (Point16 point in MyWorld.superSunFlowerPositions)
					if (Math.Abs(point.X - i) < SuperSunFlower.Range * 2)
							return;

			orig(i, j, dirt, grass, repeat, color);
		}

		/// This IL edit is used to allow the unfeller of evergreens to autoplant saplings when trees are destroyed
		/// Sadly tML doesn't provide tile destruction info by default in a way feasible in MP, so we have to resort to this
		///
		/// THESE ARE THE IL INSTRUCTIONS WE'LL HARASS AND WHAT IT'LL LOOK LIKE WHEN WE'RE DONE
		///   IL_af0c: ldloc 352    <----- WE WILL FIRST FIND AND JUMP TO THIS INSTRUCTION
		///   IL_af10: nop
		///   IL_af11: nop
		///   IL_af12: callvirt instance void Terraria.HitTile::Clear(int32)  <----- NEXT WE JUMP HERE
		///   IL_XXXX: [WE INSERT AN INSTRUCTION HERE TO PUSH THE TYPE OF THE TILE THE PLAYER JUST MINED]
		///   IL_af17: ldsfld int32 Terraria.Player::tileTargetX
		///   IL_af1c: ldsfld int32 Terraria.Player::tileTargetY
		///   IL_af21: ldc.i4.0
		///   IL_af22: ldc.i4.0
		///   IL_af23: ldc.i4.0
		///   IL_af24: call void Terraria.WorldGen::KillTile(int32, int32, bool, bool, bool)
		///   IL_XXXX: [WE RUN OUR OWN CODE HERE TO PLANT THE SAPLING]
		private static void Player_ItemCheck(ILContext il) {
			// Get an ILCursor and a logger to report errors if we find any
			ILCursor cursor = new ILCursor(il);
			ILog logger = ModContent.GetInstance<SpiritMod>().Logger;

			// Here we try to make the ILCursor jump to instruction IL_af0c (ldloc 352)
			// The instruction "ldloc 352" occurs 3 times before the one we want at IL_af0c
			// We will ask the cursor 4 times to go to the next specified instruction (in this case go to ldloc 352, 4 times)
			// If it can't find it, we stop IL editing and log an error
			for (int c = 0; c < 4; c++) {
				if (!cursor.TryGotoNext(i => i.MatchLdloc(352))) {
					logger.Error("Failed to patch Player.ItemCheck for Unfeller of Evergreens: First jump failed");
					return;
				}
			}

			// At this point we reached the instruction we want
			// Now we make the cursor jump again to the HitTile.Clear call at IL_af12
			// Same error handling as before
			if (!cursor.TryGotoNext(i => i.MatchCallvirt<HitTile>("Clear"))) {
				logger.Error("Failed to patch Player.ItemCheck for Unfeller of Evergreens: Second jump failed");
				return;
			}

			// We increment the cursor index by 1 to skip over the HitTile.Clear instruction
			// Then we emit a delegate to push the type of the targetted tile onto the stack
			cursor.Index++;
			cursor.EmitDelegate<Func<ushort>>(() => Main.tile[Player.tileTargetX, Player.tileTargetY].type);

			// Now we jump for the final time to the WorldGen.KillTile call at IL_af24
			if (!cursor.TryGotoNext(i => i.MatchCall<WorldGen>("KillTile"))) {
				logger.Error("Failed to patch Player.ItemCheck for Unfeller of Evergreens: Final jump failed");
				return;
			}

			// Increment cursor index by 1 because we want to insert after the WorldGen.KillTile call
			cursor.Index++;

			// time for the fun stuff b a y b e e
			// We still have the type of the tile that got destroyed earlier pushed onto the stack
			// Now we use it by emitting a delegate that consumes this value from the stack, and we execute our own code
			// (Mother nature is happy with you)
			cursor.EmitDelegate<Action<ushort>>(type => {
				// Item that was used to kill the tile
				Item item = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem];

				// If the tile that got destroyed isn't a tree, or if the item type isn't the unfeller of evergreens, we stop and return
				if (!(type == TileID.Trees || type == TileID.PalmTree) || item.type != ModContent.ItemType<UnfellerOfEvergreens>())
					return;

				// We will keep moving this tile down until we hit the tile the tree was sitting on (which will be the first solid tile we find)
				int currentX = Player.tileTargetX;
				int currentY = Player.tileTargetY;
				Tile currentTile = Framing.GetTileSafely(currentX, currentY);

				while (!currentTile.active() || !Main.tileSolid[currentTile.type])
					currentTile = Framing.GetTileSafely(currentX, ++currentY);

				// Now we finish up and plant a sapling above the tile we hit
				WorldGen.PlaceTile(currentX, currentY - 1, TileID.Saplings);
			});
		}

		/// This IL edit is used to stop evil stones (ebonstone/crimstone) from spreading into areas protected by super sunflowers
		/// Evil grass is also handled in a separate detour (WorldGen_SpreadGrass)
		///
		/// This edit is simple. We just need to search for one instruction that loads NPC.downedPlantBoss (as all instructions afterwards have to do with evil spreading).
		/// Then we need to push the "i" parameter onto the stack (the x coordinate of the updated tile)
		/// 
		/// THESE ARE THE IL INSTRUCTIONS WE'LL HARASS AND WHAT IT'LL LOOK LIKE WHEN WE'RE DONE
		///   IL_XXXX: [WE INSERT AN INSTRUCTION HERE TO PUSH THE FIRST PARAMETER, i (THE X COORDINATE OF THE TILE)]
		///   IL_XXXX: [WE RUN OUR CODE HERE TO CHECK IF THE X COORDINATE IS IN A PROTECTED ZONE. WE PUSH TRUE ONTO THE STACK IF IT IS, OTHERWISE FALSE]
		///   IL_XXXX: [WE INSERT AN INSTRUCTION HERE TO CHECK THE BOOL WE PUSHED, AND SKIP THE NEXT INSTRUCTION IF IT IS FALSE]
		///   IL_XXXX: [WE INSERT AN INSTRUCTION HERE TO RETURN FROM THE METHOD. THIS IS ONLY REACHED IF THE ABOVE INSTRUCTION DOESN'T BRANCH]
		///   IL_050f: ldsfld bool [Terraria]Terraria.NPC::downedPlantBoss   <------ WE WILL JUMP TO THIS INSTRUCTION TO APPLY THE ABOVE EDITS
		private static void WorldGen_hardUpdateWorld(ILContext il)
		{
			// Get an ILCursor and a logger to report errors if we find any
			ILCursor cursor = new ILCursor(il);
			ILog logger = ModContent.GetInstance<SpiritMod>().Logger;

			// Try to jump to the specified instruction and stop if we can't find it
			// We use MoveType.AfterLabel because there are labels from previous instructions pointing to the location we're emitting to
			// and thus we want our emitted instructions to become the target for the labels, instead of the NPC.downedPlantBoss instruction
			if (!cursor.TryGotoNext(MoveType.AfterLabel, i => i.MatchLdsfld<NPC>("downedPlantBoss"))) {
				logger.Error("Failed to patch WorldGen.hardUpdateWorld to add super sunflower functionality");
				return;
			}

			// We'll make use of this empty label shortly. It is supposed to target the NPC.downedPlantBoss instruction; but we initialize it later
			// This is to account for the fact that you can't spell "branch" without "bitch" and will save us from pain
			ILLabel label = cursor.DefineLabel();

			// Now that the cursor is behind the target instruction, we need to push the parameter "i" onto the stack
			cursor.Emit(OpCodes.Ldarg_0);

			// Now we run our own code that consumes "i" off the stack, checks if they're in a protected zone and returns the result of the check
			cursor.EmitDelegate<Func<int, bool>>(i => {
				foreach (Point16 point in MyWorld.superSunFlowerPositions)
					if (Math.Abs(point.X - i) < SuperSunFlower.Range * 2)
						return true;

				return false;
			});
			
			// We check the bool we pushed, and branch to the label we defined earlier (the one supposed to be looking at the NPC.downedPlantBoss instruction) if it is false
			// Doing so would skip over the return instruction we are about to emit
			cursor.Emit(OpCodes.Brfalse_S, label);

			// A return instruction that will get run if the tile is in a protected zone (meaning that the above branch didn't run)
			cursor.Emit(OpCodes.Ret);

			// We finish off by moving the cursor forwards again to the NPC.downedPlantBoss instruction and actually initializing the label
			// The reason we do this now is because emitting instructions can easily mess with labels and move them around
			// But since we only have 1 label to worry about we can just initialize it after everything has been emitted
			cursor.GotoNext(i => i.MatchLdsfld<NPC>("downedPlantBoss"));
			cursor.MarkLabel(label);
		}

		public delegate float KnockbackMultiplierDelegate(Player player, bool horizontal);
		private static void Player_Hurt(ILContext il)
		{
			ILCursor c = new ILCursor(il);

			for (int i = 0; i < 2; ++i)
				if (!c.TryGotoNext(x => x.MatchLdfld<Player>("mount"))) //match ldfld for mount twice to get to the right call
					return;
			if (!c.TryGotoNext(x => x.MatchLdcR4(4.5f))) //then match the 4.5 ldcr4
				return;

			if (!c.TryGotoNext(x => x.MatchMul())) //go to mul instruction, then move past it.
				return;
			c.Index++;

			//Now we do some shenanigains.
			c.Emit(OpCodes.Ldarg_0); //Push player onto stack so KnockbackMultiplier can consume it
			c.Emit(OpCodes.Ldc_I4_0); //Push 0 (true) to the stack, since this is horizontal knockback
			c.EmitDelegate<KnockbackMultiplierDelegate>(MiscAccessoryPlayer.KnockbackMultiplier); //Get the kb modifier
			c.Emit(OpCodes.Mul); //and multiply the value already on the stack by the modifier

			//and we move to the velocity.Y 
			if (!c.TryGotoNext(x => x.MatchLdcR4(-3.5f))) //match the -3.5f ldcr4
				return;
			c.Index++;

			//and match the same things
			c.Emit(OpCodes.Ldarg_0);
			c.Emit(OpCodes.Ldc_I4_1); //Only difference is this pushes 1, because this is vertical knockback
			c.EmitDelegate<KnockbackMultiplierDelegate>(MiscAccessoryPlayer.KnockbackMultiplier);
			c.Emit(OpCodes.Mul);
		}

		private static void Main_DrawMouseOver(ILContext il)
		{
			ILCursor c = new ILCursor(il);

			for (int i = 0; i < 3; ++i)
				if (!c.TryGotoNext(x => x.MatchStfld<Player>("showItemIcon"))) //match stfld for showItemIcon 3 times for the right call
					return;

			c.Index -= 5;
			c.Emit(OpCodes.Ldsfld, typeof(Main).GetField("npc")); //Push the current NPC to the stack -->
			c.Emit(OpCodes.Ldloc_S, (byte)8);
			c.Emit(OpCodes.Ldelem_Ref); // <--
			c.EmitDelegate<Func<bool, NPC, bool>>(CheckDrawLife); //And make a hook for overriding if the life stat draws
		}

		private static bool CheckDrawLife(bool flag, NPC self)
		{
			if (self.type == ModContent.NPCType<StardustBombNPC>() || self.type == ModContent.NPCType<Hydra>())
				return false;

			return flag;
		}
	}
}
