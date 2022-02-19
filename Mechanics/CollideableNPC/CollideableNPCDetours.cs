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
			if (!self.controlDown && self.grappling[0] < 0)
			{
				for (int i = 0; i < Main.maxNPCs; ++i)
				{
					NPC npc = Main.npc[i];

					if (!npc.active || npc.modNPC == null || !(npc.modNPC is ISolidTopNPC))
						continue;

					var playerBox = new Rectangle((int)self.position.X, (int)self.position.Y + self.height, self.width, 1);
					var floorBox = new Rectangle((int)npc.position.X, (int)npc.position.Y - (int)npc.velocity.Y, npc.width, 8 + (int)Math.Max(self.velocity.Y, 0));

					if (playerBox.Intersects(floorBox) && self.velocity.Y > 0 && !Collision.SolidCollision(self.Bottom, self.width, (int)Math.Max(1 + npc.velocity.Y, 0)))
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
			}

			orig(self);
		}

		internal static void Grappling(On.Terraria.Projectile.orig_VanillaAI orig, Projectile self)
		{
			if (self.aiStyle == 7 && false) //False as to not cause crashes
			{
				if (self.ai[0] == 0f)
				{
					for (int i = 0; i < Main.maxNPCs; ++i)
					{
						NPC npc = Main.npc[i];

						if (!npc.active || npc.modNPC == null || !(npc.modNPC is ISolidTopNPC topNPC) || !topNPC.Grappleable())
							continue;

						var projBox = self.getRect();
						var floorBox = new Rectangle((int)npc.position.X, (int)npc.position.Y - (int)npc.velocity.Y, npc.width, 8 + (int)Math.Max(self.velocity.Y, 0));

						if (projBox.Intersects(floorBox) && !Collision.SolidCollision(self.Bottom, self.width, 8))
						{
							self.ai[0] = 2f;
							self.velocity *= 0;
							self.netUpdate = true;

							SpiritMod.Instance.Logger.Debug("time for die");
							Main.player[self.owner].grappling[0] = self.whoAmI;

							if (self.owner == Main.myPlayer)
								NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, self.owner);
						}
					}
				}
			}

			orig(self);
		}
	}
}
