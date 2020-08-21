using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class GoblinSorcererStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sorcerer's Wand");
			Tooltip.SetDefault("Launches a shadowflame orb into the sky");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Magic/GoblinSorcererStaff_Glow");
		}


		public override void SetDefaults()
		{
			item.width = 48;
			item.height = 50;
			item.value = Item.buyPrice(0, 0, 30, 0);
			item.rare = ItemRarityID.Green;
			item.damage = 17;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.useTime = 24;
			item.useAnimation = 24;
			item.mana = 8;
			item.knockBack = 3;
			item.magic = true;
			item.noMelee = true;
			item.UseSound = SoundID.Item21;
			item.shoot = ModContent.ProjectileType<Projectiles.Magic.GobSorcererOrb>();
			item.shootSpeed = 8f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Main.PlaySound(SoundID.DD2_CrystalCartImpact, player.Center);
            Projectile.NewProjectile(position.X, position.Y, 0f, -4f, type, damage, knockBack, player.whoAmI, speedX, speedY);
            return false;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Weapon/Magic/GoblinSorcererStaff_Glow"),
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
