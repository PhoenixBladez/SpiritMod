using Terraria;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace SpiritMod
{
	public static class AdditiveCallManager
	{
		private static uint MaxCalls => 1000;

		private static IDrawAdditive[] AdditiveCalls;

		public static void DrawAdditiveCalls(SpriteBatch sb)
		{
			List<IDrawAdditive> CallList = new List<IDrawAdditive>();
			//NPCs and Projectiles have the luxury of getting AutoLoaded into the batch

			foreach (Projectile p in Main.projectile)
			{
				var mP = p.modProjectile;
				if (mP is IDrawAdditive && p.active)
					CallList.Add(mP as IDrawAdditive);
			}

			foreach (NPC n in Main.npc)
			{
				var nP = n.modNPC;
				if (nP is IDrawAdditive && n.active)
					CallList.Add(nP as IDrawAdditive);
			}
			// Custom Additive calls do not. Use this for particles and such

			for (int i = 0; i < MaxCalls; i++)
				if (AdditiveCalls[i] != null) CallList.Add(AdditiveCalls[i]);

			//Only restart spritebatch if needed
			if(CallList.Count > 0)
			{
				Main.spriteBatch.Begin(default, BlendState.Additive, default, default, RasterizerState.CullNone, default, Main.GameViewMatrix.ZoomMatrix);
				foreach (IDrawAdditive drawAdditive in CallList)
					drawAdditive.AdditiveCall(sb);

				Main.spriteBatch.End();
			}
		}
		//Use this return to Dispose
		public static int ManualAppend(IDrawAdditive IDA)
		{
			for (int i = 0; i < AdditiveCalls.Length; i++) {
				if (AdditiveCalls[i] == null) {
					AdditiveCalls[i] = IDA;
					return i;
				}
			}
			throw new NullReferenceException("Max Calls Reached. Calm the fuck down");
		}
		//A bit difficult to manage, but better for performance. 
		public static void RemoveCall(int Index) => AdditiveCalls[Index] = null;

		public static void Load() => AdditiveCalls = new IDrawAdditive[MaxCalls];
		public static void Unload() => AdditiveCalls = null;
	}
}
