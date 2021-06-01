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
		public int Duration = 800;
		public int Length;

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
		private bool initialized = false;
		private float toRotation = 0; //The rotation for the spider to rotate to

		private const float SPEED = 2;
		private const int THREADDURATION = 800;
		private const float THREADGROWLERP = 15;

		private Vector2 bottom
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
			SpiritMod.Metaballs.EnemyLayer.Sprites.Add(this);
		}

		public override void AI()
		{
			if (!initialized)
			{
				initialized = true;
				NewThread(true, true);
			}
			TraverseThread();
			RotateIntoPlace();
			UpdateThreads();
			if (progress >= 1)
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

			Vector2 startPos = bottom; 
			int maxDistance = Main.rand.Next(200, 500);
			if (!mainThread)
				maxDistance = (int)(maxDistance * 0.5f);
			int i = 0;

			Vector2 direction = Vector2.Zero;
			int tries = 0;
			while (i < 32)
			{
				direction = Main.rand.NextFloat(6.28f).ToRotationVector2();
				for (i = 16; i < maxDistance; i++)
				{
					Vector2 toLookAt = startPos + (direction * i);
					if (Framing.GetTileSafely((int)(toLookAt.X / 16), (int)(toLookAt.Y / 16)).active() && Main.tileSolid[Framing.GetTileSafely((int)(toLookAt.X / 16), (int)(toLookAt.Y / 16)).type])
						break;
				}
				tries++;
				if (tries > 30)
					break;
			}
			StarThread thread = new StarThread(startPos, startPos + (direction * i));
			if (mainThread)
			{
				currentThread = thread;
				progress = 0;
			}
			else
			{
				threads.Add(thread);
				thread.Duration -= (currentThread.Counter + (int)THREADGROWLERP);
			}
		}

		private void TraverseThread()
		{
			progress += (1f / currentThread.Length) * SPEED;
			bottom = Vector2.Lerp(currentThread.StartPoint, currentThread.EndPoint, progress);
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
				Main.spriteBatch.Draw(Main.magicPixel, thread.StartPoint - Main.screenPosition, new Rectangle(0, 0, 1, 1), Color.Purple, (thread.EndPoint - thread.StartPoint).ToRotation(), new Vector2(0f, 1f), new Vector2(thread.Length * length, 5 * length), SpriteEffects.None, 0f);
			}
			StarThread thread2 = currentThread;
			Main.spriteBatch.Draw(Main.magicPixel, thread2.StartPoint - Main.screenPosition, new Rectangle(0, 0, 1, 1), Color.Cyan, (thread2.EndPoint - thread2.StartPoint).ToRotation(), new Vector2(0f, 1f), new Vector2(thread2.Length * progress, 5), SpriteEffects.None, 0f);
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
