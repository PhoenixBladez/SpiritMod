using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs.StarjinxEvent.Comets;
using System.Linq;

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
        int[] comets = new int[5];

		public bool updateCometOrder = true;

        public override void AI()
        {
            npc.velocity.X = 0;
            npc.velocity.Y = (float)Math.Sin(sinCounter) * 0.75f;
            sinCounter += 0.03f;

            if (npc.alpha > 0)
                npc.alpha -= 10;
            else
                npc.alpha = 0;

			if (spawnedComets && npc.dontTakeDamage) //Child meteor active checks
			{
				for (int i = 0; i < comets.Length; i++)
				{
					NPC comet = Main.npc[comets[i]];
					int[] cometTypes = new int[] { ModContent.NPCType<LargeComet>(), ModContent.NPCType<SmallComet>(), ModContent.NPCType<MediumComet>() };
					if (comet.active && comet.life > 0 && cometTypes.Contains(comet.type))
						break;
					if (i == comets.Length - 1)
						npc.dontTakeDamage = false;
				}

				if (updateCometOrder)
				{
					NPC furthest = Main.npc[comets[0]];
					for (int i = 0; i < comets.Length; i++)
					{
						NPC comet = Main.npc[comets[i]];

						if (comet.active && comet.modNPC is SmallComet npc)
						{
							if (!furthest.active)
								furthest = comet;
							var far = furthest.modNPC as SmallComet;
							if (npc.initialDistance > far.initialDistance)
								furthest = comet;
						}
					}

					for (int i = 0; i < comets.Length; i++)
					{
						NPC comet = Main.npc[comets[i]];
						if (comet.active)
							comet.dontTakeDamage = comet.whoAmI != furthest.whoAmI;
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

				for (int i = 0; i < 2; i++)
                {
                    int direction = i == 1 ? 1 : -1;
                    float x = npc.Center.X - 150 * direction;
                    float y = npc.Center.Y + (npc.height / 2) + Main.rand.Next(-300, 100);
                    comets[i] = NPC.NewNPC((int)x, (int)y, ModContent.NPCType<SmallComet>(), 0, npc.whoAmI, Main.rand.NextFloat(10), 0, 1000);
					Main.npc[comets[i]].dontTakeDamage = true;
                }
                for (int i = 0; i < 2; i++)
                {
                    int direction = i == 1 ? 1 : -1;
                    float x = npc.Center.X - 300 * direction;
                    float y = npc.Center.Y + (npc.height / 2) + Main.rand.Next(-300, 100);
                    comets[i + 2] = NPC.NewNPC((int)x, (int)y, ModContent.NPCType<MediumComet>(), 0, npc.whoAmI, Main.rand.NextFloat(10), 0, 1000);
					Main.npc[comets[i]].dontTakeDamage = true;
				}
                for (int i = 0; i < 1; i++)
                {
                    float x = npc.Center.X - Main.rand.Next(Main.rand.Next(-700, -500), Main.rand.Next(500, 700));
                    float y = npc.Center.Y + (npc.height / 2f) + Main.rand.Next(-300, 100);
                    comets[i + 4] = NPC.NewNPC((int)x, (int)y, ModContent.NPCType<LargeComet>(), 0, npc.whoAmI, Main.rand.NextFloat(10), 0, 1000);
					Main.npc[comets[i]].dontTakeDamage = true;
				}
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

            float cos = (float)Math.Cos((Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;

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