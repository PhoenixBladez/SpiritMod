using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BowsMisc.Morningtide
{
	public class Morningtide : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morningtide");
			Tooltip.SetDefault("Converts wooden arrows into Dawnstrike Shafts");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/BowsMisc/Morningtide/Morningtide_Glow");
        }



		public override void SetDefaults()
		{
			item.damage = 55;
			item.noMelee = true;
			item.ranged = true;
			item.width = 20;
			item.height = 38;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Shuriken;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 5;
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.DD2_GhastlyGlaiveImpactGhost;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.autoReuse = true;
			item.shootSpeed = 17f;

		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            scale = .85f;
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                ModContent.GetTexture("SpiritMod/Items/Sets/BowsMisc/Morningtide/Morningtide_Glow"),
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
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.WoodenArrowFriendly) {
				type = ModContent.ProjectileType<MorningtideProjectile>();
			}
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
			return false;
		}
	}
}