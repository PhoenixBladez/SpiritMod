using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.Masks
{
	[AutoloadEquip(EquipType.Head)]
	public class StarplateMask : ModItem
	{
		public static int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starplate Voyager Mask");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/Masks/StarplateMask_Head_Glow");

		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.08f, .4f, .28f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				mod.GetTexture("Items/Armor/Masks/StarplateMask_Glow"),
				new Vector2
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f,
					item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}////
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			glowMaskColor = Color.White;
		}
		int timer = 0;
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;

			item.value = 3000;
			item.rare = 1;
			item.vanity = true;
		}
	}
}
