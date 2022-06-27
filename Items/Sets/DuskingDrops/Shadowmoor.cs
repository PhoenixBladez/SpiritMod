using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.DuskingDrops
{
	public class Shadowmoor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowmoor");
            Tooltip.SetDefault("Converts wooden arrows into Shadow Wisps");
            SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/DuskingDrops/Shadowmoor_Glow");
        }

		public override void SetDefaults()
		{
			Item.damage = 35;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 26;
			Item.height = 62;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<ShadowmoorProjectile>();
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 3.25f;
			Item.value = Terraria.Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shootSpeed = 16.2f;
		}

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.WoodenArrowFriendly)
			{
				type = ModContent.ProjectileType<ShadowmoorProjectile>();
				damage = (int)(damage * .75f);
			}
			else
			{
				float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
				Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 20f;
				if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0))
					position += spawnPlace;
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (type != ProjectileID.WoodenArrowFriendly)
			{ 
                Vector2 vel = Vector2.Normalize(Main.MouseWorld - position) * Item.shootSpeed;
                DustHelper.DrawDiamond(new Vector2(position.X, position.Y), 173, 2, .8f, .75f);
                Projectile.NewProjectile(source, position.X, position.Y, vel.X, vel.Y, ModContent.ProjectileType<ShadowmoorProjectile>(), damage, knockback, 0, 0.0f, 0.0f);
            }

            for (int I = 0; I < 4; I++)
            {
                float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
                Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 20f;
                if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0))
                    position += spawnPlace;

                Vector2 vel = Vector2.Normalize(Main.MouseWorld - position) * Item.shootSpeed;
                DustHelper.DrawDiamond(new Vector2(position.X, position.Y), 173, 2, .8f, .75f);
                int p = Projectile.NewProjectile(source, position.X, position.Y, vel.X, vel.Y, type, damage, knockback, 0, 0.0f, 0.0f);
            }
			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			scale = .85f;
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				ModContent.Request<Texture2D>("SpiritMod/Items/Sets/DuskingDrops/Shadowmoor_Glow").Value,
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