using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;

namespace SpiritMod.Mechanics.CollideableNPC
{
	class CollideableNPCDetours
	{
		internal static void SolidTopCollision(On.Terraria.Player.orig_Update_NPCCollision orig, Player self)
		{
			for (int i = 0; i < Main.maxNPCs; ++i)
			{
				NPC npc = Main.npc[i];

				if (!npc.active || npc.modNPC == null || !(npc.modNPC is ISolidTopNPC))
					continue;

				var groundBox = new Rectangle((int)self.position.X, (int)self.position.Y + self.height, self.width, 1);
				var floorBox = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, 6 + (int)Math.Max(self.velocity.Y, 0));

				if (groundBox.Intersects(floorBox) && self.velocity.Y > 0)
				{
					self.gfxOffY = npc.gfxOffY;
					self.position.Y = npc.position.Y - self.height + 4;
					self.velocity.Y = 0;
					self.fallStart = (int)(self.position.Y / 16f);

					if (self == Main.LocalPlayer)
						NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, Main.LocalPlayer.whoAmI);

					orig(self);
				}
			}

			orig(self);
		}
	}
}
