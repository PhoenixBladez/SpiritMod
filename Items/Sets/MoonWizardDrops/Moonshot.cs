using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.MoonWizardDrops
{
	public class Moonshot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonshot");
			Tooltip.SetDefault("Drains energy from tiny lunazoas for ammo\n33% chance to not consume ammo\n'Aim for the stars!'\n'I wonder if the Arms Dealer can scrounge up some more lunazoas...'");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/MoonWizardDrops/Moonshot_Glow");
        }

		public override void SetDefaults()
		{
			item.damage = 24;
			item.ranged = true;
			item.width = 24;
			item.height = 24;
			item.useTime = item.useAnimation = 60;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 3;
			item.useTurn = false;
			item.useAmmo = ModContent.ItemType<TinyLunazoaItem>();
			item.UseSound = SoundID.Item96;
			item.value = Item.sellPrice(0, 2, 30, 0);
			item.rare = ItemRarityID.Green;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<LunazoaProj>();
			item.shootSpeed = 13f;
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(item.position, 0.08f, .12f, .52f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/Sets/MoonWizardDrops/Moonshot_Glow"),
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
		public override bool ConsumeAmmo(Player player) => !Main.rand.NextBool(3);
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numextraprojs = Main.rand.Next(0, 3);
			for(int i = 0; i <= numextraprojs; i++) {
				Vector2 vel = new Vector2(speedX, speedY) * Main.rand.NextFloat(0.7f, 1.1f);
				Projectile proj = Projectile.NewProjectileDirect(player.Center, vel, type, damage, knockBack, player.whoAmI, Main.rand.Next(1, 3), new Vector2(speedX, speedY).ToRotation());
				proj.netUpdate = true;
			}
			return true;
		}
		public override void OnConsumeAmmo(Player player) => Gore.NewGorePerfect(player.Center, 2 * (-Vector2.UnitY * Main.rand.NextFloat(2, 3) - Vector2.UnitX * player.direction).RotatedByRandom(MathHelper.Pi / 16), 
			mod.GetGoreSlot("Gores/DrainedLunazoa"), Main.rand.NextFloat(0.5f, 0.8f));
		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
	}
}