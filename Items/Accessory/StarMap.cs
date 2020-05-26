using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Accessory
{
    public class StarMap : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Map");
			Tooltip.SetDefault("Increases movement speed by 10% and critical strike chance by 4% \nGetting hurt spawns stars from the sky\n'Let the stars guide you'");
		}


        public override void SetDefaults()
        {
            item.width = 34;     
            item.height = 56;   
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 2;
            item.defense = 2;
            item.expert = true;
            item.accessory = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetSpiritPlayer().starMap = true;
            player.maxRunSpeed += .1f;
            player.meleeCrit += 4;
            player.magicCrit += 4;
            player.rangedCrit += 4;
        }
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float  scale, int whoAmI) 	
		{
			Lighting.AddLight(item.position, 0.08f, .28f, .38f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Accessory/StarMap_Glow"),
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
        }
    }
}
