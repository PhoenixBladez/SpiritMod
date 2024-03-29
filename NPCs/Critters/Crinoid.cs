using SpiritMod.Items.Consumable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.NPCs.Critters
{
	public class Crinoid : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crinoid");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.dontCountMe = true;
			npc.width = 22;
			npc.height = 22;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 30;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)ModContent.ItemType<CrinoidItem>();
			npc.knockBackResist = 0f;
			npc.aiStyle = 0;
			npc.npcSlots = 0;
			npc.alpha = 255;
			aiType = NPCID.WebbedStylist;
        }
		public bool hasPicked = false;
		int pickedType;
		public override void AI()
        {
			if (npc.alpha > 0)
            {
				npc.alpha -= 5;
            }
			if (!hasPicked)
            {
				npc.scale = Main.rand.NextFloat(.6f, 1f);
				pickedType = Main.rand.Next(0, 3);
				hasPicked = true;

			}
        }

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.22f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
			npc.frame.X = 46 * pickedType;
			npc.frame.Width = 46;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(pickedType);
			writer.Write(hasPicked);

		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			pickedType = reader.ReadInt32();
			hasPicked = reader.ReadBoolean();

		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height * 0.5f));
			Vector2 drawPos = npc.Center - Main.screenPosition + drawOrigin + new Vector2(-26, -18);
			Color color = npc.GetAlpha(lightColor);
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
			return false;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
            if (npc.life <= 0)
            {
				for (int i = 0; i < 6; i++)
				{
					if (pickedType == 0)
					{
						Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crionid/PinkCrionid1"), Main.rand.NextFloat(.5f, 1.2f));
						Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crionid/PinkCrionid2"), Main.rand.NextFloat(.5f, 1.2f));
					}
					if (pickedType == 1)
					{
						Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crionid/RedCrionid1"), Main.rand.NextFloat(.5f, 1.2f));
						Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crionid/RedCrionid2"), Main.rand.NextFloat(.5f, 1.2f));
					}
					if (pickedType == 2)
					{
						Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crionid/YellowCrionid1"), Main.rand.NextFloat(.5f, 1.2f));
						Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crionid/YellowCrionid2"), Main.rand.NextFloat(.5f, 1.2f));
					}

				}
            }
        }
	}
}
