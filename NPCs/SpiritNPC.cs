using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
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

		/// <summary>Animates the NPC vertically, automatically setting its frame to the start frame if before it, and looping back to the minimum frame if past the ending frame</summary>
		/// <param name="fps">FPS of the animation.</param>
		/// <param name="startFrame">The frame to start on.</param>
		/// <param name="endFrame">The frame to end/loop on.</param>
		/// <param name="action">Action that is invoked on every frame switch.</param>
		public void UpdateYFrame(int fps, int startFrame, int endFrame, UpdateFrameAction action = null, int? loopFrame = null)
		{
			NPC.frameCounter++;
			bool reverse = startFrame > endFrame; //invert the looping logic and decrease frame height if the ending frame is before the starting frame, to allow updating frames in reverse

			void LoopCheck()
			{
				bool loopCheck = reverse ? (frame.Y < endFrame || frame.Y > startFrame) : (frame.Y > endFrame || frame.Y < startFrame);
				if (loopCheck)
					frame.Y = loopFrame ?? startFrame;
			}

			LoopCheck();
			if (fps == 0)
				return;

			if (NPC.frameCounter >= (60 / fps))
			{
				frame.Y += reverse ? -1 : 1;
				NPC.frameCounter = 0;
				LoopCheck();

				if (action != null)
					action.Invoke(frame.Y);
			}
		}

		public override bool PreAI()
		{
			if (ignorePlatforms)
			{
				bool onPlatform = true;
				for (int i = (int)NPC.position.X; i < NPC.position.X + NPC.width; i += NPC.width / 4)
				{
					Tile tile = Framing.GetTileSafely(new Point((int)NPC.position.X / 16, (int)(NPC.position.Y + NPC.height + 8) / 16));
					if (!TileID.Sets.Platforms[tile.TileType])
						onPlatform = false;
				}

				NPC.noTileCollide = onPlatform;
			}

			return SafePreAI();
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(NPC.frameCounter);
			writer.WriteVector2(frame.ToVector2());
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			NPC.frameCounter = reader.ReadDouble();
			frame = reader.ReadVector2().ToPoint();
		}

		/// <summary>
		/// Simple helper method to make an npc pass through platforms when above the player, but not when at the same height as them
		/// </summary>
		/// <param name="player"></param>
		public void PlayerPlatformCheck(Player player) => ignorePlatforms = NPC.Bottom.Y < player.Top.Y;

		/// <summary>
		/// Use in cases where PreAI would be used, due to PreAI being implemented by SpiritNPC
		/// </summary>
		/// <returns></returns>
		public virtual bool SafePreAI() => true;

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
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

			NPC.frame.Y = frame.Y * frameHeight;
			NPC.frame.X = frame.X * NPC.frame.Width;
		}

		/// <summary>
		/// Called before SpiritNPC's implementation of FindFrame, use to define specific frame sizes or the like.
		/// </summary>
		/// <param name="frameHeight"></param>
		public virtual void SafeFindFrame(int frameHeight) { }
	}
}