using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
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
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Weapon/Magic/GoblinSorcererStaff_Glow");
		}


		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 50;
			Item.value = Item.buyPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Green;
			Item.damage = 13;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.mana = 10;
			Item.knockBack = 3;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item21;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.GobSorcererOrb>();
			Item.shootSpeed = 8f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
        {
            SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact, player.Center);
            Projectile.NewProjectile(source, position.X, position.Y, 0f, -4f, type, damage, knockback, player.whoAmI, velocity.X, velocity.Y);
            return false;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				ModContent.Request<Texture2D>("SpiritMod/Items/Weapon/Magic/GoblinSorcererStaff_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
				new Vector2
				(
					Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
					Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f
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
