using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.StarjinxEvent.Enemies.Pathfinder;
using SpiritMod.NPCs.StarjinxEvent.Enemies.Starachnid;
using SpiritMod.NPCs.StarjinxEvent.Enemies.StarWeaver;
using SpiritMod.Particles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent.Comets
{
	public class SmallComet : ModNPC
	{
		public override sealed bool CloneNewInstances => true;

		protected virtual string Size => "Small";
		protected virtual float BeamScale => 0.75f;

		ref NPC Parent => ref Main.npc[(int)npc.ai[0]];
		ref float TimerOffset => ref npc.ai[1];
		ref float SpinMomentum => ref npc.ai[2];
		ref float RotationOffset => ref npc.ai[3];

		private float glowOpacity = 1f;

		public float initialDistance = 0f;

		private readonly List<int> enemies = new List<int>();
		private bool spawnedEnemies = false;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Small Starjinx Comet");

		public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 300;
            npc.defense = 0;
            npc.value = 0f;
            aiType = 0;
            npc.knockBackResist = 0f;
            npc.width = 36;
            npc.height = 38;
            npc.damage = 0;
            npc.lavaImmune = false;
            npc.noTileCollide = false;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.Item89;
            npc.alpha = 255;
			npc.direction = npc.spriteDirection = Main.rand.NextBool() ? -1 : 1;
			npc.chaseable = false;
            for (int i = 0; i < BuffLoader.BuffCount; i++)
                npc.buffImmune[i] = true;
        }

		float sinCounter;
        float sinIncrement = 0;

        public override void AI()
        {
            if (npc.alpha > 0)
                npc.alpha -= 7;
            else
                npc.alpha = 0;

            if (RotationOffset == -1) //rotation init
			{
				RotationOffset = npc.AngleTo(Parent.Center);
				initialDistance = npc.Distance(Parent.Center);
				npc.position = Parent.Center + new Vector2(0, initialDistance).RotatedBy(RotationOffset);
			}
			else
			{
				var pos = new Vector2(0, initialDistance * (float)(1 + (Math.Sin(sinCounter) * 0.02f)));
				npc.Center = Parent.Center + pos.RotatedBy(RotationOffset);
				//npc.rotation = RotationOffset;
				RotationOffset += SpinMomentum * (float)Math.Pow(50 / initialDistance, 1.25f);
			}

            if (sinIncrement == 0) 
				sinIncrement = Main.rand.NextFloat(0.025f, 0.035f);

            sinCounter += sinIncrement;
            npc.TargetClosest(true);

			if (spawnedEnemies && enemies.Count > 0)
			{
				for (int i = 0; i < enemies.Count; ++i) //update enemy list
				{
					NPC npc = Main.npc[enemies[i]];
					if ((!npc.active || npc.life <= 0) || !npc.GetGlobalNPC<StarjinxGlobalNPC>().spawnedByComet)
					{
						enemies.RemoveAt(i);
						i--;
					}
				}

				if (enemies.Count <= 0) //if all enemies are dead, make me vulnerable
					npc.dontTakeDamage = false;
			}

			npc.rotation = (float)Math.Sin(sinCounter) / 15;
			if (!npc.dontTakeDamage)
			{
				glowOpacity = MathHelper.Lerp(glowOpacity, 0f, 0.05f);
				SpinMomentum = MathHelper.Lerp(SpinMomentum, 0.04f, 0.05f);
			}
			else
				SpinMomentum = Math.Min(SpinMomentum + 0.002f, 0.08f);

            if (!Parent.active || Parent.type != ModContent.NPCType<StarjinxMeteorite>()) //kill me if the main meteor is dead somehow
                npc.active = false;
        }

		public virtual void SpawnWave()
		{
			int choice = Main.rand.Next(4);

			switch (choice)
			{
				case 0: //4 random starachnid/starweaver
					for (int i = 0; i < 4; ++i)
						SpawnValidNPC(Main.rand.NextBool(2) ? ModContent.NPCType<Starachnid>() : ModContent.NPCType<StarWeaverNPC>());
					break;
				case 1: //fan of 3 starweavers
					for (int i = -1; i < 2; ++i)
					{
						float rotation = i * (MathHelper.Pi / 3);
						SpawnValidNPC(ModContent.NPCType<StarWeaverNPC>(), (Vector2.UnitY * 200).RotatedBy(rotation));
					}
					break;

				default: //2 starachnid, 1 pathfinder
					for (int i = 0; i < 2; ++i)
						SpawnValidNPC(ModContent.NPCType<Starachnid>());
					SpawnValidNPC(ModContent.NPCType<Pathfinder>());
					break;
			}
		}

		/// <summary>Spawns an NPC that is attached to this comet with an automatic (and overrideable) offset.</summary>
		/// <param name="type">NPCID to spawn.</param>
		/// <param name="overrideOffset">If not null, this is the offset used to spawn, based off of the origin npc.Center.</param>
		/// <returns>The spawn offset used.</returns>
		internal Vector2 SpawnValidNPC(int type, Vector2? overrideOffset = null)
		{
			int X = (int)(npc.Center.X + Main.rand.Next(-500, 500));
			int Y = (int)(npc.Center.Y + Main.rand.Next(-500, 500));

			if (overrideOffset.HasValue)
			{
				X = (int)(npc.Center.X + overrideOffset.Value.X);
				Y = (int)(npc.Center.Y + overrideOffset.Value.Y);
			}

			bool success = false;
			int attempts = 0;

			while (!success)
			{
				if (attempts++ > 200)
				{
					Main.NewText("Failed to spawn Starjinx enemies. Report to devs.", Color.Red);
					break;
				}

				var spawnPos = new Vector2(X, Y);

				if (attempts > 100)
				{
					float magnitude = attempts / 100f;
					spawnPos += new Vector2(Main.rand.Next(-100, 100), Main.rand.Next(-100, 100)) * magnitude;
				}

				var temp = new NPC(); //so I can get the size of the NPC without hardcoding
				temp.SetDefaults(type);

				if (!Collision.SolidCollision(spawnPos, temp.width, temp.height))
				{
					success = true;

					int id = NPC.NewNPC((int)spawnPos.X, (int)spawnPos.Y, type);
					NPC n = Main.npc[id];
					n.GetGlobalNPC<StarjinxGlobalNPC>().spawnedByComet = true;

					enemies.Add(id);
					spawnedEnemies = true;

					if (Main.netMode == NetmodeID.MultiplayerClient)
						NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, id);

					//add spawn vfx here
				}
			}
			return new Vector2(X, Y) - npc.Center;
		}

		public override void HitEffect(int hitDirection, double damage)
        {
			if (!Main.dedServ)
				for (int i = 0; i < 8; i++)
					ParticleHandler.SpawnParticle(new StarParticle(npc.Center, 
						Main.rand.NextVector2Unit() * Main.rand.NextFloat(6f), 
						SpiritMod.StarjinxColor(Main.rand.Next(10)),
						Main.rand.NextFloat(0.1f, 0.2f), 20));

			SpinMomentum *= 0.5f;
			npc.netUpdate = true;

            if (npc.life <= 0)
                Main.PlaySound(SoundID.Item14, npc.position);
        }

        public float Timer => Main.GlobalTime + TimerOffset; //Used to offset the beam/sine wave motion

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            var center = new Vector2(Main.npcTexture[npc.type].Width / 2, (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
            float cos = (float)Math.Cos(((Timer % 3.2f) / 3.2f) * MathHelper.TwoPi) / 2f + 0.5f;

			DrawBeam(spriteBatch);

			Effect effect = SpiritMod.ShaderDict["SjinxRing"];
			float size = npc.Distance(Parent.Center) * 2;
			effect.Parameters["Size"].SetValue(size);
			effect.Parameters["RingWidth"].SetValue(2.5f * (((1 - cos) * 0.5f) + 0.5f));
			effect.Parameters["Angle"].SetValue(npc.AngleTo(Parent.Center));
			var square = new Prim.SquarePrimitive
			{
				Color = Color.Lerp(SpiritMod.StarjinxColor(Timer * 0.8f), Color.White, 0.66f) * 
					glowOpacity * 
					npc.Opacity * 
					(0.5f * (((1f - cos) * 0.75f) + 0.25f)),
				Height = size,
				Length = size,
				Position = Parent.Center - Main.screenPosition,
				ColorXCoordMod = 1
			};
			Prim.PrimitiveRenderer.DrawPrimitiveShape(square, effect);

			Color baseCol = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Color.White);
            Color col = npc.GetAlpha(baseCol) * 0.6f;

			SpriteEffects effects = npc.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, null, npc.GetAlpha(Color.Lerp(drawColor, Color.White, 0.5f)), npc.rotation, center, 1, effects, 0f);
			spriteBatch.Draw(ModContent.GetTexture($"SpiritMod/NPCs/StarjinxEvent/Comets/{Size}CometGlow"), npc.Center - Main.screenPosition, null, col * glowOpacity, npc.rotation, center, 1, effects, 0f);
            for (int i = 0; i < 6; i++)
            {
                var drawPos = npc.Center + ((i / 6f) * MathHelper.TwoPi + npc.rotation).ToRotationVector2() * (4f * cos + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * i;
				spriteBatch.Draw(ModContent.GetTexture($"SpiritMod/NPCs/StarjinxEvent/Comets/{Size}CometGlow"), drawPos, npc.frame, col * glowOpacity * (1f - cos), npc.rotation, npc.frame.Size() / 2f, npc.scale, effects, 0f);
            }
			return false;
		}

		private void DrawBeam(SpriteBatch b)
		{
			Texture2D beam = mod.GetTexture("Textures/Medusa_Ray");
			float rotation = npc.DirectionTo(Main.npc[(int)npc.ai[0]].Center).ToRotation();
			float fluctuate = (float)Math.Sin(Timer * 1.2f) / 6 + 0.125f;

			Color color = SpiritMod.StarjinxColor(Timer * 0.8f);
			color = Color.Lerp(color, Color.Transparent, fluctuate * 2) * ((glowOpacity * 0.75f) + 0.25f) * npc.Opacity;

			Rectangle rect = new Rectangle(0, 0, beam.Width, beam.Height);
			Vector2 scale = new Vector2((1 - fluctuate) * ((glowOpacity * 0.5f) + 0.5f) * npc.Distance(Main.npc[(int)npc.ai[0]].Center) / beam.Width, 1) * BeamScale;
			Vector2 offset = new Vector2(100 * scale.X, 0).RotatedBy(rotation);
			b.Draw(beam, npc.Center - Main.screenPosition + offset / 2, new Rectangle?(rect), npc.GetAlpha(color), rotation, rect.Size() / 2, scale, SpriteEffects.None, 0);
		}

		public override bool CheckDead()
		{
			(Parent.modNPC as StarjinxMeteorite).updateCometOrder = true;

			enemies.Clear();
			spawnedEnemies = false;
			return true;
		}

		public override bool CheckActive() => !Parent.active || Parent.type != ModContent.NPCType<StarjinxMeteorite>();
	}
}