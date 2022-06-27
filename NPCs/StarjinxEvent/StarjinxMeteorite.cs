using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs.StarjinxEvent.Comets;
using System.Linq;
using System.Collections.Generic;

namespace SpiritMod.NPCs.StarjinxEvent
{
	[AutoloadBossHead]
	public class StarjinxMeteorite : ModNPC
	{
		public const int EVENT_RADIUS = 1500;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starjinx");
			NPCID.Sets.TrailCacheLength[NPC.type] = 20;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetDefaults()
		{
			NPC.aiStyle = -1;
			NPC.lifeMax = 2000;
			NPC.defense = 20;
			NPC.value = 0f;
			NPC.dontTakeDamage = false;
			NPC.knockBackResist = 0f;
			NPC.width = 60;
			NPC.height = 60;
			NPC.damage = 0;
			NPC.lavaImmune = false;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.Item89;
			NPC.alpha = 255;
			NPC.chaseable = false;
			AIType = 0;
			for (int i = 0; i < BuffLoader.BuffCount; i++)
				NPC.buffImmune[i] = true;

			//Draw on a layer before all other npcs, to help with visual clarity
			NPC.behindTiles = true;
		}

		float sinCounter;
		float shieldOpacity = 0f;
		bool spawnedComets = false;

		private float musicVolume = 1f;

		private List<int> comets = new List<int>();
		private Dictionary<string, int> savedComets = new Dictionary<string, int>() { { "Small", -2 }, { "Medium", -2 }, { "Large", -2 } };

		public bool updateCometOrder = true;
		public bool spawnedBoss = false;
		public int bossWhoAmI = -1;

		public override void AI()
		{
			NPC.velocity.X = 0;
			NPC.gfxOffY = (float)Math.Sin(sinCounter) * 6f;
			sinCounter += 0.03f;

			if (NPC.alpha > 0)
				NPC.alpha -= 10;
			else
				NPC.alpha = 0;

			if (Vector2.DistanceSquared(NPC.Center, Main.LocalPlayer.Center) < EVENT_RADIUS * EVENT_RADIUS && spawnedBoss && bossWhoAmI == -1)
				musicVolume = Math.Max(musicVolume - 0.0125f, 0); //Fully fade out music when event is done
			else
				musicVolume = Math.Min(musicVolume + 0.0125f, 1f);

			if (spawnedComets && comets.Count <= 0) //once all small comets have been defeated
			{
				if (spawnedBoss)
				{
					if (NPC.AnyNPCs(ModContent.NPCType<Enemies.Archon.Archon>()) || NPC.AnyNPCs(ModContent.NPCType<Enemies.Warden.Warden>()))
						musicVolume = Math.Min(musicVolume + 0.0125f, 1f);
					else
					{
						NPC.dontTakeDamage = false;
						shieldOpacity = MathHelper.Lerp(shieldOpacity, 0, 0.05f);

						//musicVolume = Math.Max(musicVolume - 0.0125f, 0); //Fully fade out music when event is done

						StarjinxPlayer localSjinxPlayer = Main.LocalPlayer.GetModPlayer<StarjinxPlayer>();
						VignettePlayer localVignettePlayer = Main.LocalPlayer.GetModPlayer<VignettePlayer>();
						if (localSjinxPlayer.zoneStarjinxEvent || localSjinxPlayer.oldZoneStarjinx)
							localVignettePlayer.SetVignette(0, 1000, 0.75f, Color.Black, NPC.Center);
					}
				}
				else
				{
					int boss = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y - 400, ModContent.NPCType<Enemies.Warden.Warden>());

					StarjinxEventWorld.SetMaxEnemies(2);
					StarjinxEventWorld.SetComets(comets.Count);

					spawnedBoss = true;
					bossWhoAmI = boss;
				}
			}

			if (spawnedComets)
			{
				bool anyPlayerInRange = false;

				for (int i = 0; i < Main.maxPlayers; i++) //Check if players are in range
				{
					Player player = Main.player[i];
					if (player.active && !player.dead)
					{
						if (player.DistanceSQ(NPC.Center) < EVENT_RADIUS * EVENT_RADIUS)
						{
							player.GetModPlayer<StarjinxPlayer>().zoneStarjinxEvent = true;
							player.GetModPlayer<StarjinxPlayer>().StarjinxPosition = NPC.Center;
							anyPlayerInRange = true;
						}
						//else if (player.GetModPlayer<StarjinxPlayer>().oldZoneStarjinx)
						//{
						//}
					}
				}

				if (!anyPlayerInRange) //Reset event
				{
					savedComets["Small"] = savedComets["Medium"] = savedComets["Large"] = -1; //So we "save" if all of them are dead

					IterateComets((SmallComet comet) =>
					{
						savedComets[comet.Size]++;
						comet.NPC.active = false;
					});

					spawnedComets = false;
					comets.Clear();

					DespawnPlatforms();

					NPC.life = NPC.lifeMax; //Reset the meteor completely
					NPC.dontTakeDamage = false;

					shieldOpacity = 0f;
				}

				if (NPC.dontTakeDamage && comets.Count > 0) //Child meteor active checks
				{
					shieldOpacity = MathHelper.Lerp(shieldOpacity, 1, 0.05f);
					for (int i = 0; i < comets.Count; i++)
					{
						NPC comet = Main.npc[comets[i]];
						int[] cometTypes = new int[] { ModContent.NPCType<LargeComet>(), ModContent.NPCType<SmallComet>(), ModContent.NPCType<MediumComet>() };
						if (comet.active && comet.life > 0 && cometTypes.Contains(comet.type))
							break;
					}

					if (updateCometOrder)
					{
						for (int i = 0; i < comets.Count; i++) //update list
						{
							NPC comet = Main.npc[comets[i]];
							if (!comet.active || comet.ModNPC == null || !(comet.ModNPC is SmallComet))
							{
								comets.Remove(comets[i]);
								i--;
								continue;
							}
						}

						if (comets.Count <= 0) return;

						NPC furthest = Main.npc[comets[0]];
						for (int i = 0; i < comets.Count; i++)
						{
							NPC comet = Main.npc[comets[i]];

							if (comet.active && comet.ModNPC is SmallComet npc && furthest.ModNPC is SmallComet far && npc.initialDistance > far.initialDistance)
								furthest = comet;
						}

						for (int i = 0; i < comets.Count; i++)
						{
							NPC comet = Main.npc[comets[i]];
							if (comet.active && comet.ModNPC is SmallComet)
								comet.dontTakeDamage = true;
							if (comet.whoAmI == furthest.whoAmI)
								(comet.ModNPC as SmallComet).nextUp = true;
						}

						NPC.netUpdate = true;
						updateCometOrder = false;

						StarjinxEventWorld.SetComets(comets.Count); //Update to new comet amount
					}

					bool anyVulnerable = false;
					foreach (int cometID in comets)
					{
						if (!Main.npc[cometID].dontTakeDamage)
							anyVulnerable = true;
					}

					if (anyVulnerable) //Make music quieter when a comet is vulnerable to damage, and bring back up the volume when it's dead
						musicVolume = Math.Max(musicVolume - 0.0125f, 0.5f);
					else
						musicVolume = Math.Min(musicVolume + 0.0125f, 1f);
				}
			}

			if (!spawnedComets && NPC.life < NPC.lifeMax) //Spawn meteors below max health (when I take damage) and start the event
			{
				bool validPlayer = false;

				for (int i = 0; i < Main.maxPlayers; ++i) //Search for valid player in order to start event
				{
					Player p = Main.player[i];
					if (p.active && !p.dead && p.DistanceSQ(NPC.Center) < 1500 * 1500)
					{
						validPlayer = true;
						break;
					}
				}

				if (validPlayer)
				{
					SpawnPlatforms();

					spawnedComets = true;
					NPC.dontTakeDamage = true;

					ModContent.GetInstance<StarjinxEventWorld>().StarjinxActive = true;
					NetMessage.SendData(MessageID.WorldData);

					SpawnComets();
				}
			}

			musicVolume = MathHelper.Clamp(musicVolume, 0, 1); //Just in case, been getting some debug messages about value not being valid
			Main.musicFade[Main.curMusic] = musicVolume;
		}

		private void DespawnPlatforms()
		{
			for (int j = 0; j < Main.maxNPCs; ++j)
			{
				NPC plat = Main.npc[j];

				int[] platformTypes = new[] { ModContent.NPCType<SjinxPlatform>(), ModContent.NPCType<SjinxPlatformLarge>(), ModContent.NPCType<SjinxPlatformMedium>() };
				if (plat.active && platformTypes.Contains(plat.type) && plat.DistanceSQ(NPC.Center) < EVENT_RADIUS * EVENT_RADIUS)
					plat.active = false;
			}
		}

		private void SpawnPlatforms()
		{
			const int MinPlatformDistance = 400;
			const int MinPlatformOffset = 200;

			int[] platformTypes = new int[] { ModContent.NPCType<SjinxPlatform>(), ModContent.NPCType<SjinxPlatformLarge>(), ModContent.NPCType<SjinxPlatformMedium>() };
			int platformCount = Main.rand.Next(18, 24);
			var spawns = new List<Vector2>();

			for (int i = 0; i < platformCount; ++i)
			{
				Vector2 pos = Main.rand.NextVector2Circular(EVENT_RADIUS * 0.9f, EVENT_RADIUS * 0.9f);

				while ((spawns.Count > 0 && spawns.Any(x => Vector2.DistanceSquared(x, pos) < MinPlatformDistance * MinPlatformDistance)) || pos.Length() < MinPlatformOffset)
					pos = Main.rand.NextVector2Circular(EVENT_RADIUS * 0.9f, EVENT_RADIUS * 0.9f);

				int n = NPC.NewNPC((int)(NPC.Center.X + pos.X), (int)(NPC.Center.Y + pos.Y), Main.rand.Next(platformTypes));
				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n);

				spawns.Add(pos);
			}
		}

		private void SpawnComets()
		{
			int maxSmallComets = savedComets["Small"] == -2 ? 3 : savedComets["Small"] + 1; //Use cached values if respawning comets
			int maxMedComets = savedComets["Medium"] == -2 ? 2 : savedComets["Medium"] + 1;
			int maxLargeComets = savedComets["Large"] == -2 ? 1 : savedComets["Large"] + 1;

			const float MaxDist = 450;
			const float MinDist = 80;

			float FindDistance()
			{
				float maxComets = maxLargeComets + maxMedComets + maxSmallComets;
				return MathHelper.Lerp(MaxDist, MinDist, comets.Count / maxComets);
			}

			void SpawnComet(int type)
			{
				Vector2 spawnPos = NPC.Center + Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * FindDistance();

				int id = NPC.NewNPC((int)spawnPos.X, (int)spawnPos.Y, type, NPC.whoAmI, NPC.whoAmI, comets.Count * 2, 0, -1);
				Main.npc[id].Center = spawnPos;
				Main.npc[id].dontTakeDamage = true;

				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, id);

				if (comets.Count <= 0 && (savedComets["Small"] != -1 || savedComets["Medium"] != -1 || savedComets["Large"] != -1))
				{ //Kinda ugly but it makes sure the respawned comet is next up
					if (Main.npc[id].ModNPC != null && Main.npc[id].ModNPC is SmallComet comet)
						comet.nextUp = true;
				}

				comets.Add(id);
				NPC.netUpdate = true;
			}

			for (int i = 0; i < maxSmallComets; i++)
				SpawnComet(ModContent.NPCType<SmallComet>());

			for (int i = 0; i < maxMedComets; i++)
				SpawnComet(ModContent.NPCType<MediumComet>());

			for (int i = 0; i < maxLargeComets; i++)
				SpawnComet(ModContent.NPCType<LargeComet>());

			savedComets["Small"] = savedComets["Medium"] = savedComets["Large"] = -2; //clear out cache

			StarjinxUI.Initialize();
			StarjinxEventWorld.SetComets(comets.Count); //Set comet amount for ui purposes
			StarjinxEventWorld.SetMaxEnemies(1); //Initialize max enemies as well, in case of potential edge cases
		}

		private delegate void IterateAction(SmallComet comet);

		private void IterateComets(IterateAction action)
		{
			foreach (int ID in comets)
			{
				if (Main.npc[ID].ModNPC != null)
					if (Main.npc[ID].ModNPC is SmallComet comet)
						action.Invoke(comet);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			IterateComets(delegate (SmallComet comet) //Draw rings and beams underneath everything else
			{
				comet.DrawBeam(spriteBatch);
				comet.DrawRing();
			});

			IterateComets(delegate (SmallComet comet) //Then draw base textures
			{
				comet.PreDraw(spriteBatch, Lighting.GetColor((int)comet.NPC.position.X / 16, (int)comet.NPC.position.Y / 16));
			});
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var center = new Vector2((TextureAssets.Npc[NPC.type].Value.Width / 2), (TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
			float sineAdd = (float)Math.Sin(sinCounter * 1.5f);
			Vector2 drawCenter = NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY);

			//Weird shader stuff, dont touch yuyutsu
			#region shader
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			Vector4 colorMod = Color.LightGoldenrodYellow.ToVector4();
			SpiritMod.StarjinxNoise.Parameters["distance"].SetValue(2.9f - (sineAdd / 10));
			SpiritMod.StarjinxNoise.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.StarjinxNoise.Parameters["noise"].SetValue(Mod.Assets.Request<Texture2D>("Textures/noise").Value);
			SpiritMod.StarjinxNoise.Parameters["rotation"].SetValue(sinCounter / 5);
			SpiritMod.StarjinxNoise.Parameters["opacity2"].SetValue(0.3f - (sineAdd / 10));
			SpiritMod.StarjinxNoise.CurrentTechnique.Passes[0].Apply();
			spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value, drawCenter, null, NPC.GetAlpha(Color.White), 0f, new Vector2(125, 125), 1.3f - (sineAdd / 9), SpriteEffects.None, 0f);

			SpiritMod.StarjinxNoise.Parameters["opacity2"].SetValue(1);
			SpiritMod.StarjinxNoise.Parameters["rotation"].SetValue((sinCounter + 3) / 4);
			colorMod *= 0.66f;
			SpiritMod.StarjinxNoise.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.StarjinxNoise.CurrentTechnique.Passes[0].Apply();

			spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value, drawCenter, null, NPC.GetAlpha(Color.White), 0f, new Vector2(125, 125), 1.1f + (sineAdd / 7), SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			#endregion

			spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/StarjinxEvent/StarjinxMeteorite").Value, drawCenter, null, Color.White, 0f, center, 1, SpriteEffects.None, 0f);

			float cos = (float)Math.Cos((Main.GlobalTimeWrappedHourly % 2.4f / 2.4f * MathHelper.TwoPi)) / 2f + 0.5f;

			SpriteEffects spriteEffects3 = (NPC.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Color baseCol = new Color(127 - NPC.alpha, 127 - NPC.alpha, 127 - NPC.alpha, 0).MultiplyRGBA(Color.White);
			Color drawCol = NPC.GetAlpha(baseCol) * (1f - cos);

			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawCenter, null, NPC.GetAlpha(Color.Lerp(drawColor, Color.White, 0.5f)), 0f, center, 1, SpriteEffects.None, 0f);
			spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/StarjinxEvent/StarjinxMeteoriteGlow").Value, drawCenter, null, NPC.GetAlpha(Color.White * .4f), 0f, center, 1, SpriteEffects.None, 0f);

			for (int i = 0; i < 6; i++)
			{
				Vector2 drawPos = drawCenter + ((i / 6f) * MathHelper.TwoPi + NPC.rotation).ToRotationVector2() * (4f * cos + 2f) - NPC.velocity * i;
				spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/StarjinxEvent/StarjinxMeteoriteGlow").Value, drawPos, NPC.frame, drawCol, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects3, 0f);
				//spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/StarjinxMeteoriteGlowOutline"), drawPos, npc.frame, drawCol, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
			}

			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			SpiritMod.Instance.GetEffect("Effects/SjinxShield").Parameters["vnoiseTex"].SetValue(Mod.Assets.Request<Texture2D>("Textures/voronoiLooping").Value);
			SpiritMod.Instance.GetEffect("Effects/SjinxShield").Parameters["timer"].SetValue(Main.GlobalTimeWrappedHourly / 3);
			SpiritMod.Instance.GetEffect("Effects/SjinxShield").CurrentTechnique.Passes[0].Apply();
			IterateComets(delegate (SmallComet comet)
			{
				comet.DrawShield(Main.spriteBatch);
			});

			Texture2D mask = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value;

			Texture2D mainTex = TextureAssets.Npc[NPC.type].Value;
			float averageSize = mainTex.Width / 2f + mainTex.Height / 2f;
			float scale = averageSize / mask.Width;

			scale *= MathHelper.Lerp(3f, 6f, 1 - shieldOpacity) * (sineAdd / 15f + 1f);

			spriteBatch.Draw(mask, drawCenter, null, Color.LightGoldenrodYellow * shieldOpacity, NPC.rotation + Main.GlobalTimeWrappedHourly, mask.Size() / 2f, scale, SpriteEffects.None, 0);
			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
				for (int k = 0; k < 14; k++)
					Gore.NewGore(NPC.Center + new Vector2(0, Main.rand.NextFloat(42)).RotatedByRandom(MathHelper.Pi), new Vector2(2 * hitDirection, Main.rand.NextFloat(-1, 1f)), Mod.Find<ModGore>($"Gores/StarjinxEvent/Meteorite/Meteor_{Main.rand.Next(5)}").Type, Main.rand.NextFloat(.6f, 1f));
		}

		public override void OnKill()
		{
			DespawnPlatforms();

			ModContent.GetInstance<StarjinxEventWorld>().StarjinxActive = false;
			ModContent.GetInstance<StarjinxEventWorld>().StarjinxDefeated = true;
			NetMessage.SendData(MessageID.WorldData);

			int drops = Main.expertMode ? 9 : 7;
			Item.NewItem(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), Mod.Find<ModItem>("Starjinx").Type, Main.rand.Next(4, 6) * drops);

			Main.NewText("The asteroids return to their tranquil state...", 252, 150, 255);
		}

		public override bool CheckActive() => false;

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit) => CheckTakeDamage(ref damage, ref crit);
		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => CheckTakeDamage(ref damage, ref crit);

		private void CheckTakeDamage(ref int damage, ref bool crit)
		{
			if (!spawnedComets && !NPC.dontTakeDamage)
			{
				damage = 1;
				crit = false;
			}
		}
	}
}