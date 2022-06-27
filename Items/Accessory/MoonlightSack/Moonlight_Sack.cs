using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Collections.Generic;

namespace SpiritMod.Items.Accessory.MoonlightSack
{
	public class Moonlight_Sack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonlight Sack");
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 34;
			Item.value = Item.sellPrice(gold: 3);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Player player = Main.player[Item.playerIndexTheItemIsReservedFor];
			float num7 = 5E-06f;
			float num8 = 15 * player.minionDamage;
			var aga = new TooltipLine(Mod, "summonDamageText", (int)(num8+num7) + " summon damage");
			tooltips.Add(aga);
			var aga2 = new TooltipLine(Mod, "summonDamageText2", "Creates a chain of lightning between you and your minions that deal damage");
			tooltips.Add(aga2);
		}
		public override void UpdateEquip(Player player)
		{
			player.GetSpiritPlayer().moonlightSack = true;
		}		
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) 
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			Rectangle aga = TextureAssets.Item[Item.type].Value.Frame(1, 1, 0, 0);
			Lighting.AddLight(new Vector2(Item.Center.X, Item.Center.Y), 0.075f, 0.231f, 0.255f);
			var vector2_3 = new Vector2((float) (TextureAssets.Item[Item.type].Value.Width / 2), (float) (TextureAssets.Item[Item.type].Value.Height / 1 / 2));
			spriteBatch.Draw(TextureAssets.Item[Item.type].Value, Item.Center - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(aga), lightColor, rotation, vector2_3, Item.scale, spriteEffects, 0);
			float addY = 0f;
			float addHeight = -2f;
			int num7 = 5;
			float num9 = (float) (Math.Cos((double) Main.GlobalTimeWrappedHourly % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 1.0 + 0.5);
			float num8 = 0f;
			Texture2D texture = TextureAssets.Item[Item.type].Value;
			float num10 = 0.0f;
			Vector2 bb = Item.Center - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / 1)) * Item.scale / 2f + vector2_3 * Item.scale + new Vector2(0.0f, addY + addHeight);
			Color color2 = new Color((int) sbyte.MaxValue - Item.alpha, (int) sbyte.MaxValue - Item.alpha, (int) sbyte.MaxValue - Item.alpha, 0).MultiplyRGBA(Microsoft.Xna.Framework.Color.White);
			for (int index2 = 0; index2 < num7; ++index2)
			{
				Color newColor2 = color2;
				Color faa = Item.GetAlpha(newColor2) * (1f - num8);
				Vector2 position2 = Item.Center + ((float) ((double) index2 / (double) num7 * 6.28318548202515) + rotation + num10).ToRotationVector2() * (float) (2.0 * (double) num8 + 2.0) - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / 1)) * Item.scale / 2f + vector2_3 * Item.scale + new Vector2(0.0f, addY + addHeight);
				Main.spriteBatch.Draw(Mod.GetTexture("Items/Accessory/MoonlightSack/Moonlight_Sack_Glow"), position2, new Microsoft.Xna.Framework.Rectangle?(aga), faa, rotation, vector2_3, Item.scale, spriteEffects, 0.0f);
			}
			for (int index2 = 0; index2 < 4; ++index2)
			{
				Color newColor2 = color2;
				Color faa = Item.GetAlpha(newColor2) * (1f - num9);
				Vector2 position2 = Item.Center + ((float) ((double) index2 / (double) 4 * 6.28318548202515) + rotation + num10).ToRotationVector2() * (float) (4.0 * (double) num9 + 2.0) - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / 1)) * Item.scale / 2f + vector2_3 * Item.scale + new Vector2(0.0f, addY + addHeight);
				Vector2 pos2 = Item.Center + ((float) ((double) index2 / (double) 4 * 6.28318548202515) + rotation + num10).ToRotationVector2() * (float) (2.0 * (double) num9 + 2.0) - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / 1)) * Item.scale / 2f + vector2_3 * Item.scale + new Vector2(0.0f, addY + addHeight);
				Main.spriteBatch.Draw(Mod.GetTexture("Items/Accessory/MoonlightSack/Moonlight_Sack_Glow"), pos2, new Microsoft.Xna.Framework.Rectangle?(aga), color2, rotation, vector2_3, Item.scale, spriteEffects, 0.0f);
			}
			Main.spriteBatch.Draw(Mod.GetTexture("Items/Accessory/MoonlightSack/Moonlight_Sack_Glow"), bb, new Microsoft.Xna.Framework.Rectangle?(aga), color2, rotation, vector2_3, Item.scale, spriteEffects, 0.0f);
			return false;
		}
	}
}