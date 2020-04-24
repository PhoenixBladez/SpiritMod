using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class AstralLens : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Convergence");
			Tooltip.SetDefault("Shoots out bursts of electrical stars that reconverge on the player");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Magic/AstralLens_Glow");
		}


		public override void SetDefaults()
		{
			item.damage = 23;
			item.magic = true;
			item.mana = 8;
			item.width = 44;
			item.height = 46;
			item.useTime = 23;
			item.useAnimation = 23;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 6;
            item.value = Item.sellPrice(0, 01, 10, 0);
            item.rare = 3;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("Starshock1");
			item.shootSpeed = 46f;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int I = 0; I < 2; I++)
            {
                Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-250, 250) / 30), speedY + ((float)Main.rand.Next(-250, 250) / 30), type, damage, knockBack, player.whoAmI, 0f, 0f);
                if (Main.rand.Next(6) == 0)
                {
                    Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-250, 250) / 30), speedY + ((float)Main.rand.Next(-250, 250) / 30), type, damage, knockBack, player.whoAmI, 0f, 0f);
                }
            }
            return false;
        }
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float  scale, int whoAmI) 	
		{
			Lighting.AddLight(item.position, 0.08f, .28f, .38f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Weapon/Magic/AstralLens_Glow"),
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