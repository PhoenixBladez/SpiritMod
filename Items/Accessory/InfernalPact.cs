
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

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernal Pact");
			Tooltip.SetDefault("+4% all damage\nReduces your defense to 0\nAdds 0.75% all damage per defense point lost");
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 28;
			Item.value = Item.buyPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.allDamage *= 1 + (player.statDefense * 0.0075f);
			player.statDefense = 0;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) //Glowmask
		{
			Texture2D glowmask = Mod.Assets.Request<Texture2D>("Items/Accessory/InfernalPact_Glow").Value;
			var drawPos = new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - glowmask.Height * 0.5f + 2f); //Jesus this line
			float sine = (float)Math.Sin(sineTimer++ * 0.08f);
			Color col = Item.GetAlpha(Color.White) * (0.5f - sine);
			spriteBatch.Draw(glowmask, drawPos, new Rectangle(0, 0, glowmask.Width, glowmask.Height), col, rotation, glowmask.Size() * 0.5f, scale + (sine * 0.09f), SpriteEffects.None, 0f);
		}
	}
}