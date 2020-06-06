using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class TikiJavelin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiki Javelin");
			Tooltip.SetDefault("Hold and release to throw\nHold it longer for more velocity and damage");
          //  SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Equipment/StarMap_Glow");
        }

		public override void SetDefaults()
		{
			item.damage = 18;
			item.noMelee = true;
			item.channel = true; //Channel so that you can held the weapon [Important]
			item.rare = 5;
			item.width = 18;
			item.height = 18;
			item.useTime = 20;
            item.useAnimation = 60;
			item.useStyle = 1;
            item.useTime = item.useAnimation = 24;
            item.melee = true;
            item.noMelee = true;
            item.UseSound = SoundID.Item1;
			item.autoReuse = false;
             item.noUseGraphic = true;
			item.shoot = mod.ProjectileType("TikiJavelinProj");
			item.shootSpeed = 0f;
		}
     /*   public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(item.position, 0.08f, .28f, .38f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                ModContent.GetTexture("SpiritMod/Items/Equipment/StarMap_Glow"),
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
        }*/
    }
}
