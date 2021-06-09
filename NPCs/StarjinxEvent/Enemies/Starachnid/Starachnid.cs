using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.Utilities;

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
			if (Main.rand.Next((int)((1f / Length) * 20000)) < 3)
				SubStars.Add(new SubStar(Main.rand.NextFloat(0.3f, 0.5f), Vector2.Lerp(StartPoint, EndPoint, Main.rand.NextFloat()) + (Main.rand.NextFloat(6.28f).ToRotationVector2() * Main.rand.Next(15, 40)))); //super magic number-y line, not super proud of this
		}
	}

	public class Starachnid : ModNPC
	{
		public List<StarThread> threads = new List<StarThread>(); //All active threads the spider has weaved
		public StarThread currentThread; //The current thread the starachnid is on
		public float progress; //Progress along current thread

		private bool initialized = false; //Has the thread been started?
		private float toRotation = 0; //The rotation for the spider to rotate to
		private float threadRotation = 0; //Current rotation of the thread
		private float speed = 2; //The walking speed of the spider
		private int threadCounter = 0; //How long has it been since last star?
		private Color color = new Color(255, 148, 235, 0); //The color of the constellation

		private const float THREADGROWLERP = 15; //How many frames does it take for threads to fade in/out?
		private const int DISTANCEFROMPLAYER = 300; //How far does the spider have to be from the player to turn around?

		private int seed; // The seed shared between the server and clients for the starachshit
		private bool seedInitialized;
		private UnifiedRandom random; // The random instance created with the above seed, if in multiplayer

		private Vector2 Bottom
		{
			get
			{
				return npc.Center + ((npc.height / 2) * (toRotation + 1.57f).ToRotationVector2());
			}
			set
			{
				npc.Center = value - ((npc.height / 2) * (toRotation + 1.57f).ToRotationVector2());
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starachnid");
			Main.npcFrameCount[npc.type] = 8;
		}

		public override void SetDefaults()
		{
			npc.width = 64;
			npc.height = 64;
			npc.damage = 70;
			npc.defense = 30;
			npc.lifeMax = 600;
			npc.HitSound = SoundID.NPCHit6;
			npc.DeathSound = SoundID.NPCDeath8;
			npc.value = 10000f;
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
				initialized = true;
				NewThread(true, true);
				Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<StarachnidProj>(), Main.expertMode ? 40 : 60, 0, npc.target, npc.whoAmI);
			}

			Lighting.AddLight(npc.Center, color.R / 300f, color.G / 300f, color.B / 300f);

			TraverseThread(); //Walk along thread
			RotateIntoPlace(); //Smoothly rotate into place
			UpdateThreads(); //Update the spider's threads

			if (progress >= 1) //If it's at the end of it's thread, create a new thread
				NewThread(false, true);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
				ThreadDeathDust();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, npc.frame, drawColor, npc.rotation % 6.28f, npc.frame.Size() / 2, npc.scale, SpriteEffects.FlipHorizontally, 0);

			DrawThreads(spriteBatch);
			return false;
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

			int i = 0;
			Vector2 direction = NewThreadAngle(maxDistance, mainThread, ref i, startPos); //Get both the direction (direction) and the length (i) of the next thread

			StarThread thread = new StarThread(startPos, startPos + (direction * i));

			Vector2 newPos = Vector2.Zero; //make star overlap if theres a nearby star

			if (!firstThread)
				if (NearbyStars(startPos + (direction * i), ref newPos))
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
		private Vector2 NewThreadAngle(int maxDistance, bool mainThread, ref int i, Vector2 startPos)
		{
			Player player = Main.player[npc.target];
			Vector2 distanceFromPlayer = player.Center - startPos;
			Vector2 direction = Vector2.Zero;
			int tries = 0;

			if (distanceFromPlayer.Length() > DISTANCEFROMPLAYER && mainThread)
			{
				while (i < maxDistance) //Loop through the shortest angle to the player, but multiplied by a random float (above 0.5f)
				{
					float rotDifference = ((((distanceFromPlayer.ToRotation() - threadRotation) % 6.28f) + 9.42f) % 6.28f) - 3.14f;
					direction = (threadRotation + (Math.Sign(rotDifference) * random.NextFloat(1f, 1.5f))).ToRotationVector2();
					for (i = 16; i < maxDistance; i++)
					{
						Vector2 toLookAt = startPos + (direction * i);
						if (IsTileActive(toLookAt))
							break;
					}
					tries++;
					if (tries > 100)
						break;
				}

				tries = 0;
				if (i < maxDistance)
				{
					while (i < maxDistance)
					{
						direction = random.NextFloat(6.28f).ToRotationVector2();
						for (i = 16; i < maxDistance; i++)
						{
							Vector2 toLookAt = startPos + (direction * i);
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
				while (i < maxDistance) //first while loop
				{
					if (mainThread)
						direction = (threadRotation + random.NextFloat(-1f, 1f)).ToRotationVector2(); //Woohoo magic numbers
					else
						direction = random.NextFloat(6.28f).ToRotationVector2();
					for (i = 16; i < maxDistance; i++)
					{
						Vector2 toLookAt = startPos + (direction * i);
						if (IsTileActive(toLookAt))
							break;
					}
					tries++;
					if (tries > 100)
						break;
				}

				tries = 0;
				if (i < maxDistance)
				{
					while (i < maxDistance)
					{
						direction = random.NextFloat(6.28f).ToRotationVector2();
						for (i = 16; i < maxDistance; i++)
						{
							Vector2 toLookAt = startPos + (direction * i);
							if (IsTileActive(toLookAt))
								break;
						}
						tries++;
						if (tries > 100)
							break;
					}
				}
			}
			return direction;
		}

		private bool NearbyStars(Vector2 position, ref Vector2 output)
		{
			int maxDistance = 40;
			float distance = 0;
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
			return Framing.GetTileSafely((int)(toLookAt.X / 16), (int)(toLookAt.Y / 16)).active() && Main.tileSolid[Framing.GetTileSafely((int)(toLookAt.X / 16), (int)(toLookAt.Y / 16)).type];
		}

		private void TraverseThread()
		{
			progress += (1f / currentThread.Length) * speed;
			Bottom = Vector2.Lerp(currentThread.StartPoint, currentThread.EndPoint, progress);
			threadCounter--;
			if (random.Next(200) == 0 && threadCounter < 0 && progress > 0.15f && progress < 0.85f)
			{
				NewThread(false, false);
			}
			Dust.NewDustPerfect(Bottom, 21);
		}

		private void RotateIntoPlace()
		{
			toRotation = (currentThread.EndPoint - currentThread.StartPoint).ToRotation();
			float rotDifference = ((((toRotation - npc.rotation) % 6.28f) + 9.42f) % 6.28f) - 3.14f;
			if (Math.Abs(rotDifference) < 0.15f)
			{
				npc.rotation = toRotation;
				speed = 2;
				return;
			}
			speed = 1;
			npc.rotation += Math.Sign(rotDifference) * 0.06f;
		}

		private void DrawThreads(SpriteBatch spriteBatch)
		{
			if (currentThread == null)
				return;

			float length;
			Texture2D tex = SpiritMod.instance.GetTexture("NPCs/StarjinxEvent/Enemies/Starachnid/ConstellationStrip");
			foreach (StarThread thread in threads)
			{
				length = Math.Min(thread.Counter / THREADGROWLERP, (thread.Duration - thread.Counter) / THREADGROWLERP);
				length = Math.Min(length, 1);
				spriteBatch.Draw(tex, thread.StartPoint - Main.screenPosition, null, color * 0.75f, (thread.EndPoint - thread.StartPoint).ToRotation(), new Vector2(0f, tex.Height / 2), new Vector2(thread.Length, 1) * length, SpriteEffects.None, 0f);
			}
			StarThread thread2 = currentThread;
			spriteBatch.Draw(tex, thread2.StartPoint - Main.screenPosition, null, color * 0.75f, (thread2.EndPoint - thread2.StartPoint).ToRotation(), new Vector2(0f, tex.Height / 2), new Vector2(thread2.Length * progress, 1), SpriteEffects.None, 0f);

			tex = SpiritMod.instance.GetTexture("NPCs/StarjinxEvent/Enemies/Starachnid/SpiderStar");
			float size = Math.Min(thread2.Counter / THREADGROWLERP, (thread2.Duration - thread2.Counter) / THREADGROWLERP);
			size = Math.Min(size, 1);
			spriteBatch.Draw(tex, thread2.EndPoint - Main.screenPosition, null, color * size, 0, new Vector2(tex.Width, tex.Height) / 2, size * thread2.EndScale, SpriteEffects.None, 0f);

			int threadsDrawn = 0;
			foreach (StarThread thread in threads)
			{
				size = Math.Min(thread.Counter / THREADGROWLERP, (thread.Duration - thread.Counter) / THREADGROWLERP);
				size = Math.Min(size, 1);
				if (thread.DrawStart || threadsDrawn == 0)
					spriteBatch.Draw(tex, thread.StartPoint - Main.screenPosition, null, color * size, 0, new Vector2(tex.Width, tex.Height) / 2, size * thread.StartScale, SpriteEffects.None, 0f);
				spriteBatch.Draw(tex, thread.EndPoint - Main.screenPosition, null, color * size, 0, new Vector2(tex.Width, tex.Height) / 2, size * thread.EndScale, SpriteEffects.None, 0f);
				if (!thread.DrawStart)
					threadsDrawn++;
			}

			//draw substars
			foreach (StarThread thread in threads)
			{
				foreach (SubStar star in thread.SubStars)
				{
					float scale = star.MaxScale * (float)Math.Sin(star.Counter / 10f);
					spriteBatch.Draw(tex, star.Position - Main.screenPosition, null, color, 0, new Vector2(tex.Width, tex.Height) / 2, scale, SpriteEffects.None, 0f);
				}
			}
			foreach (SubStar star in currentThread.SubStars)
			{
				float scale = star.MaxScale * (float)Math.Sin(star.Counter / 10f);
				spriteBatch.Draw(tex, star.Position - Main.screenPosition, null, color, 0, new Vector2(tex.Width, tex.Height) / 2, scale, SpriteEffects.None, 0f);
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
			foreach (StarThread thread in threads)
			{
				for (int i = 0; i < thread.Length; i += 20)
				{
					Vector2 direction = thread.EndPoint - thread.StartPoint;
					direction.Normalize();
					Vector2 position = thread.StartPoint + (direction * i);
					Dust.NewDustPerfect(position, 21);
				}
			}
			for (int i = 0; i < currentThread.Length * progress; i += 20)
			{
				Vector2 direction = currentThread.EndPoint - currentThread.StartPoint;
				direction.Normalize();
				Vector2 position = currentThread.StartPoint + (direction * i);
				Dust.NewDustPerfect(position, 21);
			}
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
						{
							return true;
						}
					}
					StarThread currentThread = parent2.currentThread;
					Vector2 direction = currentThread.EndPoint - currentThread.StartPoint;
					if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), currentThread.StartPoint, currentThread.StartPoint + (direction * parent2.progress)))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
