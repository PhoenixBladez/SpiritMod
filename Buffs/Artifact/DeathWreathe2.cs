using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Artifact
{
	public class DeathWreathe2 : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Soul Wreathe");
			Main.pvpBuff[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.boss == false)
			{
				if (!npc.friendly)
				{
					npc.lifeRegen -= 6;
					npc.defense = (npc.defDefense / 100) * 5;
					int d = Dust.NewDust(npc.position, npc.width, npc.height, 110);
					Main.dust[d].scale *= 2f;
					Main.dust[d].velocity *= 0f;
					Main.dust[d].noGravity = true;
				}
				if (!npc.friendly)
				{
					if (Main.rand.Next(100) == 1)
					{
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("SoulBurst"), 50, 0, Main.myPlayer, 0, 0);

					}
				}
			}
		}
	}
}