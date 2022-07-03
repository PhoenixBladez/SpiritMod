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
		public bool maskEquipped = false;
		public override void ResetEffects() => maskEquipped = false;
	}

	public class VultureMatriachMaskLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Head);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.shadow != 0f)
				return;
			Player drawPlayer = drawInfo.drawPlayer;

			if ((drawPlayer.GetModPlayer<Vulture_Matriarch_Mask_Visuals>().maskEquipped && drawPlayer.armor[10].type == ItemID.None) || drawPlayer.armor[10].type == Mod.Find<ModItem>("Vulture_Matriarch_Mask").Type)
			{
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
				Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)(Position.X + drawPlayer.width * 0.5) / 16, (int)(Position.Y + drawPlayer.height * 0.5) / 16, Color.White), drawInfo.shadow);
				Texture2D helmTexture = ModContent.Request<Texture2D>("Items/Sets/Vulture_Matriarch/Vulture_Matriarch_Mask_Head_2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

				Vector2 helmPos = new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X) + ((drawInfo.drawPlayer.width - drawInfo.drawPlayer.bodyFrame.Width) / 2), (int)(drawInfo.Position.Y - Main.screenPosition.Y) + drawInfo.drawPlayer.height - drawInfo.drawPlayer.bodyFrame.Height - 2) + drawInfo.drawPlayer.headPosition + drawInfo.headOrigin;
				DrawData drawData3 = new DrawData(helmTexture, helmPos, new Microsoft.Xna.Framework.Rectangle?(drawPlayer.bodyFrame), color12, drawInfo.drawPlayer.headRotation, drawInfo.headOrigin, 1f, spriteEffects, 0);
				drawInfo.DrawDataCache.Add(drawData3);
			}
		}
	}
}