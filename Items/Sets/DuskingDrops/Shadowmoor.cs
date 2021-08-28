using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.Items.Sets.DuskingDrops
{
	public class Shadowmoor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowmoor");
            Tooltip.SetDefault("Converts wooden arrows into Shadow Wisps");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/DuskingDrops/Shadowmoor_Glow");
        }



		public override void SetDefaults()
		{
			item.damage = 35;
			item.noMelee = true;
			item.ranged = true;
			item.width = 26;
			item.height = 62;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ModContent.ProjectileType<ShadowmoorProjectile>();
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 3.25f;
			item.value = Terraria.Item.sellPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.shootSpeed = 16.2f;
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
                ModContent.GetTexture("SpiritMod/Items/Sets/DuskingDrops/Shadowmoor_Glow"),
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
				type = ModContent.ProjectileType<ShadowmoorProjectile>();
                damage = (int)(damage * .75f);
            }
			else
            {
                float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
                Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 20f;
                if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0))
                {
                    position += spawnPlace;
                }

                Vector2 velocity = Vector2.Normalize(Main.MouseWorld - position) * item.shootSpeed;
                DustHelper.DrawDiamond(new Vector2(position.X, position.Y), 173, 2, .8f, .75f);
                int p1 = Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ShadowmoorProjectile>(), damage, knockBack, 0, 0.0f, 0.0f);

            }
            for (int I = 0; I < 4; I++)
            {
                float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
                Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 20f;
                if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0))
                {
                    position += spawnPlace;
                }

                Vector2 velocity = Vector2.Normalize(Main.MouseWorld - position) * item.shootSpeed;
                DustHelper.DrawDiamond(new Vector2(position.X, position.Y), 173, 2, .8f, .75f);
                int p = Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockBack, 0, 0.0f, 0.0f);
            }
			return false;
		}
	}
}