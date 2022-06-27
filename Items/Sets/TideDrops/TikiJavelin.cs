using SpiritMod.Projectiles.Thrown.Charge;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.TideDrops
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
			Item.damage = 25;
			Item.noMelee = true;
			Item.channel = true; //Channel so that you can held the weapon [Important]
			Item.rare = ItemRarityID.Orange;
			Item.width = 18;
			Item.height = 18;
			Item.useTime = 15;
			Item.useAnimation = 45;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 24;
			Item.knockBack = 8;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			//   item.UseSound = SoundID.Item20;
			Item.autoReuse = false;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<TikiJavelinProj>();
			Item.shootSpeed = 0f;
			Item.value = Item.sellPrice(0, 0, 60, 0);
		}

		/*   public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
           {
               Lighting.AddLight(item.position, 0.08f, .28f, .38f);
               Texture2D texture;
               texture = Main.itemTexture[item.type];
               spriteBatch.Draw
               (
                   ModContent.Request<Texture2D>("SpiritMod/Items/Equipment/StarMap_Glow"),
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