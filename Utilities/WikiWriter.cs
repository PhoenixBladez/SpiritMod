using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Utilities
{
	public class WikiWriter
	{
		public static string Path => Environment.CurrentDirectory + "/Wiki/";

		public static void WriteWiki()
		{
			if (!Directory.Exists(Path))
				Directory.CreateDirectory(Path);

			WriteItems();
		}

		private static void WriteItems()
		{
			using (StreamWriter stream = new StreamWriter(Path + "Items.txt"))
			{
				var items = GetAllOf<ModItem>();

				foreach (var item in items)
				{
					int type = -1;

					try
					{
						string name = item.Name;
						type = SpiritMod.Instance.ItemType(name);

						Item itemInst = new Item();
						itemInst.SetDefaults(type);

						WriteSingleItem(stream, itemInst);
					}
					catch (Exception e)
					{
						stream.WriteLine($"Failed to write item of type {item.Name}\n" + e.Message + " at " + e.TargetSite + "\n" + e.StackTrace);
					}
				}
			}
		}

		private static void WriteSingleItem(StreamWriter writer, Item item)
		{
			string tooltip = "";
			for (int line = 0; line < item.ToolTip.Lines; ++line)
			{
				string text = item.ToolTip.GetLine(line);
				if (text != "")
					tooltip += text;
			}

			string damage = GetItemClass(item);

			List<string> lines = new List<string>
			{
				$"\"{item.Name}\"", //"Name"
				tooltip != "" ? $"Tooltip: {tooltip}" : "", //Tooltip(s)
				damage != "None" && item.damage != 0 ? $"{item.damage} {GetItemClass(item)} damage" : "", //X Class damage
				item.knockBack != 0 ? $"{item.knockBack} knockback" : "", //X knockback
				item.crit != 0 ? $"{item.crit} critical strike chance" : (item.damage == 0 ? "" : "4 critical strike chance"), //X critical strike chance
				$"Use time/animation {item.useTime}/{item.useAnimation}",
				GetRarity(item)
			};

			foreach (string line in lines)
				if (line != "")
					writer.Write(line);
		}

		private static string GetRarity(Item item)
		{
			foreach (var field in typeof(ItemRarityID).GetFields())
			{
				if (field.FieldType == typeof(int) && (int)field.GetRawConstantValue() == item.rare)
					return field.Name;
			}
			return "";
		}

		private static string GetItemClass(Item item)
		{
			if (item.melee)
				return "melee";
			else if (item.ranged)
				return "ranged";
			else if (item.summon)
				return "summon";
			else if (item.magic)
				return "magic";
			else if (item.thrown)
				return "thrown";
			return "None";
		}

		private static IEnumerable<Type> GetAllOf<T>() => SpiritMod.Instance.GetType().Assembly.GetTypes().Where(x => typeof(T).IsAssignableFrom(x) && !x.IsAbstract);
	}
}
