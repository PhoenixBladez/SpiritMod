using SpiritMod.Projectiles.Summon;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon.ElectricGun
{
	public class ElectricGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcbolter");
			Tooltip.SetDefault("4 summon tag damage\nYour summons will focus struck enemies\nHit enemies may create static links between each other when struck by minions, dealing additional damage");

            SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Weapon/Summon/ElectricGun/ElectricGun_Glow");
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(Item.position, 0.08f, .4f, .28f);
            Texture2D texture;
            texture = TextureAssets.Item[Item.type].Value;
            spriteBatch.Draw
            (
                Mod.GetTexture("Items/Weapon/Summon/ElectricGun/ElectricGun_Glow"),
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
        }////

        public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = DamageClass.Summon;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 2;
			Item.value = Terraria.Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item12;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<ElectricGunProjectile>();
			Item.shootSpeed = 12f;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY - 1)) * 45f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
			Vector2 velocity = new Vector2(speedX, speedY); //convert speedx and speedy into one vector, and rotate it upwards
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(10)); //then rotate it randomly, with a maximum spread of 30 degrees
            Projectile proj = Projectile.NewProjectileDirect(position, velocity, type, Item.damage, knockback, Item.playerIndexTheItemIsReservedFor, 0, 0);
			proj.netUpdate = true; //sync velocity
            for (int index1 = 0; index1 < 5; ++index1)
            {
                int index2 = Dust.NewDust(new Vector2(position.X, position.Y), Item.width - 60, Item.height - 28, DustID.Electric, speedX, speedY, (int)byte.MaxValue, new Color(), (float)Main.rand.Next(6, 10) * 0.1f);
                Main.dust[index2].noGravity = true;
                Main.dust[index2].velocity *= 0.5f;
                Main.dust[index2].scale *= .6f;
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-7, 0);
        }
    }
}
