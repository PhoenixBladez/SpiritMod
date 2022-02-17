using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.StarjinxEvent.Enemies.Pathfinder;
using SpiritMod.NPCs.StarjinxEvent.Enemies.Starachnid;
using SpiritMod.NPCs.StarjinxEvent.Enemies.StarWeaver;
using SpiritMod.Particles;
using SpiritMod.Prim;
using SpiritMod.Utilities;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent.Comets
{
	public class SmallComet : ModNPC, IDrawAdditive
	{
		public override sealed bool CloneNewInstances => true;

		public  virtual string Size => "Small";

		protected virtual float BeamScale => 0.75f;

		ref NPC Parent => ref Main.npc[(int)npc.ai[0]];
		ref float TimerOffset => ref npc.ai[1];
		ref float SpinMomentum => ref npc.ai[2];
		ref float RotationOffset => ref npc.ai[3];

		private float glowOpacity = 0f;

		public float initialDistance = 0f;
		public bool nextUp = false; //if true, spawn enemies when the player approaches next

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

			npc.hide = true; //Drawn manually by main meteor for layering and efficiency with shaders
		}

		float sinCounter;
        float sinIncrement = 0;

        public override void AI()
        {
			npc.TargetClosest(false);

            if (npc.alpha > 0)
                npc.alpha -= 7;
            else
                npc.alpha = 0;

			Player nearest = Main.player[npc.target];

			if (nearest.active && !nearest.dead && nextUp && !spawnedEnemies)
			{
				nextUp = false;
				spawnedEnemies = true;
				SpawnWave();
			}

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
				RotationOffset += SpinMomentum * (float)Math.Pow(50 / initialDistance, 1.25f);
			}

            if (sinIncrement == 0) 
				sinIncrement = Main.rand.NextFloat(0.025f, 0.035f);

            sinCounter += sinIncrement;

			if (spawnedEnemies)
			{
				bool tempProjectile = Main.projectile.Any(x => x.active && x.type == ModContent.ProjectileType<StarjinxEnemySpawner>());
				bool sjinxEnemyAlive = false;

				for (int i = 0; i < Main.maxNPCs; ++i) //make sure an sjinx enemy is alive
				{
					NPC npc = Main.npc[i];
					if (npc.active && npc.life > 0 && npc.modNPC != null && (npc.modNPC is IStarjinxEnemy || npc.type == ModContent.NPCType<Pathfinder>()))
					{
						sjinxEnemyAlive = true;
						break;
					}
				}

				if (!tempProjectile && !sjinxEnemyAlive) //if all enemies are dead, make me vulnerable
					npc.dontTakeDamage = false;
			}

			npc.rotation = (float)Math.Sin(sinCounter) / 15;
			if (!npc.dontTakeDamage)
			{
				glowOpacity = MathHelper.Lerp(glowOpacity, 0f, 0.05f);
				SpinMomentum = MathHelper.Lerp(SpinMomentum, 0.04f, 0.05f);

				StarjinxPlayer localSjinxPlayer = Main.LocalPlayer.GetModPlayer<StarjinxPlayer>();
				VignettePlayer localVignettePlayer = Main.LocalPlayer.GetModPlayer<VignettePlayer>();
				if (localSjinxPlayer.zoneStarjinxEvent)
					localVignettePlayer.SetVignette(0, 1000, 0.75f, Color.Black, npc.Center);
			}
			else
			{
				glowOpacity = MathHelper.Lerp(glowOpacity, 1f, 0.05f);
				SpinMomentum = Math.Min(SpinMomentum + 0.002f, 0.08f);
			}

            if (!Parent.active || Parent.type != ModContent.NPCType<StarjinxMeteorite>()) //kill me if the main meteor is dead somehow
                npc.active = false;

			for (int i = 0; i < Main.maxPlayers; ++i)
			{
				Player p = Main.player[i];
				if (p.active && !p.dead && p.GetModPlayer<StarjinxPlayer>().zoneStarjinxEvent) //There is a player nearby and we don't need to reset
					return;
			}
        }

		public virtual void SpawnWave()
		{
			int choice = Main.rand.Next(4);
			int numEnemies;
			switch (choice)
			{
				case 0: //4 random starachnid/starweaver
					numEnemies = 4;
					for (int i = 0; i < 4; ++i)
						SpawnSpawnerProjectile(Main.rand.NextBool(2) ? ModContent.NPCType<Starachnid>() : ModContent.NPCType<StarWeaverNPC>());
					break;
				case 1: //fan of 3 starweavers
					numEnemies = 3;
					for (int i = -1; i < 2; ++i)
					{
						float rotation = i * (MathHelper.Pi / 3);
						SpawnSpawnerProjectile(ModContent.NPCType<StarWeaverNPC>(), (Vector2.UnitY * 200).RotatedBy(rotation));
					}
					break;
				default: //2 starachnid, 1 pathfinder
					numEnemies = 3;
					for (int i = 0; i < 2; ++i)
						SpawnSpawnerProjectile(ModContent.NPCType<Starachnid>());
					SpawnSpawnerProjectile(ModContent.NPCType<Pathfinder>());
					break;
			}

			StarjinxEventWorld.SetMaxEnemies(numEnemies);
		}

		/// <summary>Spawns an NPC that is attached to this comet with an automatic (and overrideable) offset.</summary>
		/// <param name="type">NPCID to spawn.</param>
		/// <param name="overrideOffset">If not null, this is the offset used to spawn, based off of the origin npc.Center.</param>
		/// <param name="rawPosition">Completely overrides position, making overrideOffset simply equal the position.</param>
		/// <returns>The spawn offset used.</returns>
		internal Vector2 SpawnSpawnerProjectile(int type, Vector2? overrideOffset = null, bool rawPosition = false)
		{
			if (!overrideOffset.HasValue && rawPosition) //Trying to use a null position
			{
				Main.NewText("Trying to use a raw position without using the overrideOffset parameter. Report to devs.", Color.Red);
				mod.Logger.Error("Invalid raw position/overrideOffset combination (SmallComet.SpawnSpawnerProjectile)", new ArgumentException("Bad args for SmallComet.SpawnSpawnerProjectile(int, Vector2?, bool):\n"));
				return Vector2.Zero;
			}

			const int MaxOffset = 300;

			Vector2 originalSpawn = npc.Center + new Vector2(Main.rand.Next(-MaxOffset, MaxOffset), Main.rand.Next(-MaxOffset, MaxOffset));

			if (overrideOffset.HasValue)
			{
				if (!rawPosition)
					originalSpawn = npc.Center + overrideOffset.Value;
				else
					originalSpawn = overrideOffset.Value;
			}

			int attempts = 0;

			while (true)
			{
				if (attempts++ > 200)
				{
					Main.NewText("Failed to spawn Starjinx enemy. Report to devs.", Color.Red);
					break;
				}

				Vector2 spawnPos = originalSpawn;

				if (attempts > 100) //randomize position in order to find valid spawn if we're having trouble finding a good place
				{
					float magnitude = (attempts - 50) / 50f;
					spawnPos += new Vector2(Main.rand.Next(-200, 200), Main.rand.Next(-200, 200)) * magnitude;
				}

				var temp = new NPC(); //so I can get the size of the NPC without hardcoding
				temp.SetDefaults(type);

				if (!Collision.SolidCollision(spawnPos - (new Vector2(temp.width, temp.height) / 2f), temp.width * 2, temp.height * 2))
				{
					var p = Projectile.NewProjectileDirect(spawnPos, Vector2.Zero, ModContent.ProjectileType<StarjinxEnemySpawner>(), 0, 0);

					if (p.modProjectile != null && p.modProjectile is StarjinxEnemySpawner spawner)
					{
						spawner.enemyToSpawn = type;
						spawner.spawnPosition = spawnPos;
					}

					originalSpawn = spawnPos;
					break;
				}
			}

			if (rawPosition)
				return overrideOffset.Value;

			return originalSpawn - npc.Center;
		}

		public override void HitEffect(int hitDirection, double damage)
        {
			if (!Main.dedServ)
				for (int i = 0; i < 8; i++)
					ParticleHandler.SpawnParticle(new StarParticle(npc.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(6f), SpiritMod.StarjinxColor(Main.rand.Next(10)), Main.rand.NextFloat(0.1f, 0.2f), 20));

			SpinMomentum *= 0.5f;
			npc.netUpdate = true;

            if (npc.life <= 0)
                Main.PlaySound(SoundID.Item14, npc.position);

			if (npc.life <= 0)
				for (int k = 0; k < npc.lifeMax / 60; k++)
					Gore.NewGore(npc.Center + new Vector2(0, Main.rand.NextFloat(npc.width / 2f)).RotatedByRandom(MathHelper.Pi), new Vector2(2 * hitDirection, Main.rand.NextFloat(-0.5f, 0.5f)), mod.GetGoreSlot($"Gores/StarjinxEvent/Meteorite/Meteor_{Main.rand.Next(5)}"), Main.rand.NextFloat(.6f, 1f));
		}

        public float Timer => Main.GlobalTime + TimerOffset; //Used to offset the beam/sine wave motion

		public float DeathGlowStrength
		{
			get
			{
				float strength = 1 - (npc.life / (float)npc.lifeMax);
				strength = EaseFunction.EaseQuadInOut.Ease(strength);
				strength *= (float)(Math.Sin(Main.GlobalTime * 6) * 0.1f) + 1;
				strength *= 0.6f;
				return strength;
			}
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            var center = new Vector2(Main.npcTexture[npc.type].Width / 2, Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2);
            float cos = (float)Math.Cos(((Timer % 3.2f) / 3.2f) * MathHelper.TwoPi) / 2f + 0.5f;

			Color baseCol = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Color.White);
            Color col = npc.GetAlpha(baseCol) * 0.6f;

			SpriteEffects effects = npc.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, null, npc.GetAlpha(Color.Lerp(drawColor, Color.White, 0.5f)), npc.rotation, center, npc.scale, effects, 0f);
			spriteBatch.Draw(ModContent.GetTexture($"SpiritMod/NPCs/StarjinxEvent/Comets/{Size}CometGlow"), npc.Center - Main.screenPosition, null, col * glowOpacity, npc.rotation, center, npc.scale, effects, 0f);
            for (int i = 0; i < 6; i++)
            {
                var drawPos = npc.Center + ((i / 6f) * MathHelper.TwoPi + npc.rotation).ToRotationVector2() * (4f * cos + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * i;
				spriteBatch.Draw(ModContent.GetTexture($"SpiritMod/NPCs/StarjinxEvent/Comets/{Size}CometGlow"), drawPos, npc.frame, col * glowOpacity * (1f - cos), npc.rotation, npc.frame.Size() / 2f, npc.scale, effects, 0f);
            }

			return false;
		}

		public void AdditiveCall(SpriteBatch sB)
		{
			Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 scale = texture.Size() / bloom.Size();
			scale *= 3;

			sB.Draw(bloom, npc.Center - Main.screenPosition, null, Color.White * DeathGlowStrength, 0, bloom.Size() / 2, scale, SpriteEffects.None, 0);

			//Dont draw blur line if not vulnerable, to reduce unnecessary shader applications
			if (!npc.dontTakeDamage)
			{
				float blurLength = MathHelper.Lerp(texture.Width * 3, texture.Width * 4, DeathGlowStrength);
				float blurWidth = MathHelper.Lerp(texture.Height / 5, texture.Height / 4, DeathGlowStrength);

				Effect blurEffect = mod.GetEffect("Effects/BlurLine");
				var blurLine = new SquarePrimitive()
				{
					Position = npc.Center - Main.screenPosition,
					Height = blurWidth,
					Length = blurLength,
					Rotation = npc.rotation,
					Color = Color.White * DeathGlowStrength * 0.75f
				};

				PrimitiveRenderer.DrawPrimitiveShape(blurLine, blurEffect);
			}

			SpriteEffects effects = npc.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			sB.Draw(ModContent.GetTexture($"SpiritMod/NPCs/StarjinxEvent/Comets/{Size}Comet_mask"), npc.Center - Main.screenPosition, null, Color.White * DeathGlowStrength, npc.rotation, texture.Size()/2, npc.scale, effects, 0f);
		}

		private Color EnergyColor => Color.Lerp(SpiritMod.StarjinxColor(Timer * 0.8f), Color.White, 0.33f);

		private float CosTimer => (float)Math.Cos(((Timer % 3.2f) / 3.2f) * MathHelper.TwoPi) / 2f + 0.5f;

		public void DrawRing()
		{
			Effect effect = mod.GetEffect("Effects/SjinxRing");
			float width = 70f * (((1 - CosTimer) * 0.25f) + 0.75f);
			float size = (npc.Distance(Parent.Center) * 2) + width;
			effect.Parameters["Size"].SetValue(size);
			effect.Parameters["RingWidth"].SetValue(width);
			effect.Parameters["Angle"].SetValue(npc.AngleTo(Parent.Center));
			effect.Parameters["FadeAngleRange"].SetValue(MathHelper.Lerp(1.25f * MathHelper.PiOver2, MathHelper.Pi / 24, npc.Distance(Parent.Center) / 500));

			var square = new Prim.SquarePrimitive
			{
				Color =  EnergyColor *
					glowOpacity *
					npc.Opacity *
					(0.2f * (((1f - CosTimer) * 0.5f) + 0.25f)),
				Height = size,
				Length = size,
				Position = Parent.Center - Main.screenPosition,
				ColorXCoordMod = 1
			};
			PrimitiveRenderer.DrawPrimitiveShape(square, effect);
		}

		public void DrawBeam(SpriteBatch b)
		{
			Texture2D beam = mod.GetTexture("Textures/Ray");
			float rotation = npc.DirectionTo(Main.npc[(int)npc.ai[0]].Center).ToRotation() - MathHelper.PiOver2;
			float fluctuate = (CosTimer / 10f) + 0.15f;

			Color color = Color.Lerp(EnergyColor, Color.Transparent, fluctuate * 2) * ((glowOpacity * 0.75f) + 0.25f) * npc.Opacity * 0.2f;

			Rectangle rect = new Rectangle(0, 0, beam.Width, beam.Height);
			Vector2 scale = new Vector2(1, (1 - fluctuate) * ((glowOpacity * 0.5f) + 0.5f) * npc.Distance(Main.npc[(int)npc.ai[0]].Center) / beam.Height) * BeamScale;
			Vector2 offset = new Vector2(0, 100 * scale.Y).RotatedBy(rotation);
			b.Draw(beam, npc.Center - Main.screenPosition + offset / 2, new Rectangle?(rect), npc.GetAlpha(color), rotation, rect.Size() / 2, scale, SpriteEffects.None, 0);
		}

		public void DrawShield(SpriteBatch b)
		{
			Texture2D mask = mod.GetTexture("Effects/Masks/CircleGradient");

			Texture2D mainTex = Main.npcTexture[npc.type];
			float averageSize = mainTex.Width / 2f + mainTex.Height / 2f;
			float scale = averageSize / mask.Width;

			scale *= MathHelper.Lerp(3.2f, 9.6f, 1 - glowOpacity) * (CosTimer/15f + 1f);

			b.Draw(mask, npc.Center - Main.screenPosition, null, EnergyColor * glowOpacity, npc.rotation + Timer * 2f, mask.Size() / 2f, scale, SpriteEffects.None, 0);
		}

		public override bool CheckDead()
		{
			(Parent.modNPC as StarjinxMeteorite).updateCometOrder = true;

			spawnedEnemies = false;
			return true;
		}

		public override bool CheckActive() => !Parent.active || Parent.type != ModContent.NPCType<StarjinxMeteorite>();
	}
}