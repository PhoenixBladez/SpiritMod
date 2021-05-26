
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class InfernalPact : ModItem
	{
		private int sineTimer = 0; //maybe there's a better way to do this idk
		private Texture2D glowmask;


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernal Pact");
			Tooltip.SetDefault("+4% all damage\nReduces your defense to 0\nAdds 0.75% all damage per defense point lost");
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 28;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.allDamage *= 1 + (player.statDefense * 0.0075f);
			player.statDefense = 0;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) //Glowmask
		{
			Texture2D glowmask = mod.GetTexture("Items/Accessory/InfernalPact_Glow");
			var drawPos = new Vector2(item.position.X - Main.screenPosition.X + item.width * 0.5f, item.position.Y - Main.screenPosition.Y + item.height - glowmask.Height * 0.5f + 2f); //Jesus this line
			float sine = (float)Math.Sin(sineTimer++ * 0.08f);
			Color col = item.GetAlpha(Color.White) * (0.5f - sine);
			spriteBatch.Draw(glowmask, drawPos, new Rectangle(0, 0, glowmask.Width, glowmask.Height), col, rotation, glowmask.Size() * 0.5f, scale + (sine * 0.09f), SpriteEffects.None, 0f);
		}
	}
}