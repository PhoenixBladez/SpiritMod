using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.DarkfeatherMage.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.MagicMisc.Lightspire
{
	public class AkaviriStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lightspire");
			Tooltip.SetDefault("'Any amount of light is enough'");
            SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/MagicMisc/Lightspire/AkaviriStaff_Glow");
			Item.staff[Item.type] = true;
		}

        public override void SetDefaults()
		{
			Item.damage = 18;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 27;
			Item.mana = 7;
            Item.rare = ItemRarityID.Orange;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 1, 15, 0);
			Item.rare = ItemRarityID.Orange;
			Item.autoReuse = true;
			Item.shootSpeed = 9;
			Item.UseSound = SoundID.Item110;
			Item.shoot = ModContent.ProjectileType<DarkfeatherBoltRegular>();
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 45f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
        {
            for (int I = 0; I < Main.rand.Next(1, 3); I++)
            {
                int p = Projectile.NewProjectile(source, position.X, position.Y, velocity.X + ((float)Main.rand.Next(-120, 120) / 100), velocity.Y + ((float)Main.rand.Next(-120, 120) / 100), type, damage, knockback, player.whoAmI, 0f, 0f);
                Main.projectile[p].DamageType = DamageClass.Magic;
                Main.projectile[p].hostile = false;
                Main.projectile[p].friendly = true;
                Projectile projectile = Main.projectile[p];
                for (int k = 0; k < 10; k++)
                {
                    Vector2 mouse = Main.MouseWorld;
                    Vector2 offset = Vector2.Normalize(mouse - player.position) * 23f;
                    int dust = Dust.NewDust(projectile.Center + offset, projectile.width, projectile.height, DustID.ChlorophyteWeapon, 0f, 0f, 0, default, 1f);
                    Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(69, Main.LocalPlayer);
                    Main.dust[dust].velocity *= -1f;
                    Main.dust[dust].noGravity = true;
                    Vector2 vector2_1 = Vector2.Normalize(new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101)));
                    Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
                    Main.dust[dust].velocity = vector2_2;
                    vector2_2.Normalize();
                    Vector2 vector2_3 = vector2_2 * 34f;
                    Main.dust[dust].position = (projectile.Center + offset) - vector2_3;
                }
            }

			if (Main.rand.NextBool(6))
            {
                SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sound/CoilRocket"), player.Center);
                int proj = Projectile.NewProjectile(Item.GetSource_ItemUse(Item), position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<DarkfeatherBomb>(), damage, knockback*2, player.whoAmI);
                Main.projectile[proj].DamageType = DamageClass.Magic;
                Main.projectile[proj].hostile = false;
                Main.projectile[proj].friendly = true;
            }
            return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(Item.position, 0.08f, .38f, .24f);
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				ModContent.Request<Texture2D>("SpiritMod/Items/Sets/MagicMisc/Lightspire/AkaviriStaff_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
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
