using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SpiritMod.NPCs.Vulture_Matriarch
{
	public class Vulture_Matriarch_Player : ModPlayer
	{
		public bool goldified = false;
		
		public override void ResetEffects()
		{
			goldified = false;
		}
		
		public override void UpdateDead()
		{
			goldified = false;
		}
		
		public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			if (goldified)
			{
				if (Main.rand.Next(5) == 0 && drawInfo.shadow == 0f)
				{
					int index = Dust.NewDust(new Vector2((float)player.getRect().X, (float)player.getRect().Y), player.getRect().Width, player.getRect().Height, 228, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index].scale = 1.5f;
					Main.dust[index].noGravity = true;
					Main.dust[index].velocity *= 1.1f;
					Main.playerDrawDust.Add(index);
				}
				fullBright = false;
			}
		}
		
		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit,
			ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (goldified)
			{
				damageSource = PlayerDeathReason.ByCustomReason(player.name + " died solid as a gold bar");
				playSound = false;
				Main.PlaySound(SoundID.Item37, player.position);
			}
			return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
		}
	}
}