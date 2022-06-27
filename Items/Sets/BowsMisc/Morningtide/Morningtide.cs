using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.BowsMisc.Morningtide
{
	public class Morningtide : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morningtide");
			Tooltip.SetDefault("Converts wooden arrows into Dawnstrike Shafts");
            SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/BowsMisc/Morningtide/Morningtide_Glow");
        }



		public override void SetDefaults()
		{
			Item.damage = 55;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 20;
			Item.height = 38;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.Shuriken;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 5;
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.DD2_GhastlyGlaiveImpactGhost;
			Item.value = Item.buyPrice(0, 5, 0, 0);
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.autoReuse = true;
			Item.shootSpeed = 17f;

		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            scale = .85f;
            Texture2D texture;
            texture = TextureAssets.Item[Item.type].Value;
            spriteBatch.Draw
            (
                ModContent.Request<Texture2D>("SpiritMod/Items/Sets/BowsMisc/Morningtide/Morningtide_Glow"),
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
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (type == ProjectileID.WoodenArrowFriendly) {
				type = ModContent.ProjectileType<MorningtideProjectile>();
			}
				Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 0f, 0f);
			return false;
		}
	}
}