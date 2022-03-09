using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.Utilities;
using SpiritMod.NPCs.StarjinxEvent.Enemies.Pathfinder;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Starachnid
{
	public class SubStar //Substars are stars that draw along threads and quickly fade
	{
		public int Counter; //Counter to keep track of time
		public float MaxScale; //Maximum size they can reach

		public Vector2 Position; //Position of the substar

		public SubStar(float maxScale, Vector2 position)
		{
			Counter = 0;
			MaxScale = maxScale;
			Position = position;
		}

		public void Update() => Counter++;
	}

	public class StarThread
	{
		public Vector2 StartPoint;
		public Vector2 EndPoint;

		public int Counter;
		public int Duration = 800; //How long the thread lasts
		public int Length; //Length of the thread

		public float StartScale = 1; //Scale of start point star
		public float EndScale = 1; //Scale of end point star

		public bool DrawStart = true;

		public List<SubStar> SubStars = new List<SubStar>(); //list of stars that twinkle along the string

		public StarThread(Vector2 startPoint, Vector2 endPoint)
		{
			StartPoint = startPoint;
			EndPoint = endPoint;
			Counter = 0;
			Length = (int)(startPoint - endPoint).Length();
		}

		public void Update()
		{
			Counter++;
			foreach (SubStar star in SubStars.ToArray()) //update substars
			{
				star.Update();
				if (star.Counter > 31)
					SubStars.Remove(star);
			}
			if (Main.rand.Next(Math.Max((int)((1f / Length) * 20000), 1)) + 1 < 4)
				SubStars.Add(new SubStar(Main.rand.NextFloat(0.3f, 0.5f), Vector2.Lerp(StartPoint, EndPoint, Main.rand.NextFloat()) + (Main.rand.NextFloat(6.28f).ToRotationVector2() * Main.rand.Next(15, 40)))); //super magic number-y line, not super proud of this
		}
	}

	public class Starachnid : ModNPC, IStarjinxEnemy, IDrawAdditive
	{
		public List<StarThread> threads = new List<StarThread>(); //All active threads the spider has weaved
		public StarThread currentThread; //The current thread the starachnid is on
		public float progress; //Progress along current thread

		private bool initialized = false; //Has the thread been started?
		private float toRotation = 0; //The rotation for the spider to rotate to
		private float threadRotation = 0; //Current rotation of the thread
		private float speed = 2; //The walking speed of the spider
		private int threadCounter = 0; //How long has it been since last star?

		internal const float THREADGROWLERP = 30; //How many frames does it take for threads to fade in/out?
		internal const int DISTANCEFROMPLAYER = 300; //How far does the spider have to be from the player to turn around?

		private int seed; // The seed shared between the server and clients for the starachshit
		private bool seedInitialized;
		private UnifiedRandom random; // The random instance created with the above seed, if in multiplayer

		private Vector2 Bottom
		{
			get => npc.Center + ((npc.height / 2) * (toRotation + 1.57f).ToRotationVector2());
			set => npc.Center = value - ((npc.height / 2) * (toRotation + 1.57f).ToRotationVector2());
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starachnid");
			Main.npcFrameCount[npc.type] = 8;
		}

		public override bool Autoload(ref string name) => false;

		public override void SetDefaults()
		{
			npc.width = 64;
			npc.height = 64;
			npc.damage = 70;
			npc.defense = 28;
			npc.lifeMax = 450;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.DD2_LightningBugDeath;
			npc.value = 600f;
			npc.knockBackResist = 0;
			npc.noGravity = true;
			npc.noTileCollide = true;

			int seed = Main.rand.Next();
			random = new UnifiedRandom(seed);

			if (Main.netMode == NetmodeID.Server)
				npc.netUpdate = true;
		}

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(seed);

		// If in multiplayer we receive the seed from the server and create the starachnid's random instance with it
		// The starachnid does not update in multiplayer until the seed is received
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			int seed = reader.ReadInt32();

			if (!seedInitialized)
			{
				this.seed = seed;
				random = new UnifiedRandom(seed);
				seedInitialized = true;

				initialized = false;
				toRotation = 0f;
				threadRotation = 0f;
				threadCounter = 0;
				threads.Clear();
				currentThread = null;
				progress = 0f;
			}
		}

		public override void AI()
		{
			if (!seedInitialized && Main.netMode != NetmodeID.SinglePlayer)
				return;

			npc.TargetClosest(false);

			if (!initialized)
			{
				Main.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy, npc.Center);
				initialized = true;
				NewThread(true, true);
				Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<StarachnidProj>(), Main.expertMode ? 20 : 45, 0, npc.target, npc.whoAmI);
			}

			TraverseThread(); //Walk along thread
			RotateIntoPlace(); //Smoothly rotate into place
			UpdateThreads(); //Update the spider's threads

			if (progress >= 1) //If it's at the end of it's thread, create a new thread
				NewThread(false, true);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 12; i++)
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.VilePowder, 2.5f * hitDirection, -2.5f, 0, default, Main.rand.NextFloat(.45f, .75f));

			if (npc.life <= 0)
			{
                for (int k = 0; k < 4; k++)
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/StarjinxEvent/Starachnid/Starachnid1"), Main.rand.NextFloat(.6f, 1f));
				ThreadDeathDust();
			}
		}

		public override Color? GetAlpha(Color drawColor) => Color.Lerp(drawColor, Color.White, 0.5f) * npc.Opacity;
		public void DrawPathfinderOutline(SpriteBatch spriteBatch) => PathfinderOutlineDraw.DrawAfterImage(spriteBatch, npc, npc.frame, Vector2.Zero, npc.frame.Size() / 2);

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, npc.frame, GetAlpha(drawColor).Value, npc.rotation % 6.28f, npc.frame.Size() / 2, npc.scale, SpriteEffects.FlipHorizontally, 0);

			return false;
		}

		public void AdditiveCall(SpriteBatch sB)
		{
			DrawThreads(sB);
			sB.Draw(ModContent.GetTexture(Texture + "_glow"), npc.Center - Main.screenPosition, npc.frame, Color.White, npc.rotation % 6.28f, npc.frame.Size() / 2, npc.scale, SpriteEffects.FlipHorizontally, 0);
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
			npc.frameCounter += 0.30f;
		}

		private void NewThread(bool firstThread, bool mainThread)
		{
			if (!firstThread && mainThread)
				threads.Add(currentThread);

			Vector2 startPos = Bottom;
			int maxDistance = random.Next(200, 500);
			if (!mainThread)
				maxDistance = (int)(maxDistance * 0.5f);

			int distance = 0;
			Vector2 direction = NewThreadAngle(maxDistance, mainThread, ref distance, startPos); //Get both the direction (direction) and the length (i) of the next thread

			var thread = new StarThread(startPos, startPos + (direction * distance));

			Vector2 newPos = Vector2.Zero; //make star overlap if theres a nearby star

			if (!firstThread)
				if (NearbyStars(startPos + (direction * distance), ref newPos))
					thread = new StarThread(startPos, newPos);

			thread.StartScale = random.NextFloat(0.5f, 0.8f);
			thread.EndScale = random.NextFloat(0.5f, 0.8f);

			if (mainThread)
			{
				thread.DrawStart = false;
				currentThread = thread;
				progress = 0;
				threadRotation = direction.ToRotation();
			}
			else
			{
				threads.Add(thread);
				thread.Duration -= (currentThread.Counter + (int)THREADGROWLERP); //Make sure it fades away before the thread it's attached to!
				threadCounter = 45; //45 ticks until stars are able to spawn again
			}
		}

		//The follow method runs 2 while loops to try and avoid tiles
		//Both while loops cast out from the bottom of the spider, in a random angle, and try to reach their set distance without hitting a tile. If they can't, they try again
		//Both have "tries" to make sure it doesn't try over 100 times
		//The first while loop, the new angle is in the same GENERAL direction as the current thread
		//The second one can go in any direction
		//However, if the player is too far away, it curves towards them
		private Vector2 NewThreadAngle(int maxDistance, bool mainThread, ref int dist, Vector2 startPos)
		{
			Player player = Main.player[npc.target];
			Vector2 distanceFromPlayer = player.Center - startPos;
			Vector2 direction = Vector2.One;
			int tries = 0;

			if (distanceFromPlayer.Length() > DISTANCEFROMPLAYER && mainThread)
			{
				while (dist < maxDistance) //Loop through the shortest angle to the player, but multiplied by a random float (above 0.5f)
				{
					float rotDifference = ((((distanceFromPlayer.ToRotation() - threadRotation) % MathHelper.TwoPi) + 9.42f) % MathHelper.TwoPi) - MathHelper.Pi;
					direction = (threadRotation + (Math.Sign(rotDifference) * random.NextFloat(1f, 1.5f))).ToRotationVector2();

					for (dist = 16; dist < maxDistance; dist++)
					{
						Vector2 toLookAt = startPos + (direction * dist);
						if (IsTileActive(toLookAt))
							break;
					}

					if (tries++ > 100)
						break;
				}

				Vector2 finalPos = startPos + (direction * dist); //Top of the world check
				if (finalPos.Y < 590 && direction.Y < 0)
					direction.Y *= -1;

				if (ModContent.GetInstance<StarjinxEventWorld>().StarjinxActive && Vector2.Distance(finalPos, player.GetModPlayer<StarjinxPlayer>().StarjinxPosition) > StarjinxMeteorite.EVENT_RADIUS - 50)
				{ //Check for leaving event area
					direction = Vector2.Normalize(player.GetModPlayer<StarjinxPlayer>().StarjinxPosition - startPos);
					dist = maxDistance;
				}

				tries = 0;
				if (dist < maxDistance) //Runs if the current angle would make too short of a thread
				{
					while (dist < maxDistance)
					{
						direction = random.NextFloat(MathHelper.TwoPi).ToRotationVector2();
						for (dist = 16; dist < maxDistance; dist++)
						{
							Vector2 toLookAt = startPos + (direction * dist);
							if (IsTileActive(toLookAt))
								break;
						}
						tries++;
						if (tries > 100)
							break;
					}
				}
			}
			else
			{
				while (dist < maxDistance) //first while loop - get angle
				{
					if (mainThread)
						direction = (threadRotation + random.NextFloat(-1f, 1f)).ToRotationVector2(); //Woohoo magic numbers
					else
						direction = random.NextFloat(MathHelper.TwoPi).ToRotationVector2();

					for (dist = 16; dist < maxDistance; dist++)
					{
						Vector2 toLookAt = startPos + (direction * dist);
						if (IsTileActive(toLookAt))
							break;
					}

					if (tries++ > 100)
						break;
				}

				Vector2 finalPos = startPos + (direction * dist); //Top of the world check
				if (finalPos.Y < 590 && direction.Y < 0)
					direction.Y *= -1;

				if (ModContent.GetInstance<StarjinxEventWorld>().StarjinxActive && Vector2.Distance(finalPos, player.GetModPlayer<StarjinxPlayer>().StarjinxPosition) > StarjinxMeteorite.EVENT_RADIUS)
				{ //Check for leaving event area
					direction = Vector2.Normalize(player.GetModPlayer<StarjinxPlayer>().StarjinxPosition - startPos);
					dist = 2;
				}

				tries = 0;
				if (dist < maxDistance) //Runs if the current angle would make too short of a thread
				{
					while (dist < maxDistance)
					{
						direction = random.NextFloat(MathHelper.TwoPi).ToRotationVector2();
						for (dist = 16; dist < maxDistance; dist++)
						{
							Vector2 toLookAt = startPos + (direction * dist);
							if (IsTileActive(toLookAt))
								break;
						}

						if (tries++ > 100)
							break;
					}
				}
			}
			return direction;
		}

		private bool NearbyStars(Vector2 position, ref Vector2 output)
		{
			int maxDistance = 40;
			float distance;

			foreach (StarThread thread in threads.ToArray())
			{
				distance = (thread.StartPoint - position).Length();
				if (distance < maxDistance)
				{
					output = thread.StartPoint;
					return true;
				}
				distance = (thread.EndPoint - position).Length();
				if (distance < maxDistance)
				{
					output = thread.EndPoint;
					return true;
				}
			}

			distance = (currentThread.StartPoint - position).Length();

			if (distance < maxDistance)
			{
				output = currentThread.StartPoint;
				return true;
			}

			distance = (currentThread.EndPoint - position).Length();

			if (distance < maxDistance)
			{
				output = currentThread.EndPoint;
				return true;
			}
			return false;
		}

		private bool IsTileActive(Vector2 toLookAt) //Is the tile at the vector position solid?
		{
			Point tPos = toLookAt.ToTileCoordinates();
			if (WorldGen.InWorld(tPos.X, tPos.Y, 2) && Framing.GetTileSafely(tPos.X, tPos.Y).active())
				return Main.tileSolid[Framing.GetTileSafely((int)(toLookAt.X / 16f), (int)(toLookAt.Y / 16f)).type];

			return false;
		}

		private void TraverseThread()
		{
			progress += (1f / currentThread.Length) * speed;
			Bottom = Vector2.Lerp(currentThread.StartPoint, currentThread.EndPoint, progress);
			threadCounter--;
			//if (random.Next(200) == 0 && threadCounter < 0 && progress > 0.15f && progress < 0.85f)
			//	NewThread(false, false);
			if (!Main.dedServ && progress < 0.9f && progress > 0) //Make particles along the thread while starachnid walks
			{
				for(int i = -1; i <= 1; i++)
				{
					Vector2 vel = Vector2.Normalize(npc.position - npc.oldPosition);
					Particles.ParticleHandler.SpawnParticle(new Particles.GlowParticle(Bottom, vel.RotatedBy(Main.rand.NextFloat(MathHelper.Pi / 16, MathHelper.Pi / 6) * i),
						Color.HotPink, Main.rand.NextFloat(0.05f, 0.1f), 22, 15));
				}
			}
		}

		private void RotateIntoPlace()
		{
			toRotation = (currentThread.EndPoint - currentThread.StartPoint).ToRotation();

			bool empowered = npc.GetGlobalNPC<PathfinderGNPC>().Buffed;
			float speedInc = empowered ? 1.4f : 1f;

			float rotDifference = ((((toRotation - npc.rotation) % 6.28f) + 9.42f) % 6.28f) - 3.14f;
			if (Math.Abs(rotDifference) < 0.15f)
			{
				npc.rotation = toRotation;
				speed = 2 * speedInc;
				return;
			}
			speed = 1 * speedInc;
			npc.rotation += Math.Sign(rotDifference) * 0.06f;
		}

		private void DrawThreads(SpriteBatch spriteBatch)
		{
			if (currentThread == null)
				return;

			float length;
			Texture2D tex = SpiritMod.Instance.GetTexture("Textures/Trails/Trail_4");
			Vector2 threadScale = new Vector2(1 / (float)tex.Width, 30 / (float)tex.Height); //Base scale of the thread based on the texture's size, stretched horizontally depending on thread length

			//Draw each thread's beam
			foreach (StarThread thread in threads) 
			{
				length = Math.Min(thread.Counter / THREADGROWLERP, (thread.Duration - thread.Counter) / THREADGROWLERP);
				length = Math.Min(length, 1);
				spriteBatch.Draw(tex, thread.StartPoint - Main.screenPosition, null, Color.HotPink * length, (thread.EndPoint - thread.StartPoint).ToRotation(), new Vector2(0f, tex.Height / 2), 
					threadScale * new Vector2(thread.Length, 1), SpriteEffects.None, 0f);
			}
			StarThread thread2 = currentThread;

			float size = Math.Min(thread2.Counter / THREADGROWLERP, (thread2.Duration - thread2.Counter) / THREADGROWLERP);
			size = Math.Min(size, 1);

			spriteBatch.Draw(tex, thread2.StartPoint - Main.screenPosition, null, Color.HotPink, (thread2.EndPoint - thread2.StartPoint).ToRotation(), //Draw the portion of the current beam that's already been walked through
				new Vector2(0f, tex.Height / 2), threadScale * new Vector2(progress * thread2.Length, 1), SpriteEffects.None, 0f);

			spriteBatch.Draw(tex, thread2.EndPoint - Main.screenPosition, null, Color.HotPink * 0.5f * size, (thread2.StartPoint - thread2.EndPoint).ToRotation(), //Draw the remaining portion at lower opacity
				new Vector2(0f, tex.Height / 2), threadScale * new Vector2((1 - progress) * thread2.Length, 1), SpriteEffects.None, 0f);

			tex = SpiritMod.Instance.GetTexture("NPCs/StarjinxEvent/Enemies/Starachnid/SpiderStar");
			Texture2D Bloom = mod.GetTexture("Effects/Masks/CircleGradient");

			//Use a method to cut down on boilerplate with drawing stars
			void DrawStar(Vector2 center, float starSize, float rotation) 
			{
				int bloomstodraw = 3;
				for (int i = 0; i < bloomstodraw; i++)
				{
					float progress = i / (float)bloomstodraw;
					spriteBatch.Draw(Bloom, center - Main.screenPosition, null, Color.HotPink * starSize * MathHelper.Lerp(1f, 0.5f, 1 - progress), 0,
						Bloom.Size() / 2, starSize * 0.4f * MathHelper.Lerp(0.66f, 1f, progress), SpriteEffects.None, 0);
				}

				spriteBatch.Draw(tex, center - Main.screenPosition, null, Color.White * starSize, rotation, tex.Size() / 2, starSize * 0.6f, SpriteEffects.None, 0);
				spriteBatch.Draw(tex, center - Main.screenPosition, null, Color.White * starSize * 0.7f, -rotation, tex.Size() / 2, starSize * 0.5f, SpriteEffects.None, 0);
			}

			float rotationSpeed = 1.5f;
			if (threads.Count == 0) //Fix it otherwise spawning on a thread with only 1 star
				DrawStar(thread2.StartPoint, size, Main.GlobalTime * rotationSpeed);

			DrawStar(thread2.EndPoint, size, Main.GlobalTime * rotationSpeed);

			//Draw stars at each thread's end and start points
			int threadsDrawn = 0;
			foreach (StarThread thread in threads) 
			{
				size = Math.Min(thread.Counter / THREADGROWLERP, (thread.Duration - thread.Counter) / THREADGROWLERP);
				size = Math.Min(size, 1);
				if (thread.DrawStart || threadsDrawn == 0)
					DrawStar(thread.StartPoint, size, Main.GlobalTime * rotationSpeed);

				DrawStar(thread.EndPoint, 1, Main.GlobalTime * rotationSpeed);
				if (!thread.DrawStart)
					threadsDrawn++;
			}

			//Draw substars(replace with particles later?)
			foreach (StarThread thread in threads)
			{
				foreach (SubStar star in thread.SubStars)
				{
					float scale = star.MaxScale * (float)Math.Sin(star.Counter / 10f) * 0.5f;
					spriteBatch.Draw(tex, star.Position - Main.screenPosition, null, Color.LightPink, 0, new Vector2(tex.Width, tex.Height) / 2, scale, SpriteEffects.None, 0f);
				}
			}
			foreach (SubStar star in currentThread.SubStars)
			{
				float scale = star.MaxScale * (float)Math.Sin(star.Counter / 10f) * 0.5f;
				spriteBatch.Draw(tex, star.Position - Main.screenPosition, null, Color.LightPink, 0, new Vector2(tex.Width, tex.Height) / 2, scale, SpriteEffects.None, 0f);
			}
		}

		private void UpdateThreads()
		{
			foreach (StarThread thread in threads)
			{
				thread.Update();
			}
			foreach (StarThread thread in threads.ToArray())
			{
				if (thread.Counter > thread.Duration)
					threads.Remove(thread);
			}
			StarThread thread2 = currentThread;
			thread2.Update();
		}
		private void ThreadDeathDust()
		{
			if (Main.dedServ) //Dont do this on server
				return;

			//Burst of star particles at the positions of endpoints
			void StarBreakParticles(Vector2 position)
			{
				for (int i = 0; i < 5; i++)
					Particles.ParticleHandler.SpawnParticle(new Particles.StarParticle(position, Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.5f),
						Color.LightPink, Color.HotPink, Main.rand.NextFloat(0.2f, 0.3f), 25));
			}

			//Bloom particles along each thread
			void ThreadBreakParticles(StarThread thread, float progress = 1)
			{
				for (int i = 0; i < thread.Length * progress; i += Main.rand.Next(7, 14))
				{
					Vector2 direction = thread.EndPoint - thread.StartPoint;
					direction.Normalize();
					Vector2 position = thread.StartPoint + (direction * i);
					Particles.ParticleHandler.SpawnParticle(new Particles.GlowParticle(position, Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.5f), Color.HotPink,
						Main.rand.NextFloat(0.07f, 0.1f), Main.rand.Next(30, 40), 12));
				}
			}

			foreach (StarThread thread in threads)
			{
				ThreadBreakParticles(thread);
				StarBreakParticles(thread.StartPoint);
				StarBreakParticles(thread.EndPoint);
			}

			ThreadBreakParticles(currentThread, progress);
			StarBreakParticles(currentThread.StartPoint);
			StarBreakParticles(currentThread.EndPoint);
		}

		public override void NPCLoot()
		{
			if (Main.rand.NextBool(15))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Pets.CosmicRattler.CosmicRattler>());
		}
	}

	public class StarachnidProj : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Constellation");

		public override void SetDefaults()
		{
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.width = projectile.height = 38;
			projectile.timeLeft = 150;
			projectile.ignoreWater = true;
			projectile.alpha = 255;
		}

		NPC parent;

		public override void AI()
		{
			parent = Main.npc[(int)projectile.ai[0]];
			if (parent.active)
			{
				projectile.Center = parent.Center;
				projectile.timeLeft = 2;
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (parent != null)
			{
				if (parent.active && parent.modNPC is Starachnid parent2)
				{
					foreach (StarThread thread in parent2.threads)
					{
						if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), thread.StartPoint, thread.EndPoint) && thread.Counter > 35)
							return true;
					}

					StarThread currentThread = parent2.currentThread;
					Vector2 direction = currentThread.EndPoint - currentThread.StartPoint;

					if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), currentThread.StartPoint, currentThread.StartPoint + (direction * parent2.progress)))
						return true;
				}
			}
			return false;
		}
	}
}
