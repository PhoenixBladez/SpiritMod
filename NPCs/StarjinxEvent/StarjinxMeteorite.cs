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
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starjinx");
            NPCID.Sets.TrailCacheLength[npc.type] = 20;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 8000;
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

            aiType = 0;
		}

		float sinCounter;
        bool spawnedComets = false;

		List<int> comets = new List<int>();

		public bool updateCometOrder = true;
		public bool spawnedBoss = false;
		public int bossWhoAmI = -1;

        public override void AI()
        {
            npc.velocity.X = 0;
            npc.velocity.Y = (float)Math.Sin(sinCounter) * 0.75f;
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
						NPC boss = Main.npc[bossWhoAmI];
						if (!boss.active || !boss.boss)
							bossWhoAmI = -1;
					}
					else
						npc.dontTakeDamage = false;
				}
				else
				{
					int boss = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 400, NPCID.EyeofCthulhu);

					spawnedBoss = true;
					bossWhoAmI = boss;
				}
			}

			if (spawnedComets && npc.dontTakeDamage && comets.Count > 0) //Child meteor active checks
			{
				for (int i = 0; i < comets.Count; i++)
				{
					NPC comet = Main.npc[comets[i]];
					int[] cometTypes = new int[] { ModContent.NPCType<LargeComet>(), ModContent.NPCType<SmallComet>(), ModContent.NPCType<MediumComet>() };
					if (comet.active && comet.life > 0 && cometTypes.Contains(comet.type))
						break;
					//if (i == comets.Count - 1)
					//	npc.dontTakeDamage = false;
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
							(comet.modNPC as SmallComet).SpawnWave();
					}

					npc.netUpdate = true;
					updateCometOrder = false;
				}
			}

			if (!spawnedComets && npc.life < npc.lifeMax) //Spawn meteors below max health (when I take damage) and start the event
            {
                spawnedComets = true;
				npc.dontTakeDamage = true;
				StarjinxEventWorld.StarjinxActive = true;

				SpawnComets();
            }
        }

		private void SpawnComets()
		{
			for (int i = 0; i < 3; i++)
			{
				float x = npc.Center.X - Main.rand.Next(150, 250) * (i == 1 ? 1 : -1);
				float y = npc.Bottom.Y + (Main.rand.Next(200, 300) * (Main.rand.NextBool(2) ? -1 : 1));
				int id = NPC.NewNPC((int)x, (int)y, ModContent.NPCType<SmallComet>(), 0, npc.whoAmI, Main.rand.NextFloat(10), 0, -1);
				Main.npc[id].dontTakeDamage = true;
				comets.Add(id);
			}

			for (int i = 0; i < 2; i++)
			{
				float x = npc.Center.X - Main.rand.Next(100, 150) * (i == 1 ? 1 : -1);
				float y = npc.Bottom.Y + (Main.rand.Next(125, 200) * (Main.rand.NextBool(2) ? -1 : 1));
				int id = NPC.NewNPC((int)x, (int)y, ModContent.NPCType<MediumComet>(), 0, npc.whoAmI, Main.rand.NextFloat(10), 0, -1);
				Main.npc[id].dontTakeDamage = true;
				comets.Add(id);
			}

			int maxLargeComets = Main.expertMode ? 3 : 1;
			for (int i = 0; i < maxLargeComets; i++)
			{
				float x = npc.Center.X + (Main.rand.Next(75, 100) * (Main.rand.NextBool(2) ? -1 : 1));
				float y = npc.Bottom.Y + (Main.rand.Next(50, 125) * (Main.rand.NextBool(2) ? -1 : 1));
				int id = NPC.NewNPC((int)x, (int)y, ModContent.NPCType<LargeComet>(), 0, npc.whoAmI, Main.rand.NextFloat(10), 0, -1);
				Main.npc[id].dontTakeDamage = true;
				comets.Add(id);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) => false;

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
			var center = new Vector2((Main.npcTexture[npc.type].Width / 2), (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
            float sineAdd = (float)Math.Sin(sinCounter * 1.33f);

            //Weird shader stuff, dont touch yuyutsu
            #region shader
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Vector4 yellow = new Vector4(1f, 0.89f, 0.63f, 1f);
            Vector4 pink = new Vector4(0.95f, 0.45f, 0.78f, 1f);
            Vector4 colorMod = Vector4.Lerp(yellow, pink, 0.5f - (sineAdd / 2));
            SpiritMod.StarjinxNoise.Parameters["distance"].SetValue(2.9f - (sineAdd / 10));
			SpiritMod.StarjinxNoise.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.StarjinxNoise.Parameters["noise"].SetValue(mod.GetTexture("Textures/noise"));
			SpiritMod.StarjinxNoise.Parameters["rotation"].SetValue(sinCounter / 5);
			SpiritMod.StarjinxNoise.Parameters["opacity2"].SetValue(0.3f - (sineAdd / 10));
			SpiritMod.StarjinxNoise.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition), null, npc.GetAlpha(Color.White), 0f, new Vector2(50, 50), 2.1f - (sineAdd / 9), SpriteEffects.None, 0f);

			SpiritMod.StarjinxNoise.Parameters["opacity2"].SetValue(1);
			SpiritMod.StarjinxNoise.Parameters["rotation"].SetValue((sinCounter + 3) / 4);
            colorMod = Vector4.Lerp(yellow, pink, 0.5f + (sineAdd / 2));
			SpiritMod.StarjinxNoise.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.StarjinxNoise.CurrentTechnique.Passes[0].Apply();

            Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition), null, npc.GetAlpha(Color.White), 0f, new Vector2(50, 50), 1.3f + (sineAdd / 7), SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            #endregion

            Main.spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/StarjinxMeteorite"), npc.Center - Main.screenPosition, null, Color.White, 0f, center, 1, SpriteEffects.None, 0f);

            float cos = (float)Math.Cos((Main.GlobalTime % 2.4f / 2.4f * MathHelper.TwoPi)) / 2f + 0.5f;

            SpriteEffects spriteEffects3 = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Color baseCol = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Color.White);
			Color drawCol = npc.GetAlpha(baseCol) * (1f - cos);

            Main.spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, null, npc.GetAlpha(Color.White), 0f, center, 1, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/StarjinxMeteoriteGlowOutline"), npc.Center - Main.screenPosition, null, npc.GetAlpha(Color.White * .4f), 0f, center, 1, SpriteEffects.None, 0f);

            for (int i = 0; i < 6; i++)
            {
                Vector2 drawPos = npc.Center + (i / 4 * MathHelper.TwoPi + npc.rotation).ToRotationVector2() * (4f * cos + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * i;
                Main.spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/StarjinxMeteoriteGlow"), drawPos, npc.frame, drawCol, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
                Main.spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/StarjinxMeteoriteGlowOutline"), drawPos, npc.frame, drawCol, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
            }
        }

        public override void NPCLoot()
        {
            StarjinxEventWorld.StarjinxActive = false;

            int drops = (Main.expertMode) ? 9 : 7;
            for (int i = 0; i < drops; i++)
                Item.NewItem(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), mod.ItemType("Starjinx"), Main.rand.Next(3, 6));

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