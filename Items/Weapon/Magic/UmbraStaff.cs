using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class UmbraStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Umbra Staff");
			Tooltip.SetDefault("Shoots out homing Shadow Balls");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Magic/UmbraStaff_Glow");
		}


		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 36;
			item.value = Item.buyPrice(0, 7, 0, 0);
			item.rare = 5;
			item.damage = 44;
			item.knockBack = 3;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.useTime = 24;
			item.useAnimation = 24;
			item.mana = 6;
			item.magic = true;
            item.autoReuse = true;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("ShadowBall_Friendly");
			item.shootSpeed = 10f;
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float  scale, int whoAmI) 	
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Weapon/Magic/UmbraStaff_Glow"),
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
