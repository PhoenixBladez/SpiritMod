using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.BossLoot.MoonWizardDrops
{
	public class Moonshot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonshot");
			Tooltip.SetDefault("Drains energy from tiny lunazoas for ammo\n33% chance to not consume ammo\n'Aim for the stars!'\n'I wonder if the Arms Dealer can scrounge up some more lunazoas...'");
            SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/BossLoot/MoonWizardDrops/Moonshot_Glow");
        }

		public override void SetDefaults()
		{
			Item.damage = 24;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 24;
			Item.height = 24;
			Item.useTime = Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 3;
			Item.useTurn = false;
			Item.useAmmo = ModContent.ItemType<TinyLunazoaItem>();
			Item.UseSound = SoundID.Item96;
			Item.value = Item.sellPrice(0, 2, 30, 0);
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<LunazoaProj>();
			Item.shootSpeed = 13f;
		}

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(Item.position, 0.08f, .12f, .52f);
            Texture2D texture;
            texture = TextureAssets.Item[Item.type].Value;
            spriteBatch.Draw
            (
                ModContent.Request<Texture2D>("Items/BossLoot/MoonWizardDrops/Moonshot_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
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

		public override bool CanConsumeAmmo(Item item, Player player) => !Main.rand.NextBool(3);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			int numextraprojs = Main.rand.Next(0, 3);
			for(int i = 0; i <= numextraprojs; i++) {
				Vector2 vel = velocity * Main.rand.NextFloat(0.7f, 1.1f);
				Projectile proj = Projectile.NewProjectileDirect(source, player.Center, vel, type, damage, knockback, player.whoAmI, Main.rand.Next(1, 3), vel.ToRotation());
				proj.netUpdate = true;
			}
			return true;
		}

		public override void OnConsumeAmmo(Item item, Player player) => Gore.NewGorePerfect(Item.Source_ShootWithAmmo(player), player.Center, 2 * (-Vector2.UnitY * Main.rand.NextFloat(2, 3) - Vector2.UnitX * player.direction).RotatedByRandom(MathHelper.Pi / 16), Mod.Find<ModGore>("DrainedLunazoa").Type, Main.rand.NextFloat(0.5f, 0.8f));
		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
	}
}