using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Collections.ObjectModel;

namespace SpiritMod.Items.Halloween
{
	public class Candy : CandyBase
	{
		public static int _type;

		public override bool CloneNewInstances => true;

		public int Variant
			{ get; internal set; }


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Candy");
			Tooltip.SetDefault("Increases all stats slightly");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 30;
			item.rare = 2;
			item.maxStack = 1;

			item.useStyle = 2;
			item.useTime = item.useAnimation = 20;

			item.consumable = true;
			item.autoReuse = false;

			item.buffType = mod.BuffType("CandyBuff");
			item.buffTime = 14400;

			item.UseSound = SoundID.Item2;

			Variant = Main.rand.Next(CandyNames.Count);
		}


		internal static readonly ReadOnlyCollection<string> CandyNames =
			Array.AsReadOnly(new string[]
		{
			"Popstone",
			"Three Muskets",
			"Lhizzlers",
			"Moon Jelly Beans",
			"Silk Duds",
			"Necro Wafers",
			"Blinkroot Pop",
			"Gummy Slimes",
			"Cry Goblin",
			"Sour patch Slimes",
			"Stardust Burst",
			"Hellfire Tamales",
			"Blinkroot Patty",
			"Xenowhoppers",
			"Gem&Ms",
			"100,000 copper bar",
			"Toblerbone",
			"Delicious Looking Eye",
			"Silky Way",
			"Malted Silk Balls",
			"Cloudheads",
			"Red Devil Hots",
			"Rune Pop",
			"Nursey Kisses",
			"Skullies",
			"Firebolts",
			"Vinewrath Cane",
			"Candy Acorn",
			"Bunnyfinger",
			"Ichorice",
			"Lunatic-tac"
		});

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);

			int index = tooltips.FindIndex(tooltip => tooltip.Name.Equals("ItemName"));
			if (index >= 0)
			{
				TooltipLine name = tooltips.ElementAt(index);
				TooltipLine line = new TooltipLine(mod, "ItemNameSub", "'"+ CandyNames[Variant] +"'");
				tooltips.Insert(index + 1, line);
			}
		}


		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			tag.Add("Variant", Variant);
			return tag;
		}

		public override void Load(TagCompound tag)
		{
			Variant = tag.GetInt("Variant");
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write((byte)Variant);
		}

		public override void NetRecieve(BinaryReader reader)
		{
			Variant = reader.ReadByte();
		}
	}
}
