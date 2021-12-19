using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;

namespace SpiritMod.Utilities.Helpers
{
	public static class ILHelper
	{
		public static void InGameDebug<T>(this ILCursor cursor)
		{
			cursor.Emit(OpCodes.Dup);
			cursor.EmitDelegate<Action<T>>((T v) => { Main.NewText(v); });
		}
	}
}
