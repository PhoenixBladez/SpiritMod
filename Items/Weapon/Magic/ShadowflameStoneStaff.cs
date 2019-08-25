using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class ShadowflameStoneStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowbreak Wand");
			Tooltip.SetDefault("Shoots out three bolts of Shadowflame rapidly");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Magic/ShadowflameStoneStaff_Glow");
		}


		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 46;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = 2;
			item.damage = 13;
			item.knockBack = 4;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.useTime = 12;
			item.useAnimation = 36;
			item.mana = 9;
			item.magic = true;
            item.autoReuse = false;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("ShadowflameStoneBolt");
			item.shootSpeed = 10f;
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float  scale, int whoAmI) 	
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Weapon/Magic/ShadowflameStoneStaff_Glow"),
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
