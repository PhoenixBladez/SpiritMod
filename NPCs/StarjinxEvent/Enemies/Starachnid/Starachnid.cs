using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;
using System.Collections.Generic;
using SpiritMod.Stargoop;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Starachnid
{
	public class StarThread
	{
		public Vector2 StartPoint;
		public Vector2 EndPoint;

		public int Counter;
		public int Duration = 800; //How long the thread lasts
		public int Length; //Length of the thread

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
		}
	}

	public class Starachnid : ModNPC, IGalaxySprite
	{
		private List<StarThread> threads = new List<StarThread>();
		private StarThread currentThread; //The current thread the starachnid is on
		private float progress; //Progress along current thread
		private bool initialized = false; //Has the thread been started?
		private float toRotation = 0; //The rotation for the spider to rotate to
		private float threadRotation = 0; //Current rotation of the thread

		private const float SPEED = 2; //The walking speed of the spider
		private const float THREADGROWLERP = 15; //How many frames does it take for threads to fade in/out?
		private const int DISTANCEFROMPLAYER = 300; //How far does the spider have to be from the player to turn around?

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
			npc.width = 60;
			npc.height = 60;
			npc.damage = 70;
			npc.defense = 30;
			npc.lifeMax = 600;
			npc.HitSound = SoundID.NPCHit6;
			npc.DeathSound = SoundID.NPCDeath8;
			npc.value = 10000f;
			npc.knockBackResist = 0;
			npc.noGravity = true;
			npc.noTileCollide = true;
			if(Main.netMode != NetmodeID.Server)
				SpiritMod.Metaballs.EnemyLayer.Sprites.Add(this);
		}

		public override void AI()
		{
			if (!initialized)
			{
				initialized = true;
				NewThread(true, true);
			}
			npc.TargetClosest(false);
			TraverseThread(); //Walk along thread
			RotateIntoPlace(); //Smoothly rotate into place
			UpdateThreads(); //Update the spider's threads
			if (progress >= 1) //If it's at the end of it's thread, create a new slope
				NewThread(false, true);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				SpiritMod.Metaballs.EnemyLayer.Sprites.Remove(this);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			npc.frame.X = 0;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, npc.frame, drawColor, npc.rotation % 6.28f, npc.frame.Size() / 2, npc.scale, SpriteEffects.None, 0);

			DrawThreads(spriteBatch);
			return false;
		}

		public void DrawGalaxyMappedSprite(SpriteBatch sB)
		{
			if (npc.type == ModContent.NPCType<Starachnid>() && npc.active)
			{
				npc.frame.X = npc.frame.Width;
				sB.Draw(Main.npcTexture[npc.type], (npc.Center - Main.screenPosition) / 2, npc.frame, Color.White, npc.rotation % 6.28f, npc.frame.Size() / 2, npc.scale * 0.5f, SpriteEffects.None, 0);
			}
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Width = Main.npcTexture[npc.type].Width / 2;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
			npc.frameCounter += 0.20f;
			npc.frameCounter += 0.20f;
		}

		private void NewThread(bool firstThread, bool mainThread)
		{
			if (!firstThread && mainThread)
			{
				threads.Add(currentThread);
			}

			Vector2 startPos = Bottom; 
			int maxDistance = Main.rand.Next(200, 500);
			if (!mainThread)
				maxDistance = (int)(maxDistance * 0.5f);

			int i = 0;
			Vector2 direction = NewThreadAngle(maxDistance, mainThread, ref i, startPos); //Get both the direction (direction) and the length (i) of the next thread

			StarThread thread = new StarThread(startPos, startPos + (direction * i));
			if (mainThread)
			{
				currentThread = thread;
				progress = 0;
				threadRotation = direction.ToRotation();
			}
			else
			{
				threads.Add(thread);
				thread.Duration -= (currentThread.Counter + (int)THREADGROWLERP); //Make sure it fades away before the thread it's attached to!
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
					direction = (threadRotation + (rotDifference * Main.rand.NextFloat(0.5f,1f))).ToRotationVector2();
					for (i = 16; i < maxDistance; i++)
					{
						Vector2 toLookAt = startPos + (direction * i);
						if (Framing.GetTileSafely((int)(toLookAt.X / 16), (int)(toLookAt.Y / 16)).active() && Main.tileSolid[Framing.GetTileSafely((int)(toLookAt.X / 16), (int)(toLookAt.Y / 16)).type])
							break;
					}
					tries++;
					if (tries > 100)
						break;
				}
			}
			else
			{
				while (i < maxDistance) //first while loop
				{
					if (mainThread)
						direction = (threadRotation + Main.rand.NextFloat(-1.3f, 1.3f)).ToRotationVector2(); //Woohoo magic numbers
					else
						direction = Main.rand.NextFloat(6.28f).ToRotationVector2();
					for (i = 16; i < maxDistance; i++)
					{
						Vector2 toLookAt = startPos + (direction * i);
						if (Framing.GetTileSafely((int)(toLookAt.X / 16), (int)(toLookAt.Y / 16)).active() && Main.tileSolid[Framing.GetTileSafely((int)(toLookAt.X / 16), (int)(toLookAt.Y / 16)).type])
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
						direction = Main.rand.NextFloat(6.28f).ToRotationVector2();
						for (i = 16; i < maxDistance; i++)
						{
							Vector2 toLookAt = startPos + (direction * i);
							if (Framing.GetTileSafely((int)(toLookAt.X / 16), (int)(toLookAt.Y / 16)).active() && Main.tileSolid[Framing.GetTileSafely((int)(toLookAt.X / 16), (int)(toLookAt.Y / 16)).type])
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

		private void TraverseThread()
		{
			progress += (1f / currentThread.Length) * SPEED;
			Bottom = Vector2.Lerp(currentThread.StartPoint, currentThread.EndPoint, progress);
			if (Main.rand.Next(200) == 0)
			{
				NewThread(false, false);
			}
		}

		private void RotateIntoPlace()
		{
			toRotation = (currentThread.EndPoint - currentThread.StartPoint).ToRotation();
			float rotDifference = ((((toRotation - npc.rotation) % 6.28f) + 9.42f) % 6.28f) - 3.14f;
			if (Math.Abs(rotDifference) < 0.15f)
			{
				npc.rotation = toRotation;
				return;
			}
			npc.rotation += Math.Sign(rotDifference) * 0.1f;
		}

		private void DrawThreads(SpriteBatch spriteBatch)
		{
			float length;
			foreach(StarThread thread in threads)
			{
				length = Math.Min(thread.Counter / THREADGROWLERP, (thread.Duration - thread.Counter) / THREADGROWLERP);
				length = Math.Min(length, 1);
				spriteBatch.Draw(Main.magicPixel, thread.StartPoint - Main.screenPosition, new Rectangle(0, 0, 1, 1), Color.Purple, (thread.EndPoint - thread.StartPoint).ToRotation(), new Vector2(0f, 1f), new Vector2(thread.Length, 5) * length, SpriteEffects.None, 0f);
			}
			StarThread thread2 = currentThread;
			spriteBatch.Draw(Main.magicPixel, thread2.StartPoint - Main.screenPosition, new Rectangle(0, 0, 1, 1), Color.Cyan, (thread2.EndPoint - thread2.StartPoint).ToRotation(), new Vector2(0f, 1f), new Vector2(thread2.Length * progress, 5), SpriteEffects.None, 0f);
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
	}
}
