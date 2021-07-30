using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	//Currently used to cut down on boilerplate for animation and platform logic, might add more later
	public abstract class SpiritNPC : ModNPC 
	{

		public Point frame = new Point(0, 0);

		public bool ignorePlatforms = false;

		public delegate void UpdateFrameAction(int frameNumber);

		/// <summary>
		/// Animates the NPC vertically, automatically setting its frame to the start frame if before it, and looping back to the minimum frame if past the ending frame
		/// </summary>
		/// <param name="framespersecond"></param>
		/// <param name="minframe"></param>
		/// <param name="maxframe"></param>
		public void UpdateYFrame(int framespersecond, int startframe, int endframe, UpdateFrameAction action = null, int? loopFrame = null)
		{
			npc.frameCounter++;
			bool reverse = startframe > endframe; //invert the looping logic and decrease frame height if the ending frame is before the starting frame, to allow updating frames in reverse

			void LoopCheck()
			{
				bool loopcheck = (reverse) ? (frame.Y < endframe || frame.Y > startframe) : (frame.Y > endframe || frame.Y < startframe);
				if (loopcheck)
					frame.Y = loopFrame ?? startframe;
			}

			LoopCheck();
			if (framespersecond == 0)
				return;

			if (npc.frameCounter >= (60 / framespersecond))
			{
				frame.Y += (reverse) ? -1 : 1;
				npc.frameCounter = 0;
				LoopCheck();

				if (action != null)
					action.Invoke(frame.Y);
			}
		}

		public override bool PreAI()
		{
			if (ignorePlatforms)
			{
				bool onplatform = true;
				for (int i = (int)npc.position.X; i < npc.position.X + npc.width; i += npc.width / 4)
				{
					Tile tile = Framing.GetTileSafely(new Point((int)npc.position.X / 16, (int)(npc.position.Y + npc.height + 8) / 16));
					if (!TileID.Sets.Platforms[tile.type])
						onplatform = false;
				}
				if (onplatform)
					npc.noTileCollide = true;
				else
					npc.noTileCollide = false;
			}

			return SafePreAI();
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(npc.frameCounter);
			writer.WriteVector2(frame.ToVector2());
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			npc.frameCounter = reader.ReadDouble();
			frame = reader.ReadVector2().ToPoint();
		}

		/// <summary>
		/// Simple helper method to make an npc pass through platforms when above the player, but not when at the same height as them
		/// </summary>
		/// <param name="player"></param>
		public void PlayerPlatformCheck(Player player) => ignorePlatforms = (npc.Bottom.Y < player.Top.Y);

		/// <summary>
		/// Use in cases where PreAI would be used, due to PreAI being implemented by SpiritNPC
		/// </summary>
		/// <returns></returns>
		public virtual bool SafePreAI() => true;

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
				OnHitKill(hitDirection, damage);

			SafeHitEffect(hitDirection, damage);
		}

		/// <summary>
		/// Use in cases where HitEffect would be used, due to HitEffect being implemented by SpiritNPC
		/// </summary>
		/// <param name="hitDirection"></param>
		/// <param name="damage"></param>
		public virtual void SafeHitEffect(int hitDirection, double damage) { }

		/// <summary>
		/// Called in HitEffect when the npc's life is below or equal to 0, use for any gores or the like
		/// </summary>
		/// <param name="hitDirection"></param>
		/// <param name="damage"></param>
		public virtual void OnHitKill(int hitDirection, double damage) { }

		public override void FindFrame(int frameHeight)
		{
			SafeFindFrame(frameHeight);

			npc.frame.Y = frame.Y * frameHeight;
			npc.frame.X = frame.X * npc.frame.Width;
		}

		/// <summary>
		/// Called before SpiritNPC's implementation of FindFrame, use to define specific frame sizes or the like.
		/// </summary>
		/// <param name="frameHeight"></param>
		public virtual void SafeFindFrame(int frameHeight) { }
	}
}