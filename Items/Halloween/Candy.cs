using SpiritMod.Buffs.Candy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SpiritMod.Items.Halloween
{
	public class Candy : CandyBase
	{

		protected override bool CloneNewInstances => true;

		public int Variant { get; internal set; }


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Candy");
			Tooltip.SetDefault("Increases all stats slightly");
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 30;
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 1;

			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 20;

			Item.consumable = true;
			Item.autoReuse = false;

			Item.buffType = ModContent.BuffType<CandyBuff>();
			Item.buffTime = 14400;

			Item.UseSound = SoundID.Item2;

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
			if (index >= 0) {
				TooltipLine name = tooltips.ElementAt(index);
				TooltipLine line = new TooltipLine(Mod, "ItemNameSub", "'" + CandyNames[Variant] + "'");
				tooltips.Insert(index + 1, line);
			}
		}
		public override void SaveData(TagCompound tag)
		{
			tag.Add("Variant", Variant);
		}

		public override void LoadData(TagCompound tag)
		{
			Variant = tag.GetInt("Variant");
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write((byte)Variant);
		}

		public override void NetReceive(BinaryReader reader)
		{
			Variant = reader.ReadByte();
		}
	}
}
