using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;


namespace SpiritMod.Items.Material
{
    public class CosmiliteShard : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astralite Shard");
			Tooltip.SetDefault("'It seems that Starplate entities have been scouring the stars looking for this'");
		}


        public override void SetDefaults()
        {

            item.useStyle = 1;
            item.useTime = 10;
            item.useAnimation = 15;
            item.width = 24;
            item.height = 26;
            item.value = 100;
            item.rare = 3;
            item.maxStack = 999;
            item.createTile = mod.TileType("Glowstone");
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float  scale, int whoAmI) 	
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Material/CosmiliteShard_Glow"),
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
