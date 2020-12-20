
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.DarkfeatherMage.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;

namespace SpiritMod.Items.Weapon.Magic
{
	public class AkaviriStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lightspire");
			Tooltip.SetDefault("'Any amount of light is enough'");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Magic/AkaviriStaff_Glow");
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(item.position, 0.08f, .38f, .24f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                ModContent.GetTexture("SpiritMod/Items/Weapon/Magic/AkaviriStaff_Glow"),
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


        public override void SetDefaults()
		{
			item.damage = 18;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.magic = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 27;
			item.mana = 7;
            item.rare = 3;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 2;
			item.value = Terraria.Item.sellPrice(0, 1, 15, 0);
			item.rare = 3;
			item.autoReuse = true;
			item.shootSpeed = 9;
			item.UseSound = SoundID.Item110;
			item.shoot = ModContent.ProjectileType<DarkfeatherBoltRegular>();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY - 1)) * 45f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            for (int I = 0; I < Main.rand.Next(1, 3); I++)
            {
                int p = Projectile.NewProjectile(position.X, position.Y, speedX + ((float)Main.rand.Next(-180, 180) / 100), speedY + ((float)Main.rand.Next(-180, 180) / 100), type, damage, knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[p].magic = true;
                Main.projectile[p].hostile = false;
                Main.projectile[p].friendly = true;
                Projectile projectile = Main.projectile[p];
                for (int k = 0; k < 10; k++)
                {
                    Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
                    Vector2 offset = mouse - player.position;
                    offset.Normalize();
                    offset *= 23f;
                    int dust = Dust.NewDust(projectile.Center + offset, projectile.width, projectile.height, 157, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(69, Main.LocalPlayer);
                    Main.dust[dust].velocity *= -1f;
                    Main.dust[dust].noGravity = true;
                    //        Main.dust[dust].scale *= 2f;
                    Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    vector2_1.Normalize();
                    Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
                    Main.dust[dust].velocity = vector2_2;
                    vector2_2.Normalize();
                    Vector2 vector2_3 = vector2_2 * 34f;
                    Main.dust[dust].position = (projectile.Center + offset) - vector2_3;
                }
            }
			if (Main.rand.NextBool(6))
            {
                Main.PlaySound(SoundLoader.customSoundType, player.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/CoilRocket"));
                int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<DarkfeatherBomb>(), damage, knockBack*2, player.whoAmI);
                Main.projectile[proj].magic = true;
                Main.projectile[proj].hostile = false;
                Main.projectile[proj].friendly = true;
            }
            return false;
		}
	}
}
