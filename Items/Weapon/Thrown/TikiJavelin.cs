using SpiritMod.Projectiles.Thrown.Charge;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
			item.damage = 25;
			item.noMelee = true;
			item.channel = true; //Channel so that you can held the weapon [Important]
			item.rare = ItemRarityID.Orange;
			item.width = 18;
			item.height = 18;
			item.useTime = 15;
			item.useAnimation = 45;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = item.useAnimation = 24;
			item.knockBack = 8;
			item.melee = true;
			item.noMelee = true;
			//   item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<TikiJavelinProj>();
			item.shootSpeed = 0f;
			item.value = Item.sellPrice(0, 0, 60, 0);
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