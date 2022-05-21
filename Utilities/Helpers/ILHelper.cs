using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.OS;
using System;
using System.IO;
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

		//Code adapted from https://github.com/absoluteAquarian
		public static void CompleteLog(ILCursor c, bool beforeEdit = false)
		{
			int index = 0;

			//Get the method name
			string method = c.Method.Name;
			if (!method.Contains("ctor"))
				method = method.Substring(method.LastIndexOf(':') + 1);
			else
				method = method.Substring(method.LastIndexOf('.'));

			if (beforeEdit)
				method += " - Before";
			else
				method += " - After";

			//And the storage path
			string path = Platform.Current.GetStoragePath();
			path = Path.Combine(path, "Terraria", "ModLoader", "TechMod");
			Directory.CreateDirectory(path);

			//Get the class name
			string type = c.Method.Name;
			type = type.Substring(0, type.IndexOf(':'));
			type = type.Substring(type.LastIndexOf('.') + 1);

			FileStream file = File.Open(Path.Combine(path, $"{type}.{method}.txt"), FileMode.Create);
			using (StreamWriter writer = new StreamWriter(file))
			{
				writer.WriteLine(DateTime.Now.ToString("'['ddMMMyyyy '-' HH:mm:ss']'"));
				writer.WriteLine($"// ILCursor: {c.Method}");
				do
				{
					PrepareInstruction(c.Instrs[index], out string offset, out string opcode, out string operand);

					writer.WriteLine($"{offset,-10}{opcode,-12} {operand}");
					index++;
				} while (index < c.Instrs.Count);
			}
		}

		//Also adapted from https://github.com/absoluteAquarian
		private static void PrepareInstruction(Instruction instr, out string offset, out string opcode, out string operand)
		{
			offset = $"IL_{instr.Offset:X4}:";

			opcode = instr.OpCode.Name;

			if (instr.Operand is null)
				operand = "";
			else if (instr.Operand is ILLabel label)
				operand = $"IL_{label.Target.Offset:X4}";
			else
				operand = instr.Operand.ToString();
		}
	}
}
