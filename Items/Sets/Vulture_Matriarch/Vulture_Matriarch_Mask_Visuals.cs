using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;

namespace SpiritMod.Items.Sets.Vulture_Matriarch
{
	public class Vulture_Matriarch_Mask_Visuals : ModPlayer
	{
		public static bool maskEquipped = false;
		public override void ResetEffects() => maskEquipped = false;
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			int head = layers.FindIndex(l => l == PlayerLayer.Head);
			if (head < 0)
				return;

			layers.Insert(head, new PlayerLayer(Mod.Name, "Head", delegate (PlayerDrawInfo drawInfo)
			{
				if (drawInfo.shadow != 0f)
					return;
				Player drawPlayer = drawInfo.drawPlayer;

				if ((maskEquipped && drawPlayer.armor[10].type == ItemID.None) || drawPlayer.armor[10].type == Mod.Find<ModItem>("Vulture_Matriarch_Mask").Type)
				{
					Mod mod = ModLoader.GetMod("SpiritMod");
					Vector2 Position = drawPlayer.position;
					SpriteEffects spriteEffects;
					if (drawPlayer.gravDir == 1.0)
					{
						if (drawPlayer.direction == 1)
							spriteEffects = SpriteEffects.None;
						else
							spriteEffects = SpriteEffects.FlipHorizontally;
					}
					else
					{
						if (drawPlayer.direction == 1)
							spriteEffects = SpriteEffects.FlipVertically;
						else
							spriteEffects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
					}
					Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)(Position.X + drawPlayer.width * 0.5) / 16, (int)(Position.Y + drawPlayer.height * 0.5) / 16, Color.White), drawPlayer.shadow);
					Texture2D helmTexture = mod.GetTexture("Items/Sets/Vulture_Matriarch/Vulture_Matriarch_Mask_Head_2");

					Vector2 helmPos = new Vector2((int)(drawInfo.position.X - Main.screenPosition.X) + ((drawInfo.drawPlayer.width - drawInfo.drawPlayer.bodyFrame.Width) / 2), (int)(drawInfo.position.Y - Main.screenPosition.Y) + drawInfo.drawPlayer.height - drawInfo.drawPlayer.bodyFrame.Height - 2) + drawInfo.drawPlayer.headPosition + drawInfo.headOrigin;
					DrawData drawData3 = new DrawData(helmTexture, helmPos, new Microsoft.Xna.Framework.Rectangle?(drawPlayer.bodyFrame), color12, drawInfo.drawPlayer.headRotation, drawInfo.headOrigin, 1f, spriteEffects, 0);
					Main.playerDrawData.Add(drawData3);

				}

			}));
		}
	}
}