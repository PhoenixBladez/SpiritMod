using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween.DevMasks
{
    [AutoloadEquip(EquipType.Head)]
    public class MaskLeemyy : ModItem
    {
		public static ModItem _ref;
		public static Texture2D _glow;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leemyy's Mask");
            Tooltip.SetDefault("Vanity item \n'Great for impersonating devs!'");
			if (!Main.dedServ)
				_glow = mod.GetTexture("Items/Halloween/DevMasks/MaskLeemyy_Head_Glow");
        }


        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 3000;
            item.rare = 9;
        }

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawHair = true;
			drawAltHair = true;
		}
	}

	public class MaskLeemyyDraw : ModPlayer
	{
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			int head = layers.FindIndex(l => l == PlayerLayer.Head);
			if (head < 0)
				return;

			layers.Insert(head + 1, new PlayerLayer(mod.Name, "HeadGlow",
				delegate(PlayerDrawInfo draw)
			{
				Player player = draw.drawPlayer;
				if (player.head != MaskLeemyy._ref.item.headSlot)
					return;

				Color alpha = Color.Multiply(Color.White, draw.upperArmorColor.A * (1f / 255));
				DrawData data = new DrawData(MaskLeemyy._glow, new Vector2((float)((int)(draw.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(draw.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.headPosition + draw.headOrigin, player.bodyFrame, alpha, player.headRotation, draw.headOrigin, 1f, draw.spriteEffects, 0);
				Main.playerDrawData.Add(data);
			}));
		}
	}
}
