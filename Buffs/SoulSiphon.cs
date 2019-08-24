using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class SoulSiphon : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Soul Siphon");
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			int num = 1100;
			for (int i = 0; i < 255; ++i)
			{
				if (Main.player[i].active && !Main.player[i].dead && ((npc.Center - Main.player[i].position).Length() < num && Main.player[i].inventory[Main.player[i].selectedItem].type == mod.ItemType("SoulSiphon") && Main.player[i].itemAnimation > 0))
				{
					if (i == Main.myPlayer)
						++Main.player[i].GetModPlayer<MyPlayer>(mod).soulSiphon;
					if (Main.rand.Next(3) != 0)
					{
						Vector2 center = npc.Center;
						center.X += Main.rand.Next(-100, 100) * 0.05F;
						center.Y += Main.rand.Next(-100, 100) * 0.05F;
						center += npc.velocity;
						Dust newDust = Main.dust[Dust.NewDust(center, 1, 1, mod.DustType("SoulSiphonDust"), 0.0f, 0.0f, 0, default(Color), 1f)];
						newDust.velocity *= 0.0f;
						newDust.scale = Main.rand.Next(70, 85) * 0.01f;
						newDust.fadeIn = i + 1;
					}
				}
			}
		}
	}
}
