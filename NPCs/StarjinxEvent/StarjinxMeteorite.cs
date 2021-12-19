using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
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
            NPCID.Sets.TrailCacheLength[npc.type] = 20;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 2000;
            npc.defense = 20;
            npc.value = 0f;
            npc.dontTakeDamage = false;
            npc.knockBackResist = 0f;
            npc.width = 60;
            npc.height = 60;
            npc.damage = 0;
            npc.lavaImmune = false;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.Item89;
            npc.alpha = 255;
			npc.chaseable = false;
            aiType = 0;
			for (int i = 0; i < BuffLoader.BuffCount; i++)
				npc.buffImmune[i] = true;

			//Draw on a layer before all other npcs, to help with visual clarity
			npc.behindTiles = true;
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
            npc.velocity.X = 0;
            npc.gfxOffY = (float)Math.Sin(sinCounter) * 6f;
            sinCounter += 0.03f;

            if (npc.alpha > 0)
                npc.alpha -= 10;
            else
                npc.alpha = 0;

			if (spawnedComets && comets.Count <= 0) //once all small comets have been defeated
			{
				if (spawnedBoss)
				{
					if (bossWhoAmI != -1)
					{
						musicVolume = Math.Min(musicVolume + 0.0125f, 1f);
						NPC boss = Main.npc[bossWhoAmI];
						if (!boss.active || !boss.boss)
							bossWhoAmI = -1;
					}
					else
					{
						npc.dontTakeDamage = false;
						shieldOpacity = MathHelper.Lerp(shieldOpacity, 0, 0.05f);
						musicVolume = Math.Max(musicVolume - 0.0125f, 0); //Fully fade out music when event is done
					}
				}
				else
				{
					int boss = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 400, NPCID.EyeofCthulhu);

					StarjinxEventWorld.SetMaxEnemies(1); //Change to 2 when actual boss fight is added
					StarjinxEventWorld.SetComets(comets.Count);

					spawnedBoss = true;
					bossWhoAmI = boss;
				}
			}

			if(spawnedComets)
			{
				for (int i = 0; i < Main.maxPlayers; i++) //Check if players are in range
				{
					Player player = Main.player[i];
					if (player.active && !player.dead)
					{
						if (player.DistanceSQ(npc.Center) < EVENT_RADIUS * EVENT_RADIUS)
						{
							player.GetModPlayer<StarjinxPlayer>().zoneStarjinxEvent = true;
							player.GetModPlayer<StarjinxPlayer>().StarjinxPosition = npc.Center;
						}
						else if (player.GetModPlayer<StarjinxPlayer>().oldZoneStarjinx) //cache all meteors if need be
						{
							savedComets["Small"] = savedComets["Medium"] = savedComets["Large"] = -1; //So we "save" if all of them are dead

							IterateComets((SmallComet comet) =>
							{
								savedComets[comet.Size]++;
								comet.npc.active = false;
							});

							spawnedComets = false;
							comets.Clear();

							npc.life = npc.lifeMax; //Reset the meteor completely
							npc.dontTakeDamage = false;

							shieldOpacity = 0f;
						}
					}
				}

				if(npc.dontTakeDamage && comets.Count > 0) //Child meteor active checks
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
							if (!comet.active || comet.modNPC == null || !(comet.modNPC is SmallComet))
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

							if (comet.active && comet.modNPC is SmallComet npc && furthest.modNPC is SmallComet far && npc.initialDistance > far.initialDistance)
								furthest = comet;
						}

						for (int i = 0; i < comets.Count; i++)
						{
							NPC comet = Main.npc[comets[i]];
							if (comet.active && comet.modNPC is SmallComet)
								comet.dontTakeDamage = true;
							if (comet.whoAmI == furthest.whoAmI)
								(comet.modNPC as SmallComet).nextUp = true;
						}

						npc.netUpdate = true;
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

			if (!spawnedComets && npc.life < npc.lifeMax) //Spawn meteors below max health (when I take damage) and start the event
            {
				bool validPlayer = false;

				for (int i = 0; i < Main.maxPlayers; ++i) //Search for valid player in order to start event
				{
					Player p = Main.player[i];
					if (p.active && !p.dead && p.DistanceSQ(npc.Center) < 1500 * 1500)
					{
						validPlayer = true;
						break;
					}
				}

				if (validPlayer)
				{
					spawnedComets = true;
					npc.dontTakeDamage = true;

					ModContent.GetInstance<StarjinxEventWorld>().StarjinxActive = true;

					SpawnComets();
				}
            }

			musicVolume = MathHelper.Clamp(musicVolume, 0, 1); //Just in case, been getting some debug messages about value not being valid
			Main.musicFade[Main.curMusic] = musicVolume; 
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
				Vector2 spawnPos = npc.Center + Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * FindDistance();

				int id = NPC.NewNPC((int)spawnPos.X, (int)spawnPos.Y, type, npc.whoAmI, npc.whoAmI, comets.Count * 2, 0, -1);
				Main.npc[id].Center = spawnPos;
				Main.npc[id].dontTakeDamage = true;

				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, id);

				if (comets.Count <= 0 && (savedComets["Small"] != -1 || savedComets["Medium"] != -1 || savedComets["Large"] != -1))
				{ //Kinda ugly but it makes sure the respawned comet is next up
					if (Main.npc[id].modNPC != null && Main.npc[id].modNPC is SmallComet comet)
						comet.nextUp = true;
				}

				comets.Add(id);
				npc.netUpdate = true;
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
				if (Main.npc[ID].modNPC != null)
					if (Main.npc[ID].modNPC is SmallComet comet)
						action.Invoke(comet);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			IterateComets(delegate (SmallComet comet) //Draw rings and beams underneath everything else
			{
				comet.DrawBeam(spriteBatch);
				comet.DrawRing();
			});

			IterateComets(delegate (SmallComet comet) //Then draw base textures
			{
				comet.PreDraw(spriteBatch, Lighting.GetColor((int)comet.npc.position.X / 16, (int)comet.npc.position.Y / 16));
			});
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
			var center = new Vector2((Main.npcTexture[npc.type].Width / 2), (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
            float sineAdd = (float)Math.Sin(sinCounter * 1.5f);
			Vector2 drawCenter = npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY);

			//Weird shader stuff, dont touch yuyutsu
			#region shader
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Vector4 colorMod = Color.LightGoldenrodYellow.ToVector4();
            SpiritMod.StarjinxNoise.Parameters["distance"].SetValue(2.9f - (sineAdd / 10));
			SpiritMod.StarjinxNoise.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.StarjinxNoise.Parameters["noise"].SetValue(mod.GetTexture("Textures/noise"));
			SpiritMod.StarjinxNoise.Parameters["rotation"].SetValue(sinCounter / 5);
			SpiritMod.StarjinxNoise.Parameters["opacity2"].SetValue(0.3f - (sineAdd / 10));
			SpiritMod.StarjinxNoise.CurrentTechnique.Passes[0].Apply();
			spriteBatch.Draw(mod.GetTexture("Effects/Masks/CircleGradient"), drawCenter, null, npc.GetAlpha(Color.White), 0f, new Vector2(125, 125), 1.3f - (sineAdd / 9), SpriteEffects.None, 0f);

			SpiritMod.StarjinxNoise.Parameters["opacity2"].SetValue(1);
			SpiritMod.StarjinxNoise.Parameters["rotation"].SetValue((sinCounter + 3) / 4); 
			colorMod *= 0.66f;
			SpiritMod.StarjinxNoise.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.StarjinxNoise.CurrentTechnique.Passes[0].Apply();

			spriteBatch.Draw(mod.GetTexture("Effects/Masks/CircleGradient"), drawCenter, null, npc.GetAlpha(Color.White), 0f, new Vector2(125, 125), 1.1f + (sineAdd / 7), SpriteEffects.None, 0f);

            Main.spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			#endregion

			spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/StarjinxMeteorite"), drawCenter, null, Color.White, 0f, center, 1, SpriteEffects.None, 0f);

            float cos = (float)Math.Cos((Main.GlobalTime % 2.4f / 2.4f * MathHelper.TwoPi)) / 2f + 0.5f;

            SpriteEffects spriteEffects3 = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Color baseCol = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Color.White);
			Color drawCol = npc.GetAlpha(baseCol) * (1f - cos);

			spriteBatch.Draw(Main.npcTexture[npc.type], drawCenter, null, npc.GetAlpha(Color.Lerp(drawColor, Color.White, 0.5f)), 0f, center, 1, SpriteEffects.None, 0f);
			spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/StarjinxMeteoriteGlow"), drawCenter, null, npc.GetAlpha(Color.White * .4f), 0f, center, 1, SpriteEffects.None, 0f);

            for (int i = 0; i < 6; i++)
            {
                Vector2 drawPos = drawCenter + ((i / 6f) * MathHelper.TwoPi + npc.rotation).ToRotationVector2() * (4f * cos + 2f) - npc.velocity * i;
				spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/StarjinxMeteoriteGlow"), drawPos, npc.frame, drawCol, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
				//spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/StarjinxMeteoriteGlowOutline"), drawPos, npc.frame, drawCol, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
            }

			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			SpiritMod.Instance.GetEffect("Effects/SjinxShield").Parameters["vnoiseTex"].SetValue(mod.GetTexture("Textures/voronoiLooping"));
			SpiritMod.Instance.GetEffect("Effects/SjinxShield").Parameters["timer"].SetValue(Main.GlobalTime / 3);
			SpiritMod.Instance.GetEffect("Effects/SjinxShield").CurrentTechnique.Passes[0].Apply();
			IterateComets(delegate (SmallComet comet)
			{
				comet.DrawShield(Main.spriteBatch);
			});

			Texture2D mask = mod.GetTexture("Effects/Masks/CircleGradient");

			Texture2D mainTex = Main.npcTexture[npc.type];
			float averageSize = mainTex.Width / 2f + mainTex.Height / 2f;
			float scale = averageSize / mask.Width;

			scale *= MathHelper.Lerp(3f, 6f, 1 - shieldOpacity) * (sineAdd / 15f + 1f);

			spriteBatch.Draw(mask, drawCenter, null, Color.LightGoldenrodYellow * shieldOpacity, npc.rotation + Main.GlobalTime, mask.Size() / 2f, scale, SpriteEffects.None, 0);
			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
		}

        public override void NPCLoot()
        {
			ModContent.GetInstance<StarjinxEventWorld>().StarjinxActive = false;
			ModContent.GetInstance<StarjinxEventWorld>().StarjinxDefeated = true;
			NetMessage.SendData(MessageID.WorldData);

			int drops = Main.expertMode ? 9 : 7;
            Item.NewItem(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), mod.ItemType("Starjinx"), Main.rand.Next(4, 6) * drops);

            Main.NewText("The asteroids return to their tranquil state...", 252, 150, 255);
        }

		public override bool CheckActive() => false;

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit) => CheckTakeDamage(ref damage, ref crit);
		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => CheckTakeDamage(ref damage, ref crit);

		private void CheckTakeDamage(ref int damage, ref bool crit)
		{
			if (!spawnedComets && !npc.dontTakeDamage)
			{
				damage = 1;
				crit = false;
			}
		}
	}
}