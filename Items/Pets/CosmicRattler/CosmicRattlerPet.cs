using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Utilities;
using SpiritMod.NPCs.StarjinxEvent.Enemies.Starachnid;

namespace SpiritMod.Items.Pets.CosmicRattler
{
	public class CosmicRattlerPet : ModProjectile, IDrawAdditive
	{
		private int seed; // The seed shared between the server and clients for the starachshit
		private bool seedInitialized;
		private UnifiedRandom random; // The random instance created with the above seed, if in multiplayer

		public List<StarThread> threads = new List<StarThread>(); //All active threads the spider has weaved
		public StarThread currentThread; //The current thread the starachnid is on
		public float progress; //Progress along current thread

		private bool initialized = false; //Has the thread been started?
		private float toRotation = 0; //The rotation for the spider to rotate to
		private float threadRotation = 0; //Current rotation of the thread
		private float speed = 2; //The walking speed of the spider

		internal const float THREADGROWLERP = 30; //How many frames does it take for threads to fade in/out?
		internal const int DISTANCEFROMPLAYER = 300; //How far does the spider have to be from the player to turn around?

		private Vector2 Bottom
		{
			get => Projectile.Center + ((Projectile.height / 2) * (toRotation + 1.57f).ToRotationVector2());
			set => Projectile.Center = value - ((Projectile.height / 2) * (toRotation + 1.57f).ToRotationVector2());
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starachnid");
			Main.projFrames[Projectile.type] = 5;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Truffle);
			Projectile.aiStyle = 0;
			Projectile.width = 28;
			Projectile.height = 36;

			int seed = Main.rand.Next();
			random = new UnifiedRandom(seed);

			if (Main.netMode == NetmodeID.Server)
				Projectile.netUpdate = true;
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
				threads.Clear();
				currentThread = null;
				progress = 0f;
			}
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			player.truffle = false; // Relic from aiType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.dead)
				modPlayer.starachnidPet = false;
			if (modPlayer.starachnidPet)
				Projectile.timeLeft = 2;

			if (!seedInitialized && Main.netMode != NetmodeID.SinglePlayer)
				return;

			if (!initialized)
			{
				initialized = true;
				NewThread(true, true);
			}

			TraverseThread(); //Walk along thread
			RotateIntoPlace(); //Smoothly rotate into place
			UpdateThreads(); //Update the spider's threads

			if (progress >= 1) //If it's at the end of it's thread, create a new thread
				NewThread(false, true);
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
				//threadCounter = 45; //45 ticks until stars are able to spawn again
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
			Player player = Main.player[Projectile.owner];
			Vector2 distanceFromPlayer = player.Center - startPos;
			Vector2 direction;

			if (distanceFromPlayer.Length() > DISTANCEFROMPLAYER && mainThread) //If the player is too far, go roughly to the player
			{
				float rotDifference = ((((distanceFromPlayer.ToRotation() - threadRotation) % MathHelper.TwoPi) + 9.42f) % MathHelper.TwoPi) - MathHelper.Pi;
				direction = (threadRotation + (Math.Sign(rotDifference) * random.NextFloat(1f, 1.5f))).ToRotationVector2();

				Vector2 finalPos = startPos + (direction * dist); //Top of the world check
				if (finalPos.Y < 590 && direction.Y < 0)
					direction.Y *= -1;
			}
			else
			{
				if (mainThread)
					direction = (threadRotation + random.NextFloat(-1f, 1f)).ToRotationVector2(); //Woohoo magic numbers
				else
					direction = random.NextFloat(MathHelper.TwoPi).ToRotationVector2();

				Vector2 finalPos = startPos + (direction * dist); //Top of the world check
				if (finalPos.Y < 590 && direction.Y < 0)
					direction.Y *= -1;
			}
			dist = maxDistance;
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
		private void TraverseThread()
		{
			progress += (1f / currentThread.Length) * speed;
			Bottom = Vector2.Lerp(currentThread.StartPoint, currentThread.EndPoint, progress);
			//if (random.Next(200) == 0 && threadCounter < 0 && progress > 0.15f && progress < 0.85f)
			//	NewThread(false, false);
			if (!Main.dedServ && progress < 0.9f && progress > 0) //Make particles along the thread while starachnid walks
			{
				for (int i = -1; i <= 1; i++)
				{
					Vector2 vel = Vector2.Normalize(Projectile.position - Projectile.oldPosition);
					Particles.ParticleHandler.SpawnParticle(new Particles.GlowParticle(Bottom, vel.RotatedBy(Main.rand.NextFloat(MathHelper.Pi / 16, MathHelper.Pi / 6) * i),
						Color.HotPink, Main.rand.NextFloat(0.05f, 0.1f), 22, 15));
				}
			}
		}

		private void RotateIntoPlace()
		{
			toRotation = (currentThread.EndPoint - currentThread.StartPoint).ToRotation();

			float speedInc = 1f;

			float rotDifference = ((((toRotation - Projectile.rotation) % 6.28f) + 9.42f) % 6.28f) - 3.14f;
			if (Math.Abs(rotDifference) < 0.15f)
			{
				Projectile.rotation = toRotation;
				speed = 2 * speedInc;
				return;
			}
			speed = 1 * speedInc;
			Projectile.rotation += Math.Sign(rotDifference) * 0.06f;
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

		public override Color? GetAlpha(Color drawColor) => Color.Lerp(drawColor, Color.White, 0.5f) * Projectile.Opacity;

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			int frameHeight = tex.Height / Main.projFrames[Projectile.type];
			Rectangle frame = new Rectangle(0, Projectile.frame, tex.Width / 3, frameHeight);
			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, frame, GetAlpha(drawColor).Value, Projectile.rotation % 6.28f, frame.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0);

			return false;
		}

		public void AdditiveCall(SpriteBatch sB)
		{
			DrawThreads(sB);

			Texture2D tex = ModContent.Request<Texture2D>(Texture + "_Glow");
			int frameHeight = tex.Height / Main.projFrames[Projectile.type];
			Rectangle frame = new Rectangle(0, Projectile.frame, tex.Width / 3, frameHeight);
			sB.Draw(tex, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation % 6.28f, frame.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0);
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
			Texture2D Bloom = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value;

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
				DrawStar(thread2.StartPoint, size, Main.GlobalTimeWrappedHourly * rotationSpeed);

			DrawStar(thread2.EndPoint, size, Main.GlobalTimeWrappedHourly * rotationSpeed);

			//Draw stars at each thread's end and start points
			int threadsDrawn = 0;
			foreach (StarThread thread in threads)
			{
				size = Math.Min(thread.Counter / THREADGROWLERP, (thread.Duration - thread.Counter) / THREADGROWLERP);
				size = Math.Min(size, 1);
				if (thread.DrawStart || threadsDrawn == 0)
					DrawStar(thread.StartPoint, size, Main.GlobalTimeWrappedHourly * rotationSpeed);

				DrawStar(thread.EndPoint, 1, Main.GlobalTimeWrappedHourly * rotationSpeed);
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
	}
}